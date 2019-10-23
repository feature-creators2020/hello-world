using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using GamepadInput;

public class HDoorManager : CStateBase<HumanStateManager>
{
    public HDoorManager(HumanStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        Debug.Log("State:Door");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);
        var DoorScript = m_cOwner.GDoorData.GetComponent<Door>();

        // 速度設定
        if (m_cOwner.EOldState == EHumanState.SlowDown)
        {
            // 速度落とす
            m_cOwner.m_fmoveSpeed = m_cOwner.m_fDoorSpeed * m_cOwner.m_fSlowDownRate;

            // 経過時間処理
            m_cOwner.m_fSlowTime -= Time.deltaTime;
            // タイマーが過ぎてたら前の状態をnormalにする
            if (m_cOwner.m_fSlowTime <= 0f)
            {
                m_cOwner.EOldState = EHumanState.Normal;
            }
        }
        else
        {
            m_cOwner.m_fmoveSpeed = m_cOwner.m_fDoorSpeed;
        }

        // ドアを開閉する
        if (GamePad.GetButtonDown(GamePad.Button.B, playerNo))
        {
            Debug.Log("DoorClosing : " + DoorScript.isClosing);
                ExecuteEvents.Execute<IDoorInterface>(
                    target: m_cOwner.GDoorData,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.Open());
            
            Debug.Log("DoorOpening : " + DoorScript.isOpening);
            if (DoorScript.isClosing)
            {
                ExecuteEvents.Execute<IDoorInterface>(
                    target: m_cOwner.GDoorData,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.Close());
            }
        }

        // Debug:ステート変更
        if (GamePad.GetButtonDown(GamePad.Button.X, playerNo))
        {
            m_cOwner.ChangeState(0, EHumanState.Normal);
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, playerNo))
        {
            m_cOwner.ChangeState(0, EHumanState.SlowDown);
        }

    }

    public override void Exit()
    {
        //m_cOwner.EOldState = EMouseState.Door;
    }

}
