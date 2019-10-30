using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class MUpManager : CStateBase<MouseStateManager>
{
    float Distance;    // よじ登りで段々速度を下げる
    Vector3 StartPos;   // 上る最初の地点
    Vector3 EndPos;     // 最終地点

    float speed = 1.0f;

    public MUpManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        StartPos = m_cOwner.transform.position;
        var UpPos = StartPos + new Vector3(0f, m_cOwner.m_GTargetBoxObject.transform.localScale.y, 0f);
        EndPos = UpPos + m_cOwner.transform.forward * 2.0f;
        Distance = Vector3.Distance(StartPos, EndPos);
        Debug.Log("StartPos : " + StartPos);
        Debug.Log("EndPos : " + EndPos);
    }

    public override void Execute()
    {
        Debug.Log("State:Up");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // ゲームパッドの入力情報取得
        m_cOwner.inputHorizontal = 0f;
        m_cOwner.inputVertical = 0f;

        m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        m_cOwner.inputVertical = keyState.LeftStickAxis.y;

        float presentLocation = (Time.time * speed) / Distance;

        m_cOwner.transform.position = Vector3.Slerp(StartPos, EndPos, presentLocation);

        // 減衰速度増加
        //DownSpeed += 0.1f;

        if(presentLocation >= 1.0f)
        {
            m_cOwner.ChangeState(0, m_cOwner.EOldState);
        }

        // Debug:ステート変更
        if (GamePad.GetButtonDown(GamePad.Button.X, playerNo))
        {
            m_cOwner.ChangeState(0, EMouseState.SlowDown);
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, playerNo))
        {
            m_cOwner.ChangeState(0, EMouseState.Door);
        }
    }

    public override void Exit()
    {
        //m_cOwner.EOldState = EMouseState.Normal;
    }
}
