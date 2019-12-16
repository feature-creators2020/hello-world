using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public enum ERecordScreenChild
{
    eBackScreen,
    ePlayer1,
    ePlayer2,
    ePlayer3,
    ePlayer4,
}

public class RecordScreenCanvas : MonoBehaviour, IResultManagerInterfase
{

    [SerializeField]
    private List<Sprite> HumanIcon = new List<Sprite>();

    [SerializeField]
    private List<Sprite> MouseIcon = new List<Sprite>();

    private int First = 0;
    private int Seconed = 0;
    private int Third = 0;
    private int Forth = 0;


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
        Image Player1Image = this.gameObject.transform.GetChild((int)ERecordScreenChild.ePlayer1).gameObject.GetComponent<Image>();
        Image Player2Image = this.gameObject.transform.GetChild((int)ERecordScreenChild.ePlayer2).gameObject.GetComponent<Image>();
        Image Player3Image = this.gameObject.transform.GetChild((int)ERecordScreenChild.ePlayer3).gameObject.GetComponent<Image>();
        Image Player4Image = this.gameObject.transform.GetChild((int)ERecordScreenChild.ePlayer4).gameObject.GetComponent<Image>();


        // Decide First n Seconed.
        if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
        {
            First = GameManager.KillCountByHuman1;
            Player1Image.sprite = HumanIcon[0];

            Seconed = GameManager.KillCountByHuman2;
            Player2Image.sprite = HumanIcon[1];

        }
        else
        {
            First = GameManager.KillCountByHuman2;
            Player1Image.sprite = HumanIcon[1];

            Seconed = GameManager.KillCountByHuman1;
            Player2Image.sprite = HumanIcon[0];
        }

        // Decide Third n Forth.
        if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
        {
            Third = GameManager.EatCountByMouse1;
            Player3Image.sprite = MouseIcon[0];

            Forth = GameManager.EatCountByMouse2;
            Player4Image.sprite = MouseIcon[1];

        }
        else
        {
            Third = GameManager.EatCountByMouse2;
            Player3Image.sprite = MouseIcon[1];

            Forth = GameManager.EatCountByMouse1;
            Player4Image.sprite = MouseIcon[0];
        }
    }

    private void SortingMouseWin()
    {
        Image Player1Image = this.gameObject.transform.GetChild((int)ERecordScreenChild.ePlayer1).gameObject.GetComponent<Image>();
        Image Player2Image = this.gameObject.transform.GetChild((int)ERecordScreenChild.ePlayer2).gameObject.GetComponent<Image>();
        Image Player3Image = this.gameObject.transform.GetChild((int)ERecordScreenChild.ePlayer3).gameObject.GetComponent<Image>();
        Image Player4Image = this.gameObject.transform.GetChild((int)ERecordScreenChild.ePlayer4).gameObject.GetComponent<Image>();


        // Decide First n Seconed.
        if (GameManager.EatCountByMouse1 >= GameManager.EatCountByMouse2)
        {
            First = GameManager.EatCountByMouse1;
            Player1Image.sprite = MouseIcon[0];

            Seconed = GameManager.EatCountByMouse2;
            Player2Image.sprite = MouseIcon[1];

        }
        else
        {
            First = GameManager.EatCountByMouse2;
            Player1Image.sprite = MouseIcon[1];

            Seconed = GameManager.EatCountByMouse1;
            Player2Image.sprite = MouseIcon[0];
        }

        // Decide Third n Forth.
        if (GameManager.KillCountByHuman1 >= GameManager.KillCountByHuman2)
        {
            Third = GameManager.KillCountByHuman1;
            Player3Image.sprite = HumanIcon[0];

            Forth = GameManager.KillCountByHuman2;
            Player4Image.sprite = HumanIcon[1];

        }
        else
        {
            Third = GameManager.KillCountByHuman2;
            Player3Image.sprite = HumanIcon[1];

            Forth = GameManager.KillCountByHuman1;
            Player4Image.sprite = HumanIcon[0];
        }
    }

}
