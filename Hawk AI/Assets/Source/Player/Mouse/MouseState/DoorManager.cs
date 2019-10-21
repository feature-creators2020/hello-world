using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class DoorManager : CStateBase<MouseStateManager>
{
    public DoorManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        
    }

    public override void Execute()
    {
        Debug.Log("State:Door");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // ゲームパッドの入力情報取得
        m_cOwner.inputHorizontal = 0f;
        m_cOwner.inputVertical = 0f;

        m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        m_cOwner.inputVertical = keyState.LeftStickAxis.y;

        // カメラの方向から、x-z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(m_cOwner.targetCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 移動量
        Vector3 moveForward = cameraForward * m_cOwner.inputVertical + m_cOwner.targetCamera.transform.right * m_cOwner.inputHorizontal;

        // 速度低下率
        moveForward = moveForward / m_cOwner.m_fDoorRate;

        m_cOwner.transform.position += moveForward * m_cOwner.m_fmoveSpeed * Time.deltaTime;

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);
        }

        // Debug:ステート変更
        if (GamePad.GetButtonDown(GamePad.Button.X, playerNo))
        {
            m_cOwner.ChangeState(0, EMouseState.Normal);
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, playerNo))
        {
            m_cOwner.ChangeState(0, EMouseState.SlowDown);
        }

    }

    public override void Exit()
    {
        
    }
}
