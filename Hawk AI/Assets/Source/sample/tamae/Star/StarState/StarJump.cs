using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarJump : CStateBase<Star>
{

    private int m_nJumpCount = 0;           // ジャンプ時間のカウント
    private int m_nHitLayer = 0;            // レイヤー設定

    private float m_fJumpUpper = 0.0f;      // ジャンプの1フレームの上昇量
    private float m_fJumpTime = 0.0f;       // ジャンプ時間
    private float m_fElapsedTime = 0.0f;    // ステートに入ってからの経過時間
    private float m_fAnimationLength = 0.0f;// アニメーションの時間を入れ込む
    private float m_fGroundDintance = 0.0f; // 地面までの距離

    private Vector3 vec = Vector3.zero;     // 保存用ベクトル
    private Vector3 m_vOffset = new Vector3(0.0f, 0.495f, 0.0f);    // Ray飛ばすようオフセット
    private Vector3 m_vInitvec = Vector3.zero;                      // 座標初期化ベクトル

    private bool m_bDurationFlag = false;   // ジャンプの上昇中か滞空中のフラグ
    private bool m_bRoofFlag = false;       // 天井に当たっているか当たっていないかのフラグ

    public StarJump(Star _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        // 元オブジェクトから設定
        m_fJumpTime = m_cOwner.Starjumptime;                    
        m_fJumpUpper = m_cOwner.Starjumpheight / m_fJumpTime;
        m_fJumpTime = m_cOwner.Starjumptime * 60;

        // 初期化
        m_nJumpCount = 0;
        m_bDurationFlag = false;
        m_fElapsedTime = 0.0f;
        m_fAnimationLength = m_cOwner.AnimationClipLength;      // アニメーションの再生時間を設定

        m_cOwner.PlayStarAnimation(StarAnimation.JumpUp);

        //エフェクト再生
        m_vInitvec = m_cOwner.transform.position;
        m_vInitvec.y -= 0.45f;
        GameObject.Instantiate(m_cOwner.GetEffect(EffectType.JUMP), m_vInitvec, Quaternion.identity);

        //サウンド再生
        ExecuteEvents.Execute<IAudioInterface>(
           target: GameObject.Find("StarAudio"),
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.Play((int)StarAudioType.Jumping));

        m_cOwner.GetComponent<Rigidbody>().isKinematic = false;
    }

    public override void Execute()
    {
        var speed = Input.GetAxis("Horizontal") * m_cOwner.StarWalkSpeed;
        RaycastHit hit;

        // アニメーションを一回だけ再生する、再生されたらストップ
        if (m_fElapsedTime > m_fAnimationLength)
        {
            m_fElapsedTime = 0.0f;
            m_cOwner.StopAnimation();
        }

        m_bRoofFlag = false;

        // Rayで上との当たり判定をとり跳ね返る
        m_nHitLayer = LayerMask.GetMask(new string[] { "Stage", "MoveBlock" });
        var isRayHit = Physics.Raycast(m_cOwner.transform.position + m_vOffset, Vector3.Normalize(m_cOwner.transform.TransformDirection(Vector3.up)), out hit, 100.0f, m_nHitLayer);
        if (isRayHit)
        {
            m_fGroundDintance = hit.distance;
        }
        else
        {
            // 当たっていない場合確実に当たっていない値を入れる
            m_fGroundDintance = float.MaxValue;
        }

        if (m_fGroundDintance < m_fJumpUpper / 90)
        {
            m_cOwner.ChangeState(0, StarState.Falling);
        }

        // ジャンプ時間計測
        if (m_fJumpTime > m_nJumpCount)
        {
            // 経過時間によって上昇量、アニメーションを変更
            if ((m_fJumpTime / 3 * 1) < m_nJumpCount && !m_bDurationFlag)
            {
                m_cOwner.PlayStarAnimation(StarAnimation.JumpDuration);
            }
            if ((m_fJumpTime / 5 * 4) < m_nJumpCount)
            {
                m_fJumpUpper = m_fJumpUpper / 2;
            }

            // 移動量たし込み
            vec = m_cOwner.transform.position;
            vec.y += m_fJumpUpper / 90;
            vec.x += speed / 120;
            m_cOwner.transform.position = vec;

            m_nJumpCount++;
        }
        else
        {
            m_cOwner.ChangeState(0, StarState.Falling);
        }

        m_fElapsedTime += Time.deltaTime;
        m_cOwner.Direction();
    }

    public override void Exit()
    {

    }
}
