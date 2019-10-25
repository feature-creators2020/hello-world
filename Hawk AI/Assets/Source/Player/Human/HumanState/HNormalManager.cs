using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class HNormalManager : CStateBase<HumanStateManager>
{
    public HNormalManager(HumanStateManager _cOwner) : base(_cOwner) { }

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

        // アイテム使用
        if(GamePad.GetButtonDown(GamePad.Button.B, playerNo))
        {
            m_cOwner.UseItem();
        }

        // Debug:ステート変更
        if (GamePad.GetButtonDown(GamePad.Button.X, playerNo))
        {
            m_cOwner.ChangeState(0, EHumanState.SlowDown);
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, playerNo))
        {
            m_cOwner.ChangeState(0, EHumanState.Door);
        }
    }

    public override void Exit()
    {
        m_cOwner.EOldState = EHumanState.Normal;
    }

}
