using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MouseLifeBoard : SingletonMonoBehaviour<MouseLifeBoard>
{
    [SerializeField]
    private GameObject Life;
    [SerializeField]
    private List<Sprite> Numbers = new List<Sprite>();
    private int RemainingMouse = 8;

    //Start is called before the first frame update
    void Start()
    {
        Life.GetComponent<Image>().sprite = Numbers[RemainingMouse];
    }

    public void GetCaught()
    {
        RemainingMouse -= 1;
        if(RemainingMouse < 0)
        {
            RemainingMouse = 0;

            var obj = ManagerObjectManager.Instance.GetGameObject("GameManager");

            //人間側勝利
            GameManager.IsHumanWin = true;

            ExecuteEvents.Execute<IGameInterface>(
            target: obj,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.ChangeState(EGameState.End));
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                Invoke("Retry", 1.0f);
            }
        }

        Life.GetComponent<Image>().sprite = Numbers[RemainingMouse];
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
