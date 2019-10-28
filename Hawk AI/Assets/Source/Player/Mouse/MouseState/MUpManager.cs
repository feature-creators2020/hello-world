using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class MUpManager : CStateBase<MouseStateManager>
{
    float DownSpeed;    // よじ登りで段々速度を下げる

    public MUpManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        DownSpeed = 0f;
    }

    public override void Execute()
    {
        Debug.Log("State:Up");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // 速度設定
        m_cOwner.m_fmoveSpeed = m_cOwner.m_fDefaultSpeed - DownSpeed;

        // ゲームパッドの入力情報取得
        m_cOwner.inputHorizontal = 0f;
        m_cOwner.inputVertical = 0f;

        m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        m_cOwner.inputVertical = keyState.LeftStickAxis.y;

        // カメラの方向から、x-z平面の単位ベクトルを取得
        Vector3 cameraUp = Vector3.Scale(m_cOwner.targetCamera.transform.forward, new Vector3(1, 1, 0)).normalized;

        // 移動量
        Vector3 moveForward = -cameraUp * m_cOwner.inputVertical; // + m_cOwner.targetCamera.transform.right * m_cOwner.inputHorizontal;

        m_cOwner.transform.position += moveForward * m_cOwner.m_fmoveSpeed * Time.deltaTime;

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);
        }

        // 減衰速度増加
        //DownSpeed += 0.1f;


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
