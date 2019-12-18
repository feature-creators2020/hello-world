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



    private GamePad.Index GamePadIndex;          // 対象のコントローラー
    private KeyBoard.Index KeyboardIndex;        // 対象のキーボード

    DoorAreaCollision m_sDoorAreaCollision;      // ドアエリアスクリプト


    // Start is called before the first frame update
    public virtual void Start()
    {
        isOpening = false;
        isClosing = false;
        StartAngle = this.gameObject.transform.parent.rotation.eulerAngles;

        var StateMachine = new CStateMachine<Door>();
        m_cStateMachineList.Add(StateMachine);

        var DoorOpen = new Door_Open(this);
        var DoorClose = new Door_Close(this);

        m_cStateList.Add(DoorOpen);
        m_cStateList.Add(DoorClose);

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EDoorState.eClose]);

        m_sDoorAreaCollision = this.transform.parent.GetChild(0).gameObject.GetComponent<DoorAreaCollision>();

    }

    public virtual void Update()
    {
        //if(Input.GetKeyDown(KeyCode.I))
        //{
        //    OpenOrClose();
        //}

        // 対象の人が入っていないとき処理をしない
        if (!ReferenceEquals(m_sDoorAreaCollision.GetTargetHuman(), null))
        {
            var human = m_sDoorAreaCollision.GetTargetHuman().GetComponent<HumanStateManager>();

            var playerNo = human.GamePadIndex;
            var keyState = GamePad.GetState(playerNo, false);
            var playerKeyNo = (KeyBoard.Index)playerNo;
            var keyboardState = KeyBoard.GetState(human.KeyboardIndex, false);

            // 開閉させる処理
            if (GamePad.GetButtonDown(GamePad.Button.A, playerNo) || KeyBoard.GetButtonDown(KeyBoard.Button.A, playerKeyNo))
            {
                OpenOrClose();
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
            ManagerObjectManager.Instance.GetGameObject("SEAudio").GetComponent<SEAudio>().MultiplePlay((int)SEAudioType.eSE_DoorOpen);
        }
    }

    //閉める
    public virtual void Close()
    {
        if (isClosing == false)
        {
            m_cStateMachineList[0].ChangeState(m_cStateList[(int)EDoorState.eClose]);
            ManagerObjectManager.Instance.GetGameObject("SEAudio").GetComponent<SEAudio>().MultiplePlay((int)SEAudioType.eSE_DoorClose);
        }
    }

}
