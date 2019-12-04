using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using KeyBoardInput;

public class MSlowDownManager : CStateBase<MouseStateManager>
{

    float DefaultSlowDownRate;

    public MSlowDownManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        DefaultSlowDownRate = m_cOwner.m_fSlowDownRate;
        // 前の状態が速度低下以外のときは新しく時間を設定する
        if (m_cOwner.EOldState != EMouseState.SlowDown)
        {
            m_cOwner.m_fSlowTime = m_cOwner.m_fLimitSlowTime;
        }
    }

    public override void Execute()
    {
        //Debug.Log("State:SlowDown");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);
        var playerKeyNo = (KeyBoard.Index)playerNo;
        var keyboardState = KeyBoard.GetState(m_cOwner.KeyboardIndex, false);

        // 速度設定
        m_cOwner.m_fmoveSpeed *= m_cOwner.m_fSlowDownRate;
        if(m_cOwner.m_fSlowDownRate <= 0.0f)
        {
            m_cOwner.m_fSlowDownRate = 0f;
        }
        else
        {
            m_cOwner.m_fSlowDownRate -= 0.01f;
        }

        // ゲームパッドの入力情報取得
        m_cOwner.inputHorizontal = 0f;
        m_cOwner.inputVertical = 0f;

        m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        m_cOwner.inputVertical = keyState.LeftStickAxis.y;
        m_cOwner.inputHorizontal += keyboardState.LeftStickAxis.x;
        m_cOwner.inputVertical += keyboardState.LeftStickAxis.y;

        // カメラの方向から、x-z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(m_cOwner.targetCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 移動量
        Vector3 moveForward = cameraForward * m_cOwner.inputVertical + m_cOwner.targetCamera.transform.right * m_cOwner.inputHorizontal;

        if (moveForward != Vector3.zero)
        {
            m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);
        }

        // 移動処理
        m_cOwner.Move(moveForward * m_cOwner.m_fSlowDownRate);

        // 経過時間処理
        m_cOwner.m_fSlowTime -= Time.deltaTime;
        // タイマーが過ぎたらnormalに遷移する
        if (m_cOwner.m_fSlowTime <= 0f)
        {
            m_cOwner.ChangeState(0, EMouseState.Normal);
        }

        // Debug:ステート変更
    }

    public override void Exit()
    {
        m_cOwner.m_fSlowDownRate = DefaultSlowDownRate;
        //m_cOwner.EOldState = EMouseState.SlowDown;
    }

}
