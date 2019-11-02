using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class MDoorManager : CStateBase<MouseStateManager>
{
    public MDoorManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        
    }

    public override void Execute()
    {
        Debug.Log("State:Door");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // 速度設定
        if(m_cOwner.EOldState == EMouseState.SlowDown)
        {
            // 速度落とす
            m_cOwner.m_fmoveSpeed = m_cOwner.m_fDoorSpeed * m_cOwner.m_fSlowDownRate;

            // 経過時間処理
            m_cOwner.m_fSlowTime -= Time.deltaTime;
            // タイマーが過ぎてたら前の状態をnormalにする
            if (m_cOwner.m_fSlowTime <= 0f)
            {
                m_cOwner.EOldState = EMouseState.Normal;
            }
        }
        else
        {
            m_cOwner.m_fmoveSpeed = m_cOwner.m_fDoorSpeed;
        }


        // ゲームパッドの入力情報取得
        m_cOwner.inputHorizontal = 0f;
        m_cOwner.inputVertical = 0f;

        m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        m_cOwner.inputVertical = keyState.LeftStickAxis.y;

        // カメラの方向から、x-z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(m_cOwner.targetCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 移動量
        Vector3 moveForward = cameraForward * m_cOwner.inputVertical + m_cOwner.targetCamera.transform.right * m_cOwner.inputHorizontal;

        if (moveForward != Vector3.zero)
        {
            m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);
        }

        // 移動判定
        if (m_cOwner.IsMove(moveForward))
        {

        }
        else
        {
            var correctionMove = m_cOwner.hMoveColliderScript.hit.normal;
            moveForward += correctionMove;
            if (m_cOwner.IsMove(moveForward))
            {
                moveForward -= correctionMove;
            }
        }
        // 移動処理
        m_cOwner.transform.position += moveForward * m_cOwner.m_fmoveSpeed * Time.deltaTime;


        // Debug:ステート変更

    }

    public override void Exit()
    {
        //m_cOwner.EOldState = EMouseState.Door;
    }
}
