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

            this.m_cOwner.ChangeState(0, EFadeState.FadeStay);
        }
    }

    public override void Exit()
    {

    }

    //public virtual IEnumerator FadeInCoroutine(Vector3 _StartAngle, Vector3 _EndAngle)
    //{
    //    float lerpVal = 0f;

    //    while (lerpVal <= 1f)
    //    {//開ける時間補間
    //        lerpVal += Time.deltaTime / 10f;
    //        m_cOwner.m_cImageRect.localPosition
    //            = Vector3.Lerp(_StartAngle, _EndAngle, lerpVal);

    //        Debug.Log("lerpVal : " + lerpVal);
    //        Debug.Log("m_cOwner.m_cImageRect.localPosition : " + m_cOwner.m_cImageRect.localPosition);

    //        yield return null;
    //    }
    //}
}
