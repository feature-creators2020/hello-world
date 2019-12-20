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
    private GameObject State;
    [SerializeField]
    private List<Sprite> Numbers = new List<Sprite>();
    [SerializeField]
    private List<Sprite> StateIcon = new List<Sprite>();
    private int RemainingMouse = 10;
    private bool m_bIsNight;
    private int m_DontRespawnCount;     // ネズミがリスポーンできなかった回数

    //Start is called before the first frame update
    void Start()
    {
        Life.GetComponent<Image>().sprite = Numbers[RemainingMouse];
        State.GetComponent<Image>().sprite = StateIcon[0];
        m_bIsNight = false;
    }

    //TODO--killされたら1度だけこの処理を動かす
    public void GetCaught()
    {
        RemainingMouse -= 1;
        // ネズミの残機がなくなった
        if(RemainingMouse <= 0)
        {
            RemainingMouse = 0;

            m_DontRespawnCount++;
            // ネズミが2匹とも捕まった
            //if (m_DontRespawnCount >= 2)
            {

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
        }

        Life.GetComponent<Image>().sprite = Numbers[RemainingMouse];
    }

    private void Retry()
    {
        var gameObject = ManagerObjectManager.Instance.GetGameObject("SceneManager");

        ExecuteEvents.Execute<ISceneInterfase>(
           target: gameObject,
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.Tutorial));
    }

    public void IsNight(bool TorF)
    {
        m_bIsNight = TorF;
    }

    public void ChangeIconState(int num)
    {
        State.GetComponent<Image>().sprite = StateIcon[num + 1];
        Invoke("RemoveState", 5.0f / num);
    }

    public void RemoveState()
    {
        if (m_bIsNight)
        {
            State.GetComponent<Image>().sprite = StateIcon[1];
        }
        else
        {
            State.GetComponent<Image>().sprite = StateIcon[0];
        }
    }

    public int GetRemainingMouse()
    {
        return RemainingMouse;
    }
}
