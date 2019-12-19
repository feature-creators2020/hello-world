using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public interface IRecordScreenCanvas : IEventSystemHandler
{
    void ChangeRecordScreen(int id);
}


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

public enum EPlayerContorollerID
{
    eHuman1,
    eMouse1,
    eHuman2,
    eMouse2,
}

public class RecordScreenCanvas : MonoBehaviour, IResultManagerInterfase,IRecordScreenCanvas
{

    [SerializeField]
    private List<Sprite> HumanIcon = new List<Sprite>();

    [SerializeField]
    private List<Sprite> MouseIcon = new List<Sprite>();

    [SerializeField]
    private List<Sprite> PlayerNoSprite = new List<Sprite>();

    [SerializeField]
    private List<Sprite> RecordScreen = new List<Sprite>();

    [SerializeField]
    private Sprite RecordMouseIcon;

    [SerializeField]
    private Sprite RecordHumanIcon;

    [SerializeField]
    private Sprite RecordMask;

    [SerializeField]
    private List<Material> RecordMaskMaterial = new List<Material>();

    [SerializeField]
    private float RecordMaskFadeSpeed;

    [SerializeField]
    private List<GameObject> PlayerRecordObj = new List<GameObject>();

    [SerializeField]
    private GameObject m_cResultManager;

    private const int PlayerScreenNo = 0;
    private const int PlayerIconNo = 4;
    private const int PlayerScreenShotNo = 5;

    private const int First = 0;
    private const int Seconed = 1;
    private const int Third = 2;
    private const int Forth = 3;

    private bool[] m_bScreenOut = new bool[4];
    private bool[] m_bScreenOutComplete = new bool[4];

    //private ERecordScreenChild FirstObj;
    //private ERecordScreenChild SeconedObj;
    //private ERecordScreenChild ThirdObj;
    //private ERecordScreenChild ForthObj;

    private Vector2[] m_cMaskTexUV = new Vector2[4];

    public void HawkAIWin()
    {
        SortingHumanWin();
    }


    public void MouseWin()
    {
        SortingMouseWin();
    }


    void Update()
    {
        for (int i = 0; i < m_bScreenOut.Length; i++)
        {
            if (m_bScreenOut[i] == true)
            {
                var val =
                this.gameObject.transform.GetChild(i + 1).GetComponent<Image>().material;

                if (m_cMaskTexUV[i].x <= 1f)
                {
                    m_cMaskTexUV[i] = new Vector2(m_cMaskTexUV[i].x + RecordMaskFadeSpeed, 1);
                }
                else
                {
                    if (m_bScreenOutComplete[i] == false)
                    {
                        ExecuteEvents.Execute<IResultManagerInterfase>(
                        target: m_cResultManager,
                        eventData: null,
                        functor: (recieveTarget, y) => recieveTarget.SetIsOk(i, true));

                        m_bScreenOutComplete[i] = true;
                    }
                }


                if (val != null)
                {
                    val.SetVector("_MaskTexUV", m_cMaskTexUV[i]);
                    val.SetTexture("_MaskTexture", RecordMask.texture);
                }
            }

        }
    }


    private void SortingHumanWin()
    {
        Image Player1Image = PlayerRecordObj[PlayerScreenShotNo].transform.GetChild(0).gameObject.GetComponent<Image>();
        Image Player2Image = PlayerRecordObj[PlayerScreenShotNo].transform.GetChild(1).gameObject.GetComponent<Image>();
        Image Player3Image = PlayerRecordObj[PlayerScreenShotNo].transform.GetChild(2).gameObject.GetComponent<Image>();
        Image Player4Image = PlayerRecordObj[PlayerScreenShotNo].transform.GetChild(3).gameObject.GetComponent<Image>();

        Image PlayerNo1Image = PlayerRecordObj[PlayerIconNo].transform.GetChild(0).gameObject.GetComponent<Image>();
        Image PlayerNo2Image = PlayerRecordObj[PlayerIconNo].transform.GetChild(1).gameObject.GetComponent<Image>();
        Image PlayerNo3Image = PlayerRecordObj[PlayerIconNo].transform.GetChild(2).gameObject.GetComponent<Image>();
        Image PlayerNo4Image = PlayerRecordObj[PlayerIconNo].transform.GetChild(3).gameObject.GetComponent<Image>();

        Image PlayerScreen1Image = this.gameObject.transform.
            GetChild((int)ERecordScreenChild.ePlayer1).gameObject.GetComponent<Image>();
        Image PlayerScreen2Image = this.gameObject.transform.
            GetChild((int)ERecordScreenChild.ePlayer2).gameObject.GetComponent<Image>();
        Image PlayerScreen3Image = this.gameObject.transform.
            GetChild((int)ERecordScreenChild.ePlayer3).gameObject.GetComponent<Image>();
        Image PlayerScreen4Image = this.gameObject.transform.
            GetChild((int)ERecordScreenChild.ePlayer4).gameObject.GetComponent<Image>();


        // Decide First n Seconed.
        if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
        {
            Player1Image.sprite = HumanIcon[0];
            PlayerNo1Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eHuman1];
            PlayerScreen1Image.sprite = RecordScreen[(int)EPlayerContorollerID.eHuman1];

            RecordPopUp(First, ERecordPlayerType.eHuman1);
   //         FirstObj = ERecordScreenChild.ePlayer1;

            Player2Image.sprite = HumanIcon[1];
            PlayerNo2Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eHuman2];
            PlayerScreen2Image.sprite = RecordScreen[(int)EPlayerContorollerID.eHuman2];

            RecordPopUp(Seconed, ERecordPlayerType.eHuman2);
    //        SeconedObj = ERecordScreenChild.ePlayer3;

        }
        else
        {
            Player1Image.sprite = HumanIcon[1];
            PlayerNo1Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eHuman2];
            PlayerScreen1Image.sprite = RecordScreen[(int)EPlayerContorollerID.eHuman2];

            RecordPopUp(First, ERecordPlayerType.eHuman2);
      //      FirstObj = ERecordScreenChild.ePlayer3;

            Player2Image.sprite = HumanIcon[0];
            PlayerNo2Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eHuman1];
            PlayerScreen2Image.sprite = RecordScreen[(int)EPlayerContorollerID.eHuman1];

            RecordPopUp(Seconed, ERecordPlayerType.eHuman1);
     //       SeconedObj = ERecordScreenChild.ePlayer1;
        }

        // Decide Third n Forth.
        if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
        {
            Player3Image.sprite = MouseIcon[0];
            PlayerNo3Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eMouse1];
            PlayerScreen3Image.sprite = RecordScreen[(int)EPlayerContorollerID.eMouse1];

            RecordPopUp(Third, ERecordPlayerType.eMouse1);
   //         ThirdObj = ERecordScreenChild.ePlayer2;

            Player4Image.sprite = MouseIcon[1];
            PlayerNo4Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eMouse2];
            PlayerScreen4Image.sprite = RecordScreen[(int)EPlayerContorollerID.eMouse2];

            RecordPopUp(Forth, ERecordPlayerType.eMouse2);
    //        ForthObj = ERecordScreenChild.ePlayer4;

        }
        else
        {
            Player3Image.sprite = MouseIcon[1];
            PlayerNo3Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eMouse2];
            PlayerScreen3Image.sprite = RecordScreen[(int)EPlayerContorollerID.eMouse2];

            RecordPopUp(Third, ERecordPlayerType.eMouse2);
     //       ThirdObj = ERecordScreenChild.ePlayer4;

            Player4Image.sprite = MouseIcon[0];
            PlayerNo4Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eMouse1];
            PlayerScreen4Image.sprite = RecordScreen[(int)EPlayerContorollerID.eMouse1];

            RecordPopUp(Forth, ERecordPlayerType.eMouse1);
     //       ForthObj = ERecordScreenChild.ePlayer2;

        }
    }

    private void SortingMouseWin()
    {
        Image Player1Image = PlayerRecordObj[PlayerScreenShotNo].transform.GetChild(0).gameObject.GetComponent<Image>();
        Image Player2Image = PlayerRecordObj[PlayerScreenShotNo].transform.GetChild(1).gameObject.GetComponent<Image>();
        Image Player3Image = PlayerRecordObj[PlayerScreenShotNo].transform.GetChild(2).gameObject.GetComponent<Image>();
        Image Player4Image = PlayerRecordObj[PlayerScreenShotNo].transform.GetChild(3).gameObject.GetComponent<Image>();

        Image PlayerNo1Image = PlayerRecordObj[PlayerIconNo].transform.GetChild(0).gameObject.GetComponent<Image>();
        Image PlayerNo2Image = PlayerRecordObj[PlayerIconNo].transform.GetChild(1).gameObject.GetComponent<Image>();
        Image PlayerNo3Image = PlayerRecordObj[PlayerIconNo].transform.GetChild(2).gameObject.GetComponent<Image>();
        Image PlayerNo4Image = PlayerRecordObj[PlayerIconNo].transform.GetChild(3).gameObject.GetComponent<Image>();

        Image PlayerScreen1Image = this.gameObject.transform.
            GetChild((int)ERecordScreenChild.ePlayer1).gameObject.GetComponent<Image>();
        Image PlayerScreen2Image = this.gameObject.transform.
            GetChild((int)ERecordScreenChild.ePlayer2).gameObject.GetComponent<Image>();
        Image PlayerScreen3Image = this.gameObject.transform.
            GetChild((int)ERecordScreenChild.ePlayer3).gameObject.GetComponent<Image>();
        Image PlayerScreen4Image = this.gameObject.transform.
            GetChild((int)ERecordScreenChild.ePlayer4).gameObject.GetComponent<Image>();

        // Decide First n Seconed.
        if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
        {
            Player1Image.sprite = MouseIcon[0];
            PlayerNo1Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eMouse1];
            PlayerScreen1Image.sprite = RecordScreen[(int)EPlayerContorollerID.eMouse1];

            RecordPopUp(First, ERecordPlayerType.eMouse1);
    //        FirstObj = ERecordScreenChild.ePlayer2;

            Player2Image.sprite = MouseIcon[1];
            PlayerNo2Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eMouse2];
            PlayerScreen2Image.sprite = RecordScreen[(int)EPlayerContorollerID.eMouse2];

            RecordPopUp(Seconed, ERecordPlayerType.eMouse2);
       //     SeconedObj = ERecordScreenChild.ePlayer4;

        }
        else
        {
            Player1Image.sprite = MouseIcon[1];
            PlayerNo1Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eMouse2];
            PlayerScreen1Image.sprite = RecordScreen[(int)EPlayerContorollerID.eMouse2];

            RecordPopUp(First, ERecordPlayerType.eMouse2);
      //      FirstObj = ERecordScreenChild.ePlayer4;

            Player2Image.sprite = MouseIcon[0];
            PlayerNo2Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eMouse1];
            PlayerScreen2Image.sprite = RecordScreen[(int)EPlayerContorollerID.eMouse1];

            RecordPopUp(Seconed, ERecordPlayerType.eMouse1);
      //      SeconedObj = ERecordScreenChild.ePlayer2;
        }

        // Decide Third n Forth.
        if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
        {
            Player3Image.sprite = HumanIcon[0];
            PlayerNo3Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eHuman1];
            PlayerScreen3Image.sprite = RecordScreen[(int)EPlayerContorollerID.eHuman1];

            RecordPopUp(Third, ERecordPlayerType.eHuman1);
        //    ThirdObj = ERecordScreenChild.ePlayer1;

            Player4Image.sprite = HumanIcon[1];
            PlayerNo4Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eHuman2];
            PlayerScreen4Image.sprite = RecordScreen[(int)EPlayerContorollerID.eHuman2];

            RecordPopUp(Forth, ERecordPlayerType.eHuman2);
          //  ForthObj = ERecordScreenChild.ePlayer3;
        }
        else
        {
            Player3Image.sprite = HumanIcon[1];
            PlayerNo3Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eHuman2];
            PlayerScreen3Image.sprite = RecordScreen[(int)EPlayerContorollerID.eHuman2];

            RecordPopUp(Third, ERecordPlayerType.eHuman2);
        //    ThirdObj = ERecordScreenChild.ePlayer3;

            Player4Image.sprite = HumanIcon[0];
            PlayerNo4Image.sprite = PlayerNoSprite[(int)EPlayerContorollerID.eHuman1];
            PlayerScreen4Image.sprite = RecordScreen[(int)EPlayerContorollerID.eHuman1];

            RecordPopUp(Forth, ERecordPlayerType.eHuman1);
         //   ForthObj = ERecordScreenChild.ePlayer1;

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
                functor: (recieveTarget, y) => recieveTarget.SetActive(RecordHumanIcon,GameManager.KillCountByHuman1,true));

                break;

            case ERecordPlayerType.eHuman2:

                ExecuteEvents.Execute<IActivatePlayerRecordImage>(
                target: PlayerRecordObj[Rank],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.SetActive(RecordHumanIcon, GameManager.KillCountByHuman2, true));

                break;

            case ERecordPlayerType.eMouse1:

                ExecuteEvents.Execute<IActivatePlayerRecordImage>(
                target: PlayerRecordObj[Rank],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.SetActive(RecordMouseIcon, GameManager.EatCountByMouse1, true));
                break;

            case ERecordPlayerType.eMouse2:

                ExecuteEvents.Execute<IActivatePlayerRecordImage>(
                target: PlayerRecordObj[Rank],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.SetActive(RecordMouseIcon, GameManager.EatCountByMouse2, true));

                break;

        }
    }

    public void ChangeRecordScreen(int id)
    {
        int childnum = 0;
        EPlayerContorollerID PlayerNum = (EPlayerContorollerID)id;


        SetJudgeRank(PlayerNum, ref childnum);

        

        // Icno N records
        m_bScreenOut[childnum - 1] = true;

        Image PlayerImage = this.gameObject.transform.GetChild(childnum).
            gameObject.GetComponent<Image>();

        PlayerImage.material = RecordMaskMaterial[childnum - 1];

        //set uv value
        m_cMaskTexUV[childnum - 1] = new Vector4(0, 1, 1, 1);

        ExecuteEvents.Execute<IActivatePlayerRecordImage>(
        target: PlayerRecordObj[childnum - 1],
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.SetActive(RecordMouseIcon, childnum - 1, false));

        ExecuteEvents.Execute<IActivatePlayerRecordImage>(
        target: PlayerRecordObj[PlayerIconNo],
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.SetActive(RecordMouseIcon, childnum - 1, false));

    }

    private void SetJudgeRank(EPlayerContorollerID _PlayerNum, ref int chilnum)
    {
        int First = 1;
        int Seconed = 2;
        int Third = 3;
        int Forth = 4;

        // プレイヤーから順位の判定
        switch (_PlayerNum)
        {
            case EPlayerContorollerID.eHuman1:
                if (GameManager.IsHumanWin == true)
                {// Decide First n Seconed.
                    if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
                    {
                        chilnum = First;
                    }
                    else
                    {
                        chilnum = Seconed;
                    }

                }
                else
                {//GameManager.IsHumanWin == false

                    // Decide Third n Forth.
                    if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
                    {
                        chilnum = Third;
                    }
                    else
                    {
                        chilnum = Forth;
                    }
                }

                break;

            case EPlayerContorollerID.eHuman2:

                if (GameManager.IsHumanWin == true)
                {// Decide First n Seconed.
                    if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
                    {
                        chilnum = Seconed;
                    }
                    else
                    {
                        chilnum = First;
                    }

                }
                else
                {//GameManager.IsHumanWin == false

                    // Decide Third n Forth.
                    if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
                    {
                        chilnum = Forth;
                    }
                    else
                    {
                        chilnum = Third;
                    }
                }

                break;

            case EPlayerContorollerID.eMouse1:

                if (GameManager.IsHumanWin == true)
                {// Decide First n Seconed.
                    if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
                    {
                        chilnum = Third;
                    }
                    else
                    {
                        chilnum = Forth;
                    }

                }
                else
                {//GameManager.IsHumanWin == false

                    // Decide Third n Forth.
                    if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
                    {
                        chilnum = First;
                    }
                    else
                    {
                        chilnum = Seconed;
                    }
                }

                break;

            case EPlayerContorollerID.eMouse2:

                if (GameManager.IsHumanWin == true)
                {  // Decide Third n Forth.

                    if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
                    {
                        chilnum = Forth;
                    }
                    else
                    {
                        chilnum = Third;
                    }

                }
                else
                {//GameManager.IsHumanWin == false
                    // Decide First n Seconed.                  

                    if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
                    {
                        chilnum = Seconed;
                    }
                    else
                    {
                        chilnum = First;
                    }
                }

                break;

        }

    }

    public void SetIsOk(int id, bool isVal)
    {

    }


}
