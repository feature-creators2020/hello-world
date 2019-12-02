using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;
using KeyBoardInput;



public enum EDoorState
{
    eOpen,
    eClose,
}

public interface IDoorInterface : IEventSystemHandler
{
    void OpenOrClose();
}

/// <summary>
/// Doorの開閉スクリプト
/// </summary>

public class Door : CStateObjectBase<Door, EDoorState>, IDoorInterface
{
    [SerializeField]
    public float OpenSpeed;                //扉の開閉速度
    [SerializeField]
    public float MaxOpenRadian;             //扉の開閉最大角度
    public Vector3 StartAngle;
    public bool isOpening {get;set; }        //扉の開フラグ
    public bool isClosing { get; set; }       //扉の閉フラグ


    private List<GameObject> HumanObject_List = new List<GameObject>();

    private GamePad.Index GamePadIndex;          // 対象のコントローラー
    private KeyBoard.Index KeyboardIndex;        // 対象のキーボード


    // Start is called before the first frame update
    public virtual void Start()
    {
        isOpening = false;
        isClosing = false;
        StartAngle = this.gameObject.transform.rotation.eulerAngles;

        var StateMachine = new CStateMachine<Door>();
        m_cStateMachineList.Add(StateMachine);

        var DoorOpen = new Door_Open(this);
        var DoorClose = new Door_Close(this);

        m_cStateList.Add(DoorOpen);
        m_cStateList.Add(DoorClose);

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EDoorState.eClose]);

    }

    public virtual void Update()
    {
        //if(Input.GetKeyDown(KeyCode.I))
        //{
        //    OpenOrClose();
        //}

        foreach(var val in HumanObject_List)
        {
            // 人が入っているか探す
            if(val.tag == "Human")
            {
                var human = val.GetComponent<HumanStateManager>();

                var playerNo = human.GamePadIndex;
                var keyState = GamePad.GetState(playerNo, false);
                var playerKeyNo = (KeyBoard.Index)playerNo;
                var keyboardState = KeyBoard.GetState(human.KeyboardIndex, false);

                // 開閉させる処理
                if (GamePad.GetButtonDown(GamePad.Button.B, playerNo) || KeyBoard.GetButtonDown(KeyBoard.Button.B, playerKeyNo))
                {
                    OpenOrClose();
                }
            }
        }
    }

    public virtual void OpenOrClose()
    {
        if (m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)EDoorState.eClose])
        {
            Open();
        }
        else if (m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)EDoorState.eOpen])
        {
            Close();
        }
    }

    //開ける
    public virtual void Open()
    {
        if (isOpening == false)
        {
            m_cStateMachineList[0].ChangeState(m_cStateList[(int)EDoorState.eOpen]);
        }
    }

    //閉める
    public virtual void Close()
    {
        if (isClosing == false)
        {
            m_cStateMachineList[0].ChangeState(m_cStateList[(int)EDoorState.eClose]);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("DoorEnter : " + other.gameObject.name);
        // 人の時処理をする
        if(other.tag == "Human")
        {
            foreach (var val in HumanObject_List)
            {
                if (val == other.gameObject)
                {
                    return;
                }
            }
            // 人の情報を入れる
            HumanObject_List.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("DoorExit : " + other.gameObject.name);
        // 人の時処理をする
        if (other.tag == "Human")
        {
            // 人の情報を消す
            //HumanObject_List.Remove(other.gameObject);

            foreach (var val in HumanObject_List)
            {
                if (val == other.gameObject)
                {
                    // 人の情報を消す
                    HumanObject_List.Remove(other.gameObject);
                    return;
                }
            }
        }
    }
}
