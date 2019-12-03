using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarLanding : CStateBase<Star>
{

    private float m_fElapsedTime = 0.0f;            // 経過時間
    private float m_fAnimationLength = 0.0f;        // アニメーション時間の入れ子
    private Vector3 m_vInitVec = Vector3.zero;      // 初期化ベクトル

    public StarLanding(Star _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        m_cOwner.FadeStarAnimation(StarAnimation.Landing);

        // 初期化
        m_fAnimationLength = m_cOwner.AnimationClipLength / 2;
        m_cOwner.SetStarAnimationState(2, 0);
        m_fElapsedTime = 0.0f;

        // エフェクト生成
        m_vInitVec = m_cOwner.transform.position;
        m_vInitVec.y -= 0.45f;
        GameObject.Instantiate(m_cOwner.GetEffect(EffectType.LANDING), m_vInitVec, Quaternion.identity);

        //サウンド再生
        ExecuteEvents.Execute<IAudioInterface>(
           target: GameObject.Find("StarAudio"),
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.Play((int)StarAudioType.Landing));

        m_cOwner.GetComponent<Rigidbody>().isKinematic = false;
    }

    public override void Execute()
    {

        var speed = Input.GetAxis("Horizontal");

        // アニメーションが終わったらステート遷移
        if (m_fElapsedTime > m_fAnimationLength)
        {
            if (speed != 0.0f)
            {
                m_cOwner.ChangeState(0, StarState.Walk);
                return;
            }
            m_cOwner.ChangeState(0, StarState.Wait);
        }

        m_fElapsedTime += Time.deltaTime;
    }

    public override void Exit()
    {
        m_cOwner.SetStarAnimationState(1, m_cOwner.AnimationClipLength);
    }
}
