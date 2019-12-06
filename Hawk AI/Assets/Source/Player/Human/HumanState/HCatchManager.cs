using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using KeyBoardInput;


public class HCatchManager : CStateBase<HumanStateManager>
{
    public HCatchManager(HumanStateManager _cOwner) : base(_cOwner) { }


    float DefaultSlowDownRate;


    public override void Enter()
    {
        m_cOwner.PlayAnimation(EHumanAnimation.Catch);
        DefaultSlowDownRate = m_cOwner.m_fSlowDownRate;
        m_cOwner.m_fSlowDownRate = 1f;
    }

    public override void Execute()
    {
        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);
        var playerKeyNo = (KeyBoard.Index)playerNo;
        var keyboardState = KeyBoard.GetState(m_cOwner.KeyboardIndex, false);

        //if (m_cOwner.m_fSlowDownRate <= 0.0f)
        //{
        //    m_cOwner.m_fSlowDownRate = 0f;
        //}
        //else
        //{
        //    m_cOwner.m_fSlowDownRate -= 0.1f;
        //}

        // 移動処理。アクションを起こしていないときに処理
        if (m_cOwner.m_fActionTime == m_cOwner.m_fLimitActionTime)
        {
            // 速度設定
            m_cOwner.m_fmoveSpeed = m_cOwner.m_fDefaultSpeed;

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

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);
            }

            // 移動処理
            m_cOwner.Move(moveForward * m_cOwner.m_fSlowDownRate);
        }

        // Debug:ステート変更
    }

    public override void Exit()
    {
        m_cOwner.m_fSlowDownRate = DefaultSlowDownRate;
    }

}

