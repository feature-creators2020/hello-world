using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class NormalManager : CStateBase<MouseStateManager>
{

    public NormalManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        Debug.Log("State:Normal");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // 速度設定
        m_cOwner.m_fmoveSpeed = m_cOwner.m_fDefaultSpeed;

        // Debug:ステート変更
        if (GamePad.GetButtonDown(GamePad.Button.X, playerNo))
        {
            m_cOwner.ChangeState(0, EMouseState.SlowDown);
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, playerNo))
        {
            m_cOwner.ChangeState(0, EMouseState.Door);
        }
    }

    public override void Exit()
    {
        m_cOwner.EOldState = EMouseState.Normal;
    }

    void OnTriggerEnter(Collider other)
    {
        // トラップに当たる
        if (LayerMask.LayerToName(other.gameObject.layer) == "Trap") {
            // ネズミ捕り
            if (other.gameObject.tag == "Mousetrap")
            {
                m_cOwner.ChangeState(0, EMouseState.SlowDown);
            }
        }

        // ドアに当たる
        if (LayerMask.LayerToName(other.gameObject.layer) == "Door")
        {
            m_cOwner.ChangeState(0, EMouseState.Door);
        }
    }
}
