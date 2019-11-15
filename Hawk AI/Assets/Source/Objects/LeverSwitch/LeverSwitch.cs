﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;
using KeyBoardInput;

public enum ELeverSwitchState
{
    ActiveCorrect,
    ActiveInverse,
}


public interface ILeverSwitch : IEventSystemHandler
{
    void ChangeState(ELeverSwitchState _eLeverSwitchState);
    ELeverSwitchState GetState();

}


public class LeverSwitch : MonoBehaviour, ILeverSwitch
{
    private ELeverSwitchState m_eLeverSwitchState;
    private bool m_bActivation;
    [SerializeField]
    private string m_sTag;

    // Start is called before the first frame update
    void Start()
    {
        m_eLeverSwitchState = ELeverSwitchState.ActiveCorrect;
        m_bActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bActivation == true)
        {
            // コントローラー対応
            if (GamePad.GetButton(GamePad.Button.B, GamePad.Index.One) || KeyBoard.GetButton(KeyBoard.Button.B, KeyBoard.Index.Three))
            {
                if (GetState() == ELeverSwitchState.ActiveCorrect)
                {
                    // Correct To Inverse
                    var RailManager = ManagerObjectManager.Instance.GetGameObject("RailManager").
                        GetComponent<RailManager>().GetGameObjectsList();

                    foreach (var val in RailManager)
                    {
                        ExecuteEvents.Execute<IRailInterfase>(
                            target: val,
                            eventData: null,
                            functor: (recieveTarget, y) => recieveTarget.ChangeState(ERailState.Inverse));
                    }

                    ChangeState(ELeverSwitchState.ActiveInverse);
                }
                else
                {
                    //  Inverse To Correct
                    var RailManager = ManagerObjectManager.Instance.GetGameObject("RailManager").
                        GetComponent<RailManager>().GetGameObjectsList();

                    foreach (var val in RailManager)
                    {
                        ExecuteEvents.Execute<IRailInterfase>(
                            target: val,
                            eventData: null,
                            functor: (recieveTarget, y) => recieveTarget.ChangeState(ERailState.Correct));
                    }

                    ChangeState(ELeverSwitchState.ActiveCorrect);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == m_sTag)
            m_bActivation = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == m_sTag)
            m_bActivation = false;
    }

    public void ChangeState(ELeverSwitchState _eLeverSwitchState)
    {
        m_eLeverSwitchState = _eLeverSwitchState;
    }

    public ELeverSwitchState GetState()
    {
        return m_eLeverSwitchState;
    }

}
