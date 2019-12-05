using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HForcedWaitManager : CStateBase<HumanStateManager>
{
    public HForcedWaitManager(HumanStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        //Debug.Log("State:ForcedWait");

        EGameState eGameState = 0;
        ExecuteEvents.Execute<IGameInterface>(
        target: ManagerObjectManager.Instance.GetGameObject("GameManager"),
        eventData: null,
        functor: (recieveTarget, y) => eGameState =  recieveTarget.GetGameState());

        
        if (eGameState == EGameState.Main)
        {
            this.m_cOwner.ChangeState(0, EHumanState.Normal);
        }

    }

    public override void Exit()
    {
        m_cOwner.EOldState = EHumanState.Normal;
    }
}
