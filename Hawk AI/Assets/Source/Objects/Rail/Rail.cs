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

    ERailState m_eRailState;

    // Start is called before the first frame update
    void Start()
    {
        m_eRailState = ERailState.Correct;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ChangeState(ERailState _eRailState)
    {
        m_eRailState = _eRailState;
        Debug.Log("m_eRailState : " + m_eRailState);

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
