using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;

public class GameMainManager : CStateBase<GameManager>
{
    public GameMainManager(GameManager _cOwner) : base(_cOwner) { }

    private GameObject m_cTimerObject = null;
    private bool m_bCountFlg = false;

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : GameMainManager");

        m_cTimerObject = ManagerObjectManager.Instance.GetGameObject("TimeManager");

        ExecuteEvents.Execute<ITimeManager>(
        target: m_cTimerObject,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.TimeStart());

        ExecuteEvents.Execute<ITimeManager>(
        target: m_cTimerObject,
        eventData: null,
        functor: (recieveTarget, y) =>  m_bCountFlg = recieveTarget.IsTimeCounting);

    }
    public override void Execute()
    {

        ExecuteEvents.Execute<ITimeManager>(
        target: m_cTimerObject,
        eventData: null,
        functor: (recieveTarget, y) => m_bCountFlg = recieveTarget.IsTimeCounting);


        if (m_bCountFlg == false)
        {
            Debug.Log("Counting!");
            this.m_cOwner.ChangeState(0, EGameState.End);
        }

        //if (Input.GetKeyDown(KeyCode.Alpha2) || (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any)))
        //{
        //    ExecuteEvents.Execute<ITimeManager>(
        //    target: m_cTimerObject,
        //    eventData: null,
        //    functor: (recieveTarget, y) => recieveTarget.TimeEnd());

        //    this.m_cOwner.ChangeState(0, EGameState.End);

        //}

    }

    public override void Exit()
    {

    }

}
