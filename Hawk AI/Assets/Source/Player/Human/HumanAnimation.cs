using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimation : MonoBehaviour
{

    HumanStateManager m_sHumanStateManager;
    Animation m_sAnimation;

    // Start is called before the first frame update
    void Start()
    {
        m_sHumanStateManager = this.transform.parent.parent.gameObject.GetComponent<HumanStateManager>();
        m_sAnimation = this.gameObject.GetComponent<Animation>();


        var animName = m_sHumanStateManager.AnimationString;

        //イベントの追加
        AnimationEvent ev = new AnimationEvent();
        ev.time = 0.0f;
        ev.functionName = "OnFootEvent";
        ev.floatParameter = 1.0f;
        m_sAnimation[animName[(int)EHumanAnimation.Run]].clip.AddEvent(ev);
        //イベントをアニメーションクリップに追加するとそのイベントが起動した際SendMessageとしてfunctionNameの関数が起動される
        //この場合タイムラインの０フレーム目でSendMessage("StepSound",1.0f); が起動する 

        AnimationEvent ev2 = new AnimationEvent();
        ev2.time = m_sAnimation[animName[(int)EHumanAnimation.Run]].clip.length / 2;
        ev2.functionName = "OnFootEvent";
        ev2.floatParameter = 1.0f;
        m_sAnimation[animName[(int)EHumanAnimation.Run]].clip.AddEvent(ev2);
        //この場合runクリップのタイムライン半分の位置でSendMessage("StepSound",1.0f); が起動する 
        //考え方的にはアニメーションクリップにイベント関数を紐付する感覚に近い

        AnimationEvent ev3 = new AnimationEvent();
        ev3.time = m_sAnimation[animName[(int)EHumanAnimation.Catch]].clip.length / 2;
        ev3.functionName = "OnCatchEvent";
        ev3.floatParameter = 1.0f;
        m_sAnimation[animName[(int)EHumanAnimation.Catch]].clip.AddEvent(ev3);

        AnimationEvent ev4 = new AnimationEvent();
        ev4.time = m_sAnimation[animName[(int)EHumanAnimation.Catch]].clip.length;
        ev4.functionName = "OnEndCatchEvent";
        ev4.floatParameter = 1.0f;
        m_sAnimation[animName[(int)EHumanAnimation.Catch]].clip.AddEvent(ev4);

        AnimationEvent ev5 = new AnimationEvent();
        ev5.time = m_sAnimation[animName[(int)EHumanAnimation.Put]].clip.length / 3;
        ev5.functionName = "PutingEvent";
        ev5.floatParameter = 1.0f;
        m_sAnimation[animName[(int)EHumanAnimation.Put]].clip.AddEvent(ev5);

        AnimationEvent ev6 = new AnimationEvent();
        ev6.time = m_sAnimation[animName[(int)EHumanAnimation.Put]].clip.length;
        ev6.functionName = "OnEndPutEvent";
        ev6.floatParameter = 1.0f;
        m_sAnimation[animName[(int)EHumanAnimation.Put]].clip.AddEvent(ev6);

        AnimationEvent ev7 = new AnimationEvent();
        ev7.time = m_sAnimation[animName[(int)EHumanAnimation.VarsanDown_Start]].clip.length;
        ev7.functionName = "OnEndVarsanStartEvent";
        ev7.floatParameter = 1.0f;
        m_sAnimation[animName[(int)EHumanAnimation.VarsanDown_Start]].clip.AddEvent(ev7);

        AnimationEvent ev8 = new AnimationEvent();
        ev8.time = m_sAnimation[animName[(int)EHumanAnimation.VarsanDown_End]].clip.length;
        ev8.functionName = "OnEndVarsanEndEvent";
        ev8.floatParameter = 1.0f;
        m_sAnimation[animName[(int)EHumanAnimation.VarsanDown_End]].clip.AddEvent(ev8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFootEvent()
    {
        m_sHumanStateManager.OnFootEvent();
    }

    public void OnCatchEvent()
    {
        m_sHumanStateManager.OnCatchEvent();
    }

    public void OnEndCatchEvent()
    {
        m_sHumanStateManager.OnEndCatchEvent();
    }

    public void PutingEvent()
    {
        m_sHumanStateManager.PutingEvent();
    }

    public void OnEndPutEvent()
    {
        m_sHumanStateManager.OnEndPutEvent();
    }

    public void OnEndVarsanStartEvent()
    {
        m_sHumanStateManager.OnEndVarsanStartEvent();
    }

    public void OnEndVarsanEndEvent()
    {
        m_sHumanStateManager.OnEndVarsanEndEvent();
    }
}
