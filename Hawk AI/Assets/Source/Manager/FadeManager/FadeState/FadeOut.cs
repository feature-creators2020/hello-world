using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : CStateBase<FadeManager>
{
    public FadeOut(FadeManager _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : FadeOut");

        m_cOwner.StartCoroutine(m_cOwner.FadeCoroutine(new Vector3(0f, m_cOwner.m_cImageRect.rect.height, 0f), Vector3.zero));

    }
    public override void Execute()
    {
        //if (Input.anyKeyDown)
        //{
        //    this.m_cOwner.CallFadeStay();
        //}
    }

    public override void Exit()
    {

    }

}
