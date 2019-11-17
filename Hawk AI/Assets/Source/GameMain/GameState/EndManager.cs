using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using GamepadInput;

public class EndManager : CStateBase<GameManager>
{
    public EndManager(GameManager _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : EndManager");

        var obj = ManagerObjectManager.Instance.GetGameObject("FadeManager");
        ExecuteEvents.Execute<IFadeInterfase>(
        target: obj,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeIn());
    }
    public override void Execute()
    {
        //if ((Input.anyKeyDown) || (GamePad.GetButtonDown(GamePad.Button.B,GamePad.Index.Any)))
        //{
        //    var Object = ManagerObjectManager.Instance.GetGameObject("SceneManager");

        //    ExecuteEvents.Execute<ISceneInterfase>(
        //     target: Object,
        //     eventData: null,
        //     functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.GameMain));

        //    //            this.m_cOwner.ChangeState(0, EGameState.Ready);
        //}
    }

    public override void Exit()
    {

    }
}
