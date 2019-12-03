using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarTelBox : CStateBase<Star>
{

    Vector3 m_vtrans = Vector3.zero;        // 移動用ベクトル

    public StarTelBox(Star _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        // 初期化
        m_cOwner.AddVelocity(new Vector3(0.0f, 0.0f, 0.0f));
        m_vtrans = m_cOwner.TelBox.transform.position;
        m_vtrans.y = m_cOwner.transform.position.y;
        m_cOwner.transform.position = m_vtrans;

        // この処理のメイン
        m_cOwner.StarOrdinaly = !m_cOwner.StarOrdinaly;
    }

    public override void Execute()
    {
        // スターと清掃員のアクティブを切り替え
        if (m_cOwner.StarOrdinaly)
        {
            m_cOwner.StarActive(true);
            m_cOwner.StaffActive(false);
        }
        else
        {
            m_cOwner.StarActive(false);
            m_cOwner.StaffActive(true);
        }

        // 出るときの演出とサウンド
        if ((Input.GetButtonDown("Telbox") || Input.GetKeyDown(KeyCode.Return)))
        {
            ExecuteEvents.Execute<ITellBoxInterface>(
               target: m_cOwner.TelBox,
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.OnBoxExit());
            Vector3 pos = m_cOwner.transform.position;
            pos.z = 0.0f;
            m_cOwner.transform.position = pos;
            m_cOwner.TelboxIn = false;
            m_cOwner.ChangeState(0, StarState.Wait);
        }
    }

    public override void Exit()
    {
        //サウンド再生
        ExecuteEvents.Execute<IAudioInterface>(
           target: GameObject.Find("StarAudio"),
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.Play((int)StarAudioType.Transform));
        Debug.Log("Play Sound : StarAudioType.Transform");

        m_cOwner.StarArrayPlay();
    }
}
