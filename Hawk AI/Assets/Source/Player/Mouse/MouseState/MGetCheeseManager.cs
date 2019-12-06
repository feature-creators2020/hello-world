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

    public override void Enter()
    {
        m_fEffectTime = 5f;
        m_isFadeOut = false;
        m_cOwner.m_SEAudio.Play((int)SEAudioType.eSE_GetCheese);   // チーズゲットSE
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
                m_cOwner.ChangeState(0, EMouseState.Normal);

            }
        }
        else
        {
            m_fEffectTime -= Time.deltaTime;
        }


    }

    public override void Exit()
    {
        ExecuteEvents.Execute<IFadeInterfase>(
        target: this.m_cOwner.targetCamera.gameObject,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeIn());

        m_fEffectTime = 5f;
        m_isFadeOut = false;

    }


    void Respawn()
    {
        ExecuteEvents.Execute<IFadeInterfase>(
                target: m_cOwner.targetCamera.gameObject,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.CallFadeOut());

        ScoreBoard.Instance.GetCheese();
        RespawnPoint.Instance.Respawn(m_cOwner.gameObject);
        ShiftOtherGoal.Instance.Shift(m_cOwner.m_GTargetBoxObject);
    }
}