using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarWait : CStateBase<Star>
{
    public StarWait(Star _cOwner) : base(_cOwner) { }
    
    public override void Enter()
    {
        //m_cOwner.ChangeStarAnimation(StarAnimation.Wait);
    }

    public override void Execute()
    {

        var speed = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))        // ジャンプ
        {
            m_cOwner.ChangeState(0, StarState.Jump);
            return;
        }
        if (speed != 0.0f)                      // 歩き
        {
            m_cOwner.ChangeState(0, StarState.Walk);
            return;
        }
        if (Input.GetButtonDown("PutCone") && !m_cOwner.StarOrdinaly && m_cOwner.IsPutCone())               // コーンを置く
        {
            m_cOwner.ChangeState(0, StarState.PutCone);
            return;
        }
        if (Input.GetButtonDown("PutCone") && !m_cOwner.StarOrdinaly && m_cOwner.IsCollectCone())           // コーンを回収
        {
            m_cOwner.ChangeState(0, StarState.CollectCone);
            return;
        }
        if ((Input.GetButtonDown("Telbox") || Input.GetKeyDown(KeyCode.Return)) && m_cOwner.StarOnTelbox)   // 電話ボックスIn
        {
            ExecuteEvents.Execute<ITellBoxInterface>(
               target: m_cOwner.TelBox,
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.OnBoxEnter());
            m_cOwner.TelboxIn = true;
            m_cOwner.StarArrayStop();
            m_cOwner.ChangeState(0, StarState.TelBox);
            return;
        }
        if((Input.GetButtonDown("Telbox") || Input.GetKeyDown(KeyCode.Return))  && MapManager.Instance.BackMapData[m_cOwner.Vertical][m_cOwner.Horizontal] == 90)   // エレベーターIn
        {
            m_cOwner.StarArrayStop();
            m_cOwner.ChangeState(0, StarState.EnterBuilding);
        }
        if (!m_cOwner.CheckGroundDintance())       // 落ちちゃいます
        {
            m_cOwner.ChangeState(0, StarState.Falling);
            return;
        }

        m_cOwner.FadeStarAnimation(StarAnimation.Wait);
    }

    public override void Exit()
    {

    }
}
