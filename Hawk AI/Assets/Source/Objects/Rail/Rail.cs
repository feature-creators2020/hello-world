using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ベルトコンベアー
/// </summary>
/// 

public interface IRailInterfase : IEventSystemHandler
{
    Vector3 GetMove();

    void ChangeState(ERailState _eRailState);

    void ChangeState();
}

public enum ERailState
{
    Stop,
    Correct,
    Inverse,
}

public class Rail : MonoBehaviour,IRailInterfase
{
    [SerializeField]
    float m_fSpeed;

    [SerializeField]
    bool m_isblend;     // 斜めのベルトか

    [SerializeField]
    bool m_bReverse;    // 逆向きか

    [SerializeField]
    ERailState m_eRailState;

    [System.NonSerialized]
    public TimeZoneManager m_TimeZoneManager;           // 昼夜の状態を取得する

    // Start is called before the first frame update
    void Start()
    {
        m_eRailState = ERailState.Correct;
    }

    // Update is called once per frame
    void Update()
    {
        var managerobject = ManagerObjectManager.Instance;
        m_TimeZoneManager = managerobject.GetGameObject("TimeZoneManager").GetComponent<TimeZoneManager>();

        // 昼夜状態取得
        if (m_TimeZoneManager.TimeZoneStatus == ETimeZone.eEvenning) // タイムマネージャーから昼夜の状態を取得し、判定する
        {
            // 夜状態に切り替える
            ChangeState(ERailState.Stop);
        }

    }

    public Vector3 GetMove()
    {
        if (m_isblend)
        {
            switch (m_eRailState)
            {
                case ERailState.Correct:

                    return (-this.transform.forward + this.transform.right) * m_fSpeed;

                case ERailState.Inverse:

                    return -(-this.transform.forward + this.transform.right) * m_fSpeed;

                case ERailState.Stop:
                default:

                    return Vector3.zero;

            }
        }
        else
        {
            switch (m_eRailState)
            {
                case ERailState.Correct:

                    return this.transform.right * m_fSpeed;

                case ERailState.Inverse:

                    return -this.transform.right * m_fSpeed;

                case ERailState.Stop:
                default:

                    return Vector3.zero;

            }
        }
    }

    public void ChangeState()
    {

        switch (m_eRailState)
        {
            case ERailState.Correct:

                m_eRailState = ERailState.Inverse;
                break;

            case ERailState.Inverse:

                m_eRailState = ERailState.Correct;
                break;

            case ERailState.Stop:
            default:
                Debug.Log("ERailState.Stop : " + ERailState.Stop);
                break;
        }


    }


    public void ChangeState(ERailState _eRailState)
    {
        m_eRailState = _eRailState;
        //Debug.Log("m_eRailState : " + m_eRailState);

    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Human")
    //    {
    //        ExecuteEvents.Execute<IHumanInterface>(
    //               target: other.gameObject,
    //               eventData: null,
    //               functor: (recieveTarget, y) => recieveTarget.ChangeUpState(this.gameObject));
    //    }
    //    if(other.tag == "Mouse")
    //    {
    //        ExecuteEvents.Execute<IMouseInterface>(
    //               target: other.gameObject,
    //               eventData: null,
    //               functor: (recieveTarget, y) => recieveTarget.ChangeUpState(this.gameObject));
    //    }
    //}
}
