using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public interface IFadeInterfase : IEventSystemHandler
{
    void CallFadeIn();
    void CallFadeOut();
    void CallFadeStay();
}

public enum EFadeState
{
    FadeIn,
    FadeStay,
    FadeOut,
}

public class FadeManager : CStateObjectBase<FadeManager, EFadeState>,IFadeInterfase
{
    public float TimePerSpeed;

    public RectTransform m_cImageRect = null;

    // Start is called before the first frame update
    void Start()
    {
        var StateMachine = new CStateMachine<FadeManager>();
        m_cStateMachineList.Add(StateMachine);

        var fadeIn = new FadeIn(this);
        var fadeStay = new FadeStay(this);
        var fadeOut = new FadeOut(this);

        m_cStateList.Add(fadeIn);
        m_cStateList.Add(fadeStay);
        m_cStateList.Add(fadeOut);


        m_cImageRect = this.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();

        Debug.Log(m_cImageRect.name);

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EFadeState.FadeStay]);
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();
    }

    public virtual IEnumerator FadeCoroutine(Vector3 _StartAngle, Vector3 _EndAngle)
    {
        float lerpVal = 0f;

        while (lerpVal <= 1f)
        {//開ける時間補間
            lerpVal += Time.deltaTime / TimePerSpeed;
            m_cImageRect.localPosition
                = Vector3.Lerp(_StartAngle, _EndAngle, lerpVal);
            yield return null;
        }
    }

    public virtual void CallFadeIn()
    {
        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EFadeState.FadeIn]);
    }
    public virtual void CallFadeOut()
    {
        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EFadeState.FadeOut]);
    }

    public virtual void CallFadeStay()
    {
        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EFadeState.FadeStay]);
    }

}
