using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;
using KeyBoardInput;

public class MGetCheeseManager : CStateBase<MouseStateManager>
{
    public MGetCheeseManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        ExecuteEvents.Execute<IFadeInterfase>(
        target: this.m_cOwner.targetCamera.gameObject,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeOut());

    }

    public override void Execute()
    {
        Debug.Log("MouseState : Catch");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // 速度設定
        //m_cOwner.m_fmoveSpeed = m_cOwner.m_fDefaultSpeed;

        if (this.m_cOwner.targetCamera.gameObject.GetComponent<FadeEffect>().IsCompleteFlg)
        {
            // リスポーン処理
            RespawnPoint.Instance.Respawn(m_cOwner.gameObject);
            m_cOwner.ChangeState(0, EMouseState.Normal);

        }

    }

    public override void Exit()
    {
        ExecuteEvents.Execute<IFadeInterfase>(
        target: this.m_cOwner.targetCamera.gameObject,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeIn());

    }


    void Respawn()
    {
        ExecuteEvents.Execute<IFadeInterfase>(
                target: m_cOwner.targetCamera.gameObject,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.CallFadeOut());

        ScoreBoard.Instance.GetCheese();
        RespawnPoint.Instance.Respawn(m_cOwner.gameObject);
        ShiftOtherGoal.Instance.Shift(m_cOwner.m_GTargetBoxObject);
    }
}