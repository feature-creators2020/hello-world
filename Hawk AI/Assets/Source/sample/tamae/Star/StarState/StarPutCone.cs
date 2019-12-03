using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class StarPutCone : CStateBase<Star>
{

    private float m_fElapsedTime = 0.0f;        // 経過時間 
    private float m_fAnimationLength = 0.0f;    // アニメーションの時間入れ子   

    private bool m_bPutFlag = true;             // おいているかどうかフラグ

    private Vector3 vec = Vector3.zero;         // 初期化用ベクトル
    private Vector3 m_vTargetPos =Vector3.zero; // 位置補間用ベクトル
    private Vector3 m_vInitPos = Vector3.zero;  // 位置保存用ベクトル

    public StarPutCone(Star cOwner) : base(cOwner) { }

    public override void Enter()
    {
        // 初期化
        m_cOwner.AddVelocity(vec);
        m_fElapsedTime = 0.0f;
        m_bPutFlag = true;

        m_cOwner.PlayStarAnimation(StarAnimation.PutCone);

        m_vTargetPos = new Vector3(-(MapManager.Instance.InitMapData[0].Length / 2) + m_cOwner.Horizontal, m_cOwner.transform.position.y, 0.0f);

        if (m_cOwner.RightLeft)
        {
            m_vTargetPos.x = m_vTargetPos.x - 0.4f;
        }
        else
        {
            m_vTargetPos.x = m_vTargetPos.x + 0.4f;
        }
        
        m_fAnimationLength = m_cOwner.AnimationClipLength;
    }

    public override void Execute()
    {

        m_cOwner.transform.position = Vector3.Lerp(m_cOwner.transform.position, m_vTargetPos, m_fElapsedTime);

        // アニメーション時間に合わせてコーンを置く
        if (m_fElapsedTime > m_fAnimationLength / 3 * 1 && m_bPutFlag)
        {
            // コーンを生成する位置を決定、生成
            MapManager.Instance.FrontMapData[m_cOwner.Vertical][m_cOwner.Horizontal] = 4;
            MapManager.Instance.CreateObject(m_cOwner.Vertical, m_cOwner.Horizontal, ObjectNo.ColorCone);

            // 位置補正

            //サウンド再生
            ExecuteEvents.Execute<IAudioInterface>(
               target: GameObject.Find("StarAudio"),
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.Play((int)StarAudioType.SettingCorn));
            
            m_bPutFlag = false;
        }

        // アニメーションの時間を超えるとステート遷移
        if (m_fElapsedTime >= m_fAnimationLength)
        {
            m_cOwner.ChangeState(0, StarState.Wait);
        }

        m_fElapsedTime += Time.deltaTime;
    }

    public override void Exit()
    {
        m_cOwner.transform.position = m_vTargetPos;
    }

}
