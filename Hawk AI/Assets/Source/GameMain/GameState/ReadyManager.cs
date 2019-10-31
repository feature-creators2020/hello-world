using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;

public class ReadyManager : CStateBase<GameManager>
{
    public ReadyManager(GameManager _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : ReadyManager");

        var obj = GameObject.Find("FadeManager");
        ExecuteEvents.Execute<IFadeInterfase>(
        target: obj,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeStay());

    }
    public override void Execute()
    {
        if ((Input.anyKeyDown) || (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any)))
        {
            this.m_cOwner.ChangeState(0, EGameState.Main);
        }
    }

    public override void Exit()
    {
        var obj = ManagerObjectManager.Instance.GetGameObject("FadeManager");

        ExecuteEvents.Execute<IFadeInterfase>(
        target: obj,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeIn());

    }
}
