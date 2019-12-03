using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarWalk : CStateBase<Star>
{

    private Rigidbody rigidbody;                // 
    private Vector3 vec = Vector3.zero;         // work用
    private float m_fElapsedTime = 0.0f;        // ステートに入ってからの経過時間

    public StarWalk(Star _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        //エフェクト再生
        ExecuteEvents.Execute<IEffectControllerInterface>(
           target: GameObject.Find("RunningSmokeEffect"),
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.Play(this.m_cOwner.gameObject.transform.position));

        //サウンド再生
        ExecuteEvents.Execute<IAudioInterface>(
           target: GameObject.Find("StarAudio"),
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.Play((int)StarAudioType.Running));

        rigidbody = m_cOwner.GetComponent<Rigidbody>();

    }

    public override void Execute()
    {
        var speed = Input.GetAxis("Horizontal") * m_cOwner.StarWalkSpeed;

        rigidbody.isKinematic = false;

        m_cOwner.PlayStarAnimation(StarAnimation.Walk);

        if(speed < 0)
        {
            m_cOwner.RightLeft = false;
        }
        else if(speed > 0)
        {
            m_cOwner.RightLeft = true;
        }

        m_cOwner.Direction();
        
        vec.x = speed;
        m_cOwner.AddVelocity(vec);

        if (Input.GetButtonDown("Jump"))                        // ジャンプ
        { 
            m_cOwner.ChangeState(0, StarState.Jump);        
            return;
        }
        if (Input.GetButtonDown("PutCone") && !m_cOwner.StarOrdinaly && m_cOwner.IsPutCone())               // コーンを置く
        {
            m_cOwner.ChangeState(0, StarState.PutCone);
            return;
        }
        if (Input.GetButtonDown("PutCone") && !m_cOwner.StarOrdinaly && m_cOwner.IsCollectCone())          // コーンを回収する
        {
            m_cOwner.ChangeState(0, StarState.CollectCone);
            return;
        }
        if ((Input.GetButtonDown("Telbox") || Input.GetKeyDown(KeyCode.Return)) && m_cOwner.StarOnTelbox)   // 電話ボックスに入る
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
        if ((Input.GetButtonDown("Telbox") || Input.GetKeyDown(KeyCode.Return)) && MapManager.Instance.BackMapData[m_cOwner.Vertical][m_cOwner.Horizontal] == 90)   // エレベーターに入る
        {
            m_cOwner.StarArrayStop();
            m_cOwner.ChangeState(0, StarState.EnterBuilding);
            return;
        }

        if (speed == 0)     // Wait
        {
            m_cOwner.ChangeState(0, StarState.Wait);
            return;
        }
        if (!m_cOwner.CheckGroundDintance())    // 落ちちゃいます
        {
            m_cOwner.ChangeState(0, StarState.Falling);
        }
    }

    public override void Exit()
    {
        //エフェクト停止
        ExecuteEvents.Execute<IEffectControllerInterface>(
           target: GameObject.Find("RunningSmokeEffect"),
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.Stop());

        //サウンド停止
        ExecuteEvents.Execute<IAudioInterface>(
           target: GameObject.Find("StarAudio"),
           eventData: null,
           functor: (recieveTarget, y) => recieveTarget.Stop((int)StarAudioType.Running));
    }
}
