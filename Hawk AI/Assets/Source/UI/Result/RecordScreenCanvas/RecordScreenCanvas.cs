using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ERecordScreenChild
{
    eBackScreen,
    ePlayer1,
    ePlayer2,
    ePlayer3,
    ePlayer4,
}

public enum ERecordPlayerType
{
    eHuman1,
    eHuman2,
    eMouse1,
    eMouse2
}

public class RecordScreenCanvas : MonoBehaviour, IResultManagerInterfase
{

    [SerializeField]
    private List<Sprite> HumanIcon = new List<Sprite>();

    [SerializeField]
    private List<Sprite> MouseIcon = new List<Sprite>();

    [SerializeField]
    private Sprite RecordMouseIcon;

    [SerializeField]
    private Sprite RecordHumanIcon;

    [SerializeField]
    private List<GameObject> PlayerRecordObj = new List<GameObject>();

    private int First = 0;
    private int Seconed = 1;
    private int Third = 2;
    private int Forth = 3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HawkAIWin()
    {
        SortingHumanWin();
        

    }


    public void MouseWin()
    {
        SortingMouseWin();

    }


    private void SortingHumanWin()
    {
        Image Player1Image = PlayerRecordObj[4].transform.GetChild(0).gameObject.GetComponent<Image>();
        Image Player2Image = PlayerRecordObj[4].transform.GetChild(1).gameObject.GetComponent<Image>();
        Image Player3Image = PlayerRecordObj[4].transform.GetChild(2).gameObject.GetComponent<Image>();
        Image Player4Image = PlayerRecordObj[4].transform.GetChild(3).gameObject.GetComponent<Image>();


        // Decide First n Seconed.
        if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
        {
            Player1Image.sprite = HumanIcon[0];
            RecordPopUp(First, ERecordPlayerType.eHuman1);

            Player2Image.sprite = HumanIcon[1];
            RecordPopUp(Seconed, ERecordPlayerType.eHuman2);

        }
        else
        {
            Player1Image.sprite = HumanIcon[1];
            RecordPopUp(First, ERecordPlayerType.eHuman2);

            Player2Image.sprite = HumanIcon[0];
            RecordPopUp(Seconed, ERecordPlayerType.eHuman1);
        }

        // Decide Third n Forth.
        if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
        {
            Player3Image.sprite = MouseIcon[0];
            RecordPopUp(Third, ERecordPlayerType.eMouse1);

            Player4Image.sprite = MouseIcon[1];
            RecordPopUp(Forth, ERecordPlayerType.eMouse2);

        }
        else
        {
            Player3Image.sprite = MouseIcon[1];
            RecordPopUp(Third, ERecordPlayerType.eMouse2);

            Player4Image.sprite = MouseIcon[0];
            RecordPopUp(Forth, ERecordPlayerType.eMouse1);
        }
    }

    private void SortingMouseWin()
    {
        Image Player1Image = PlayerRecordObj[4].transform.GetChild(0).gameObject.GetComponent<Image>();
        Image Player2Image = PlayerRecordObj[4].transform.GetChild(1).gameObject.GetComponent<Image>();
        Image Player3Image = PlayerRecordObj[4].transform.GetChild(2).gameObject.GetComponent<Image>();
        Image Player4Image = PlayerRecordObj[4].transform.GetChild(3).gameObject.GetComponent<Image>();


        // Decide First n Seconed.
        if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
        {
            Player1Image.sprite = MouseIcon[0];
            RecordPopUp(First, ERecordPlayerType.eMouse1);

            Player2Image.sprite = MouseIcon[1];
            RecordPopUp(Seconed, ERecordPlayerType.eMouse2);

        }
        else
        {
            Player1Image.sprite = MouseIcon[1];
            RecordPopUp(First, ERecordPlayerType.eMouse2);

            Player2Image.sprite = MouseIcon[0];
            RecordPopUp(Seconed, ERecordPlayerType.eMouse1);
        }

        // Decide Third n Forth.
        if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
        {
            Player3Image.sprite = HumanIcon[0];
            RecordPopUp(Third, ERecordPlayerType.eHuman1);

            Player4Image.sprite = HumanIcon[1];
            RecordPopUp(Forth, ERecordPlayerType.eHuman2);
        }
        else
        {
            Player3Image.sprite = HumanIcon[1];
            RecordPopUp(Third, ERecordPlayerType.eHuman2);

            Player4Image.sprite = HumanIcon[0];
            RecordPopUp(Forth, ERecordPlayerType.eHuman1);
        }
    }

    private void RecordPopUp(int Rank, ERecordPlayerType _RecordPlayerType)
    {
        Vector3 Position = Vector3.zero;
        switch (_RecordPlayerType)
        {
            case ERecordPlayerType.eHuman1:

                ExecuteEvents.Execute<IActivatePlayerRecordImage>(
                target: PlayerRecordObj[Rank],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.SetActive(RecordHumanIcon,GameManager.KillCountByHuman1));

                break;

            case ERecordPlayerType.eHuman2:

                ExecuteEvents.Execute<IActivatePlayerRecordImage>(
                target: PlayerRecordObj[Rank],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.SetActive(RecordHumanIcon, GameManager.KillCountByHuman2));

                break;

            case ERecordPlayerType.eMouse1:

                ExecuteEvents.Execute<IActivatePlayerRecordImage>(
                target: PlayerRecordObj[Rank],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.SetActive(RecordMouseIcon, GameManager.EatCountByMouse1));
                break;

            case ERecordPlayerType.eMouse2:

                ExecuteEvents.Execute<IActivatePlayerRecordImage>(
                target: PlayerRecordObj[Rank],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.SetActive(RecordMouseIcon, GameManager.EatCountByMouse2));

                break;

        }
    }

}
