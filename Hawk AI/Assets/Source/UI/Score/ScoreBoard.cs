using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/****シングルトン化****/
public class ScoreBoard : SingletonMonoBehaviour<ScoreBoard>
{
    [SerializeField]
    private List<GameObject> CheeseIcon;
    private int RemainingCheese = 4;
    private Sprite Mouse;

    private void Start()
    {
        Mouse = Resources.Load<Sprite>("Mouse");
    }

    public void GetCheese()
    {
        RemainingCheese -= 1;
        if (RemainingCheese <= 0)
        {// State To Result
            RemainingCheese = 0;

            var obj = ManagerObjectManager.Instance.GetGameObject("GameManager");
            //ネズミ側勝利

            GameManager.IsHumanWin = false;

            ExecuteEvents.Execute<IGameInterface>(
            target: obj,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.ChangeState(EGameState.End));

            //    this.m_cOwner.ChangeState(0, EGameState.End);
        }
        //現状はアイコンの色を変えている、実際はテクスチャを変える
        CheeseIcon[RemainingCheese].GetComponent<Image>().sprite = Mouse;
    }
}
