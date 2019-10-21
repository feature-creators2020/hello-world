using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FadeIn : CStateBase<FadeManager>
{
    public FadeIn(FadeManager _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : FadeIn");

        m_cOwner.StartCoroutine(m_cOwner.FadeCoroutine(Vector3.zero,new Vector3(0f, -m_cOwner.m_cImageRect.rect.height, 0f)));

    }
    public override void Execute()
    {
        if (Input.anyKeyDown)
        {
            //m_cOwner.m_cImageRect.localPosition
            //    += new Vector3(0, 100, 0);

            this.m_cOwner.ChangeState(0, EFadeState.FadeOut);
        }
    }

    public override void Exit()
    {
        m_cOwner.m_cImageRect.localPosition = new Vector3(0f, m_cOwner.m_cImageRect.rect.height, 0f);
    }
}
