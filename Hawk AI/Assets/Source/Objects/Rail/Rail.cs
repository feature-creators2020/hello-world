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
        switch(m_eRailState)
        {
            case ERailState.Correct:

                return Vector3.forward + new Vector3(0f,0f, m_fSpeed);

            case ERailState.Inverse:

                return -Vector3.forward + new Vector3(0f, 0f, -m_fSpeed);

            case ERailState.Stop:
            default:

                return Vector3.zero;

        }
    }

    public void ChangeState(ERailState _eRailState)
    {
        m_eRailState = _eRailState;
        Debug.Log("m_eRailState : " + m_eRailState);

    }

}
