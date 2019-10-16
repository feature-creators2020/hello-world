using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStateBase<Template>
{
    protected Template m_cOwner;

    public CStateBase(Template _cOwner)
    {
        m_cOwner = _cOwner;
    }

    public virtual void Enter() { }

    public virtual void Execute() { }

    public virtual void Exit() { }

}
