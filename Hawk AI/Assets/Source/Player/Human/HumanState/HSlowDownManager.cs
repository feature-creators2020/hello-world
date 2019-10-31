using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.EventSystems;

public class HSlowDownManager : CStateBase<HumanStateManager>
{
    public HSlowDownManager(HumanStateManager _cOwner) : base(_cOwner) { }
    public override void Enter()
    {
        // 前の状態が速度低下以外のときは新しく時間を設定する
        if (m_cOwner.EOldState != EHumanState.SlowDown)
        {
            m_cOwner.m_fSlowTime = m_cOwner.m_fLimitSlowTime;
        }
    }

    public override void Execute()
    {
        Debug.Log("State:SlowDown");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // 捕獲処理
        if (m_cOwner.hCatchZone.isCatch)
        {
            Debug.Log("in the Zone !!");
            if (GamePad.GetButtonDown(GamePad.Button.B, playerNo))
            {
                Debug.Log("Catch!!");
                ExecuteEvents.Execute<IMouseInterface>(
                    target: m_cOwner.hCatchZone.TargetObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.Catched());
                //m_cOwner.hCatchZone.TargetObject;
            }
        }

        // アイテム使用
        if (GamePad.GetButton(GamePad.Button.B, playerNo))
        {
            // アイテムを所持しているか
            if (m_cOwner.m_sItemData != null)
            {
                // 　アクション時間が経過しているか
                if (m_cOwner.m_fActionTime <= 0f)
                {
                    m_cOwner.UseItem();
                    // アクション経過時間を再設定
                    m_cOwner.m_fActionTime = m_cOwner.m_fLimitActionTime;
                }
                else
                {
                    // アクション時間を経過させる
                    m_cOwner.m_fActionTime -= Time.deltaTime;
                }
            }
        }
        else
        {
            // アクション経過時間を再設定
            m_cOwner.m_fActionTime = m_cOwner.m_fLimitActionTime;
        }

        // 移動処理。アクションを起こしていないときに処理
        if (m_cOwner.m_fActionTime == m_cOwner.m_fLimitActionTime)
        {
            // 速度設定
            m_cOwner.m_fmoveSpeed = m_cOwner.m_fDefaultSpeed * m_cOwner.m_fSlowDownRate;

            // ゲームパッドの入力情報取得
            m_cOwner.inputHorizontal = 0f;
            m_cOwner.inputVertical = 0f;

            m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
            m_cOwner.inputVertical = keyState.LeftStickAxis.y;

            // カメラの方向から、x-z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(m_cOwner.targetCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 移動量
            Vector3 moveForward = cameraForward * m_cOwner.inputVertical + m_cOwner.targetCamera.transform.right * m_cOwner.inputHorizontal;

            // キャラクターの向きを進行方向に
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
                moveForward += m_cOwner.hMoveColliderScript.hit.normal * m_cOwner.m_fmoveSpeed;
            }
            // 移動処理
            m_cOwner.transform.position += moveForward * m_cOwner.m_fmoveSpeed * Time.deltaTime;

        }

        // 経過時間処理
        m_cOwner.m_fSlowTime -= Time.deltaTime;
        // タイマーが過ぎたらnormalに遷移する
        if (m_cOwner.m_fSlowTime <= 0f)
        {
            m_cOwner.ChangeState(0, EHumanState.Normal);
        }


        // Debug:ステート変更
        if (GamePad.GetButtonDown(GamePad.Button.X, playerNo))
        {
            m_cOwner.ChangeState(0, EHumanState.Door);
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, playerNo))
        {
            m_cOwner.ChangeState(0, EHumanState.Normal);
        }
    }

    public override void Exit()
    {
        m_cOwner.EOldState = EHumanState.SlowDown;
    }

}
