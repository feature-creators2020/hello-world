using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;
using KeyBoardInput;

public class MGetCheeseManager : CStateBase<MouseStateManager>
{
    public MGetCheeseManager(MouseStateManager _cOwner) : base(_cOwner) { }

    float m_fEffectTime;    // チーズを取ったときにする演出の長さ
    bool m_isFadeOut;       // フェードアウトを一回だけ処理入れる
    bool m_isFadeIn;        // フェードインを一回だけ処理入れる
    float m_fMaxEffectTime; // 演出の長さ

    public override void Enter()
    {
        m_fMaxEffectTime = 5f;
        m_fEffectTime = m_fMaxEffectTime;
        m_isFadeOut = false;
        m_isFadeIn = false;
        m_cOwner.m_SEAudio.Play((int)SEAudioType.eSE_GetCheese);   // チーズゲットSE
        m_cOwner.PlayAnimation(EMouseAnimation.Eat);
    }

    public override void Execute()
    {
        //Debug.Log("MouseState : GetCheese");
        // チーズを取得したときに流す演出の後にリスポーンなどの処理を入れる
        if(m_fEffectTime <= 0f)
        {
            if (!m_isFadeOut)
            {
                ExecuteEvents.Execute<IFadeInterfase>(
                    target: this.m_cOwner.targetCamera.gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.CallFadeOut());
                m_isFadeOut = true;
            }
            if (this.m_cOwner.targetCamera.gameObject.GetComponent<FadeEffect>().IsCompleteFlg)
            {
                // リスポーン処理
                Respawn();
                m_isFadeIn = true;
            }
            if (m_isFadeIn)
            {
                ExecuteEvents.Execute<IFadeInterfase>(
                target: this.m_cOwner.targetCamera.gameObject,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.CallFadeIn());
                m_isFadeIn = false;
                m_cOwner.ChangeState(0, EMouseState.Normal);
            }
        }
        else
        {
            m_fEffectTime -= Time.deltaTime;
            var target = new Vector3(m_cOwner.m_GTargetBoxObject.transform.position.x, m_cOwner.transform.position.y, m_cOwner.m_GTargetBoxObject.transform.position.z);
            var distance = Vector3.Distance(target, m_cOwner.transform.position);
            if (distance > 0.01f)
            {
                m_cOwner.transform.rotation = Quaternion.LookRotation(m_cOwner.transform.forward, m_cOwner.transform.up);
            }
            m_cOwner.transform.position = Vector3.Lerp(m_cOwner.transform.position, target, (m_fMaxEffectTime - m_fEffectTime) / m_fMaxEffectTime);
        }

    }

    public override void Exit()
    {
        //ExecuteEvents.Execute<IFadeInterfase>(
        //target: this.m_cOwner.targetCamera.gameObject,
        //eventData: null,
        //functor: (recieveTarget, y) => recieveTarget.CallFadeIn());

        m_fEffectTime = m_fMaxEffectTime;
        m_isFadeOut = false;

    }


    void Respawn()
    {
        //ExecuteEvents.Execute<IFadeInterfase>(
        //        target: m_cOwner.targetCamera.gameObject,
        //        eventData: null,
        //        functor: (recieveTarget, y) => recieveTarget.CallFadeOut());

        ScoreBoard.Instance.GetCheese();
        RespawnPoint.Instance.Respawn(m_cOwner.gameObject);
        ShiftOtherGoal.Instance.Shift(m_cOwner.m_GTargetBoxObject);
    }
}