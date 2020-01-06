using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/****シングルトン化****/
public class ScoreBoard : SingletonMonoBehaviour<ScoreBoard>
{
    [SerializeField]
    private List<GameObject> CheeseIcon = new List<GameObject>();
    private int RemainingCheese = 4;
    private Sprite Mouse;

    private void Start()
    {
        Mouse = Resources.Load<Sprite>("love");
    }

    public void GetCheese()
    {
        RemainingCheese -= 1;
        if (RemainingCheese <= 0)
        {// State To Result
            RemainingCheese = 0;

            //ネズミ側勝利
            GameManager.IsHumanWin = false;
            CountDownAnimation.Instance.SetFinish(true);

            Invoke("End", 1.0f);
            //    this.m_cOwner.ChangeState(0, EGameState.End);
        }
        //現状はアイコンの色を変えている、実際はテクスチャを変える
        CheeseIcon[RemainingCheese].GetComponent<Image>().sprite = Mouse;
    }

    void End()
    {
        var obj = ManagerObjectManager.Instance.GetGameObject("GameManager");
        ExecuteEvents.Execute<IGameInterface>(
            target: obj,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.ChangeState(EGameState.End));
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            Invoke("Retry", 1.0f);
        }
    }

    void Retry()
    {
        var gameObject = ManagerObjectManager.Instance.GetGameObject("SceneManager");

        ExecuteEvents.Execute<ISceneInterfase>(
           target: gameObject,
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.Tutorial));
    }
}
