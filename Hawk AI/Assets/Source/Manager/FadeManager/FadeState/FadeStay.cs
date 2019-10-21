using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeStay : CStateBase<FadeManager>
{
    public FadeStay(FadeManager _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : FadeStay");
    }
    public override void Execute()
    {
        if (Input.anyKeyDown)
        {
            this.m_cOwner.CallFadeIn();
            
        }
    }

    public override void Exit()
    {

    }
}
