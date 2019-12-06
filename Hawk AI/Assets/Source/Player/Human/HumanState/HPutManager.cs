using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using KeyBoardInput;


public class HPutManager : CStateBase<HumanStateManager>
{
    public HPutManager(HumanStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        m_cOwner.PlayAnimation(EHumanAnimation.Put);
    }

    public override void Execute()
    {
        //Debug.Log("State:Up");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);
        var playerKeyNo = (KeyBoard.Index)playerNo;
        var keyboardState = KeyBoard.GetState(m_cOwner.KeyboardIndex, false);

        // ゲームパッドの入力情報取得
        m_cOwner.inputHorizontal = 0f;
        m_cOwner.inputVertical = 0f;

        m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        m_cOwner.inputVertical = keyState.LeftStickAxis.y;

        m_cOwner.UseItem(playerNo, playerKeyNo);
    }

    public override void Exit()
    {
        
    }

}
