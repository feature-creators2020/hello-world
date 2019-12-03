using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFalling : CStateBase<Star>
{

    Vector3 vec = Vector3.zero;             // 更新用ベクトル
   
    private float m_fLandingHeight;         // 着地判定用
    private float m_fFallPow = 0.0f;        // 落下スピード
    private float m_fFallDistance = 0.0f;   // 落下距離

    private float m_fAnimationTime = 0.0f;  // アニメーション時間入れ子
    private float m_fElapsedTime = 0.0f;    // 経過時間
    
    public StarFalling(Star _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        // 変数初期化
        m_fFallPow = 0.0f;
        m_fFallDistance = m_cOwner.transform.position.y;
        m_fLandingHeight = 1.3f;

        // スター状態の時は着地判定を二倍に
        if (m_cOwner.StarOrdinaly)
        {
            m_fLandingHeight = m_fLandingHeight * 2;
        }

        // アニメーション再生、
        m_cOwner.FadeStarAnimation(StarAnimation.Fall);
        m_fAnimationTime = m_cOwner.AnimationClipLength;
    }

    public override void Execute()
    {
        var speed = Input.GetAxis("Horizontal") * m_cOwner.StarWalkSpeed;

        // アニメーションの再生を止める
        if(m_fElapsedTime > m_fAnimationTime)
        {
            m_cOwner.StopAnimation();
        }

        // 右左の判定
        if (speed < 0)
        {
            m_cOwner.RightLeft = false;
        }
        else if (speed > 0)
        {
            m_cOwner.RightLeft = true;
        }
        
        m_cOwner.Direction();

        // 地面までの距離を判定
        if (!m_cOwner.CheckGroundDintance())
        {
            // 落下スピード設定、降下
            m_fFallPow -= m_cOwner.StarFallSpeed;
            vec.y = m_fFallPow;
            vec.x = speed;
            m_cOwner.AddVelocity(vec);
        }
        else
        {
            // 着地判定距離内だと着地処理、それ以外はそれ以外
            if (m_fFallDistance - m_cOwner.transform.position.y > m_fLandingHeight)
            {
                m_cOwner.ChangeState(0, StarState.Landimg);
            }
            else
            {
                m_cOwner.ChangeState(0, StarState.Wait);
            }
        }
        // 経過時間を計測
        m_fElapsedTime += Time.deltaTime;
    }

    public override void Exit()
    {

    }
}
