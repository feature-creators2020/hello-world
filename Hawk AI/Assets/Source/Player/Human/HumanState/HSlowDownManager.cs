using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.EventSystems;
using KeyBoardInput;

public class HSlowDownManager : CStateBase<HumanStateManager>
{
    public HSlowDownManager(HumanStateManager _cOwner) : base(_cOwner) { }
    public override void Enter()
    {
        // 前の状態が速度低下以外のときは新しく時間を設定する
        //if (m_cOwner.EOldState != EHumanState.SlowDown)
        //{
        //    m_cOwner.m_fSlowTime = m_cOwner.m_fLimitSlowTime;
        //}
        Debug.Log("PlayDebuffSE");
        m_cOwner.m_SEAudio.Play((int)SEAudioType.eSE_Debuff);    // デバフSE
        m_cOwner.EOldState = EHumanState.SlowDown;
    }

    public override void Execute()
    {
        //Debug.Log("State:SlowDown");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);
        var playerKeyNo = (KeyBoard.Index)playerNo;
        var keyboardState = KeyBoard.GetState(m_cOwner.KeyboardIndex, false);

        // 捕獲処理
        if (m_cOwner.hCatchZone.isCatch)
        {
            //Debug.Log("in the Zone !!");
            if (GamePad.GetButtonDown(GamePad.Button.B, playerNo) || KeyBoard.GetButtonDown(KeyBoard.Button.B, playerKeyNo))
            {
                m_cOwner.PlayAnimation(EHumanAnimation.Catch);
                m_cOwner.ChangeState(0, EHumanState.Catch);
                return;
                ////Debug.Log("Catch!!");
                //ExecuteEvents.Execute<IMouseInterface>(
                //    target: m_cOwner.hCatchZone.TargetObject,
                //    eventData: null,
                //    functor: (recieveTarget, y) => recieveTarget.Catched());
                //m_cOwner.m_SEAudio.Play((int)SEAudioType.eSE_MouseCatching);    // キャッチSE
                //m_cOwner.hCatchZone.TargetObject;
            }
        }

        // アイテム使用
        if(this.m_cOwner.UseItem(playerNo, playerKeyNo))
        {
            return;
        }


        // 移動処理。アクションを起こしていないときに処理
        //if (m_cOwner.m_fActionTime == m_cOwner.m_fLimitActionTime)
        //{
            // 速度設定
            m_cOwner.m_fmoveSpeed = m_cOwner.m_fDefaultSpeed * m_cOwner.m_fSlowDownRate;

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
                m_cOwner.PlayAnimation(EHumanAnimation.Run);    // 走るアニメーション
                m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);
            }
            else
            {
                m_cOwner.PlayAnimation(EHumanAnimation.Wait);   // 待機アニメーション
            }

            // 移動処理
            m_cOwner.Move(moveForward);
        //}

        // タイマーが過ぎたらnormalに遷移する
        if (m_cOwner.m_fSlowTime <= 0f)
        {
            Debug.Log("Change SlowDown→Normal");
            m_cOwner.ChangeState(0, EHumanState.Normal);
        }


        // Debug:ステート変更
    }

    public override void Exit()
    {
        m_cOwner.EOldState = EHumanState.Normal;
    }

}
