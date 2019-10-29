using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMainManager : CStateBase<GameManager>
{
    public GameMainManager(GameManager _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : GameMainManager");

        var obj = ManagerObjectManager.Instance.GetGameObject("TimeManager");

        ExecuteEvents.Execute<ITimeManager>(
        target: obj,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.TimeStart());

    }
    public override void Execute()
    {
        if (Input.anyKeyDown)
        {
            this.m_cOwner.ChangeState(0, EGameState.End);
        }
    }

    public override void Exit()
    {

    }

}
