using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CStateObjectBase<Template,TEnum> 
    : MonoBehaviour where Template : class where TEnum : System.IConvertible
{
    protected List<CStateBase<Template>> m_cStateList = new List<CStateBase<Template>>();
    protected List<CStateMachine<Template>> m_cStateMachineList = new List<CStateMachine<Template>>();

    public virtual void ChangeState(int index,TEnum state)
    {
        if(m_cStateMachineList.Count <= index)
        {
            return;
        }
        m_cStateMachineList[index].ChangeState(m_cStateList[state.ToInt32(null)]);
    }

    public virtual void EndState(int index)
    {
        if(m_cStateMachineList.Count <= index)
        {
            return;
        }
        m_cStateMachineList[index].EndState();
    }

    public virtual bool IsCurrentState(int index,TEnum state)
    {
        if(m_cStateMachineList.Count <= index)
        {
            return false;
        }
        return m_cStateMachineList[index].GetCurrentState() == m_cStateList[state.ToInt32(null)];
    }

    public virtual void Update()
    {
        for (int i = 0; i < m_cStateMachineList.Count; i++)
        {
            m_cStateMachineList[i].Update();
        }
    }

    //public virtual void StateUpdate()
    //{
    //    for (int i = 0; i < m_cStateMachineList.Count; i++)
    //    {
    //        m_cStateMachineList[i].Update();
    //    }
    //}
}
