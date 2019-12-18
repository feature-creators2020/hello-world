using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.EventSystems;
using KeyBoardInput;

public class HVarsanDownManager : CStateBase<HumanStateManager>
{
    public HVarsanDownManager(HumanStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        // ダウン開始時のアニメーションを再生させる
        m_cOwner.PlayAnimation(EHumanAnimation.VarsanDown_Start);
    }

    public override void Execute()
    {
      //  Debug.Log("State:VarsanDown");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);
        var playerKeyNo = (KeyBoard.Index)playerNo;
        var keyboardState = KeyBoard.GetState(m_cOwner.KeyboardIndex, false);


    }

    public override void Exit()
    {
        m_cOwner.EOldState = EHumanState.Normal;
    }

}
