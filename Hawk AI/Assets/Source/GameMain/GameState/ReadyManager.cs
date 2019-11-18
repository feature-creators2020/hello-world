using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using GamepadInput;

public class ReadyManager : CStateBase<GameManager>
{
    public ReadyManager(GameManager _cOwner) : base(_cOwner) { }

    private float m_fStartTime = 0;

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : ReadyManager");

        var obj = ManagerObjectManager.Instance.GetGameObject("FadeManager");
        ExecuteEvents.Execute<IFadeInterfase>(
        target: obj,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeIn());
        m_fStartTime = 5.0f;
    }
    public override void Execute()
    {
        //if ((Input.anyKeyDown) || (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any)))
        //{
        //    this.m_cOwner.ChangeState(0, EGameState.Main);
        //}

        if(SceneManager.GetActiveScene().name == "GameMain")
        {
            m_fStartTime -= Time.deltaTime;
            CountDownAnimation.Instance.SetCount3(m_fStartTime);

            if (m_fStartTime <= 0)
            {
                this.m_cOwner.ChangeState(0, EGameState.Main);
                CountDownAnimation.Instance.SetCount3(5.0f);
            }
        }
        else
        {
            this.m_cOwner.ChangeState(0, EGameState.Main);
        }
        
        //else if(m_fStartTime <= 1)
        //{
        //    Debug.Log("1");
        //}
        //else if (m_fStartTime <= 2)
        //{
        //    Debug.Log("2");
        //}
        //else if (m_fStartTime <= 3)
        //{
        //    Debug.Log("3");
        //}
    }

    public override void Exit()
    {
        //var obj = ManagerObjectManager.Instance.GetGameObject("FadeManager");

        //ExecuteEvents.Execute<IFadeInterfase>(
        //target: obj,
        //eventData: null,
        //functor: (recieveTarget, y) => recieveTarget.CallFadeIn());

    }
}
