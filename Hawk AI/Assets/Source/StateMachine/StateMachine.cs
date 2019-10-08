using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStateMachine<Template>
{
    private CStateBase<Template> m_cCurrentState;

    public CStateMachine()
    {
        m_cCurrentState = null;
    }

    public void ChangeState(CStateBase<Template> _cState)
    {
        if(m_cCurrentState != null)
        {
            m_cCurrentState.Exit();
        }

        m_cCurrentState = _cState;
        m_cCurrentState.Enter();
    }

    public CStateBase<Template> GetCurrentState()
    {
        if(m_cCurrentState != null)
        {
            return m_cCurrentState;
        }
        return null;
    }

    public void EndState()
    {
        if(m_cCurrentState != null)
        {
            m_cCurrentState.Exit();
        }
        m_cCurrentState = null;
    }

    public void Update()
    {
        if(m_cCurrentState != null)
        {
            m_cCurrentState.Execute();
        }
    }

}
