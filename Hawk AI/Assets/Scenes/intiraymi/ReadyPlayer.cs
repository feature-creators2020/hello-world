using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.EventSystems;
using KeyBoardInput;

public class ReadyPlayer : SingletonMonoBehaviour<ReadyPlayer>
{
    [SerializeField]
    private List<GameObject> GameReady;

    private void Update()
    {
        if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One) || KeyBoard.GetButtonDown(KeyBoard.Button.Start, KeyBoard.Index.One))
        {
            GameReady[0].SetActive(true);
            CheckReady();
        }
        if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Two) || KeyBoard.GetButtonDown(KeyBoard.Button.Start, KeyBoard.Index.Two))
        {
            GameReady[1].SetActive(true);
            CheckReady();
        }
        if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Three) || KeyBoard.GetButtonDown(KeyBoard.Button.Start, KeyBoard.Index.Three))
        {
            GameReady[2].SetActive(true);
            CheckReady();
        }
        if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Four) || KeyBoard.GetButtonDown(KeyBoard.Button.Start, KeyBoard.Index.Four))
        {
            GameReady[3].SetActive(true);
            CheckReady();
        }
    }

    private void CheckReady()
    {
        for(int i = 0; i < 4; i++)
        {
            if (!GameReady[i].activeSelf)
            {
                return;
            }
        }
        var obj = ManagerObjectManager.Instance.GetGameObject("FadeManager");
        ExecuteEvents.Execute<IFadeInterfase>(
        target: obj,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeOut());

        Invoke("ChangeScene", 1.0f);
    }

    private void ChangeScene()
    {
        var gameObject = ManagerObjectManager.Instance.GetGameObject("SceneManager");

        ExecuteEvents.Execute<ISceneInterfase>(
           target: gameObject,
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.GameMain));
        //Debug.Log("ChangeScene");
    }
}
