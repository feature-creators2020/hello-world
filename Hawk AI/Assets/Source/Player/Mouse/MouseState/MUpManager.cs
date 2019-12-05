using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class MUpManager : CStateBase<MouseStateManager>
{
    float Distance;    // よじ登りで段々速度を下げる
    Vector3 StartPos;   // 上る最初の地点
    Vector3 EndPos;     // 最終地点

    float speed = 5.0f;
    float timer = 0f;

    public MUpManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        timer = 0f;
        StartPos = m_cOwner.transform.position;
        var TopPos = m_cOwner.m_GTargetBoxObject.transform.position + new Vector3(0f, m_cOwner.m_GTargetBoxObject.transform.localScale.y / 2f, 0f);
        var SubPos = TopPos - StartPos;
        var UpPos = StartPos + new Vector3(0f, SubPos.y, 0f);
        EndPos = UpPos - m_cOwner.m_TargetBoxNomal * 0.5f;
        Distance = Vector3.Distance(StartPos, EndPos);
        //Debug.Log(m_cOwner.m_GTargetBoxObject.name + ".lossyScale : " + m_cOwner.m_GTargetBoxObject.transform.localScale);
        //Debug.Log("StartPos : " + StartPos);
        //Debug.Log("EndPos : " + EndPos);
        m_cOwner.GravityOff();
    }

    public override void Execute()
    {
        //Debug.Log("State:Up");

        //var playerNo = m_cOwner.GamePadIndex;
        //var keyState = GamePad.GetState(playerNo, false);

        //// ゲームパッドの入力情報取得
        //m_cOwner.inputHorizontal = 0f;
        //m_cOwner.inputVertical = 0f;

        //m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        //m_cOwner.inputVertical = keyState.LeftStickAxis.y;

        float presentLocation = (timer * speed);// / Distance;

        m_cOwner.transform.position = Vector3.Slerp(StartPos, EndPos, presentLocation);
        LookAtPoint();

        timer += Time.deltaTime;

        if(presentLocation >= 1.0f)
        {
            m_cOwner.ChangeState(0, m_cOwner.EOldState);
        }

    }

    public override void Exit()
    {
        //m_cOwner.EOldState = EMouseState.Normal;
        m_cOwner.GravityOn();
        var fowardVec = Vector3.Scale(m_cOwner.transform.forward, new Vector3(1, 0, 1)).normalized;
        m_cOwner.transform.rotation = Quaternion.LookRotation(fowardVec);
    }

    private void LookAtPoint()
    {
        // キャラクターの向きを進行方向に
        Vector3 moveForward = EndPos - StartPos;
        m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);

    }

}
