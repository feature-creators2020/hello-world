using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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
}
