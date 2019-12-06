﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using KeyBoardInput;
using UnityEngine.EventSystems;

public class HRailManager : CStateBase<HumanStateManager>
{

    public HRailManager(HumanStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        //m_cOwner.GravityOff();
    }

    public override void Execute()
    {
        //Debug.Log("State:Rail");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);
        var playerKeyNo = (KeyBoard.Index)playerNo;
        var keyboardState = KeyBoard.GetState(m_cOwner.KeyboardIndex, false);

        // ゲームパッドの入力情報取得
        m_cOwner.inputHorizontal = 0f;
        m_cOwner.inputVertical = 0f;

        m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        m_cOwner.inputVertical = keyState.LeftStickAxis.y;

        // 捕獲処理
        if (m_cOwner.hCatchZone.isCatch)
        {
            //Debug.Log("in the Zone !!");
            if (GamePad.GetButtonDown(GamePad.Button.B, playerNo) || KeyBoard.GetButtonDown(KeyBoard.Button.B, playerKeyNo))
            {
                m_cOwner.PlayAnimation(EHumanAnimation.Catch);
                m_cOwner.ChangeState(0, EHumanState.Catch);
                return;
                //Debug.Log("Catch!! : " + m_cOwner.hCatchZone.TargetObject.name);
                //ExecuteEvents.Execute<IMouseInterface>(
                //    target: m_cOwner.hCatchZone.TargetObject,
                //    eventData: null,
                //    functor: (recieveTarget, y) => recieveTarget.Catched());
                ////m_cOwner.hCatchZone.TargetObject;
                //m_cOwner.m_SEAudio.Play((int)SEAudioType.eSE_MouseCatching);    // キャッチSE
            }
        }


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
                m_cOwner.PlayAnimation(EHumanAnimation.Run);    // 走るアニメーション
                m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);
            }
            else
            {
                m_cOwner.PlayAnimation(EHumanAnimation.Wait);   // 待機アニメーション
            }

            // 移動判定
            //if (m_cOwner.IsMove(moveForward))
            //{

            //}
            //else
            //{
            Vector3 railmove = Vector3.zero;
            ExecuteEvents.Execute<IRailInterfase>(
                    target: m_cOwner.m_GTargetBoxObject,
                    eventData: null,
                    functor: (recieveTarget, y) => railmove = recieveTarget.GetMove());
            //Debug.Log("RailObject : " + m_cOwner.m_GTargetBoxObject.name);
            //Debug.Log(railmove);

            // ベルトコンベアの移動量
            m_cOwner.transform.position += railmove;

            // 移動処理
            m_cOwner.Move(moveForward);


            //var TopPos = m_cOwner.m_GTargetBoxObject.transform.position + new Vector3(0f, m_cOwner.m_GTargetBoxObject.transform.localScale.y / 2f, 0f);
            //var UpPos = TopPos + new Vector3(0f, m_cOwner.transform.localScale.y, 0f);
            //Debug.Log("TargetRail : " + m_cOwner.m_GTargetBoxObject.name);
            //Debug.Log("TopPos : " + TopPos);
            //Debug.Log("UpPos : " + UpPos);
            //m_cOwner.transform.position = new Vector3(m_cOwner.transform.position.x, UpPos.y, m_cOwner.transform.position.z);


            // 接地判定
            Ray Downray = new Ray(m_cOwner.transform.position, -m_cOwner.transform.up);
            RaycastHit Downhit;
            Debug.DrawLine(m_cOwner.transform.position, m_cOwner.transform.position - m_cOwner.transform.up, Color.red);
            if (Physics.BoxCast(m_cOwner.transform.position, m_cOwner.transform.lossyScale * 0.5f, -m_cOwner.transform.up, out Downhit))
            {
                //Debug.Log("DownRootObject : " + Downhit.collider.gameObject.transform.parent.parent.gameObject.name);
                //Debug.Log("DownHumanRayHit : " + Downhit.collider.gameObject.name);
                //Debug.Log("DownHitTag : " + Downhit.collider.tag);

                var LayerName = LayerMask.LayerToName(Downhit.collider.gameObject.layer);
                var TagName = Downhit.collider.gameObject.tag;
                if (TagName != "Rail")
                {
                    m_cOwner.ChangeState(0, m_cOwner.EOldState);
                }
            }

        }
    }

    public override void Exit()
    {
        //m_cOwner.GravityOn();
    }
}
