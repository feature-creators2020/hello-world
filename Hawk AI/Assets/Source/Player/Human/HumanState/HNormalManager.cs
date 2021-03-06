﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.EventSystems;
using KeyBoardInput;


public class HNormalManager : CStateBase<HumanStateManager>
{
    public HNormalManager(HumanStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        Debug.Log("State:Normal");

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
                //Debug.Log("Catch!! : " + m_cOwner.hCatchZone.TargetObject.name);
                ExecuteEvents.Execute<IMouseInterface>(
                    target: m_cOwner.hCatchZone.TargetObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.Catched());
                //m_cOwner.hCatchZone.TargetObject;
            }
        }

        // アイテム使用
        this.m_cOwner.UseItem(playerNo, playerKeyNo);


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
        }

        // Debug:ステート変更
    }

    public override void Exit()
    {
        m_cOwner.EOldState = EHumanState.Normal;
    }

}
