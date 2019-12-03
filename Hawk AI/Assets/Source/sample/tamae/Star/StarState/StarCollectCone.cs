using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarCollectCone : CStateBase<Star>
{

    private float   m_fElapsedTime = 0.0f;      // 経過時間
    private float   m_fAnimationTime = 0.0f;    // コーン回収アニメーション時間入れ子

    private bool    m_bPutFlag = true;          // おいています

    private Vector3 m_vTargetPos = Vector3.zero;// 位置補間用ベクトル

    public StarCollectCone(Star _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        m_cOwner.SetStarAnimationState(-1, m_cOwner.GetAnimationClipLength(StarAnimation.PutCone));     // アニメーションのステート設定
        m_cOwner.PlayStarAnimation(StarAnimation.PutCone);

        // 初期化
        m_fAnimationTime = m_cOwner.AnimationClipLength;
        m_fElapsedTime = 0.0f;
        m_cOwner.AddVelocity(new Vector3(0.0f, 0.0f, 0.0f));
        m_bPutFlag = true;
        m_vTargetPos = m_cOwner.transform.position;
        m_vTargetPos.x = m_cOwner.ColorCone.transform.position.x;
        if (m_cOwner.RightLeft)
        {
            m_vTargetPos.x = m_vTargetPos.x - 0.4f;
        }
        else
        {
            m_vTargetPos.x = m_vTargetPos.x + 0.4f;
        }
    }

    public override void Execute()
    {

        m_cOwner.PlayStarAnimation(StarAnimation.PutCone);

        m_cOwner.transform.position = Vector3.Lerp(m_cOwner.transform.position, m_vTargetPos, m_fElapsedTime);

        // アニメーションに合わせてコーンを回収
        if(m_fElapsedTime > m_fAnimationTime / 3 * 2 && m_bPutFlag)
        {
            m_cOwner.CollectCone();

            //サウンド再生
            ExecuteEvents.Execute<IAudioInterface>(
            target: GameObject.Find("StarAudio"),
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.Play((int)StarAudioType.SettingCorn));

            m_bPutFlag = false;
        }

        // アニメーションの時間が終わったら遷移
        if(m_fElapsedTime >= m_fAnimationTime)
        {
            m_cOwner.ChangeState(0, StarState.Wait);
        }

        m_fElapsedTime += Time.deltaTime;
    }

    public override void Exit()
    {
        // AnimationStateをもとに戻してあげる
        m_cOwner.SetStarAnimationState(1, m_cOwner.AnimationClipLength);
    }
}
