using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;
using KeyBoardInput;
using UnityEngine.SceneManagement;

public class GameMainManager : CStateBase<GameManager>
{
    public GameMainManager(GameManager _cOwner) : base(_cOwner) { }

    private GameObject m_cBGMAudioObj = null;
    private GameObject m_cTimerObject = null;
    private bool m_bCountFlg = false;

    // Start is called before the first frame update
    public override void Enter()
    {

        Debug.Log(" NowState : GameMainManager");

        m_cTimerObject = ManagerObjectManager.Instance.GetGameObject("TimeManager");
        m_cBGMAudioObj = ManagerObjectManager.Instance.GetGameObject("BGMAudio");

        ExecuteEvents.Execute<ITimeManager>(
        target: m_cTimerObject,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.TimeStart());

        ExecuteEvents.Execute<ITimeManager>(
        target: m_cTimerObject,
        eventData: null,
        functor: (recieveTarget, y) =>  m_bCountFlg = recieveTarget.IsTimeCounting);

        SwitchingStart();
    }
    public override void Execute()
    {

        SwitchingUpdate();

        #region debug
        //if (Input.GetKeyDown(KeyCode.Alpha2) || (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any)))
        //{
        //    ExecuteEvents.Execute<ITimeManager>(
        //    target: m_cTimerObject,
        //    eventData: null,
        //    functor: (recieveTarget, y) => recieveTarget.TimeEnd());

        //    this.m_cOwner.ChangeState(0, EGameState.End);

        //}
        #endregion //debug
    }

    public override void Exit()
    {

    }

    public void SwitchingStart()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":

                ExecuteEvents.Execute<IAudioInterface>(
                target: m_cBGMAudioObj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Play((int)BGMAudioType.Title));

                break;

            case "Tutorial":

                ExecuteEvents.Execute<IAudioInterface>(
                target: m_cBGMAudioObj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Play((int)BGMAudioType.Main));

                break;


            case "GameMain":

                ExecuteEvents.Execute<IAudioInterface>(
                target: m_cBGMAudioObj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Play((int)BGMAudioType.Main));

                GameManager.EatCountByMouse1 = 0;
                GameManager.EatCountByMouse2 = 0;
                GameManager.KillCountByHuman1 = 0;
                GameManager.KillCountByHuman2 = 0;

                break;

            case "Result":

                ExecuteEvents.Execute<IAudioInterface>(
                target: m_cBGMAudioObj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Play((int)BGMAudioType.Result));



                break;

            default:


                break;
        }
    }


    public void SwitchingUpdate()
    {

        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":

                if ((Input.anyKeyDown) || (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any)))
                {
                    this.m_cOwner.ChangeState(0, EGameState.End);
                }


                break;


            case "GameMain":

                ExecuteEvents.Execute<ITimeManager>(
                target: m_cTimerObject,
                eventData: null,
                functor: (recieveTarget, y) => m_bCountFlg = recieveTarget.IsTimeCounting);


                if (m_bCountFlg == false)
                {
                    Debug.Log("Counting!");
                    //人間側勝利
                    CountDownAnimation.Instance.SetFinish(true);
                    this.m_cOwner.ChangeState(0, EGameState.End);
                }

                break;

            case "Result":

                //float NowTimeCount = 0f;

                //ExecuteEvents.Execute<ITimeManager>(
                //target: m_cTimerObject,
                //eventData: null,
                //functor: (recieveTarget, y) => NowTimeCount = recieveTarget.ExecuteTime);


                //if(NowTimeCount >= this.m_cOwner.ResultStatingTime)
                //{
                //    ExecuteEvents.Execute<ISceneInterfase>(
                //    target: ManagerObjectManager.Instance.GetGameObject("SceneManager"),
                //    eventData: null,
                //    functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.Title));
                //}

                //if ((KeyBoard.GetButtonDown(KeyBoard.Button.Start, KeyBoard.Index.Any)) || (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any)))
                //{
                //    ExecuteEvents.Execute<ISceneInterfase>(
                //    target: ManagerObjectManager.Instance.GetGameObject("SceneManager"),
                //    eventData: null,
                //    functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.Title));
                //}

                //this.m_cOwner.ChangeState(0, EGameState.End);


                break;

            default:


                break;

        }

    }

}
