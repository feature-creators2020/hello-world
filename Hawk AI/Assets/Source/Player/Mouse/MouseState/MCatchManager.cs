using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;
using KeyBoardInput;

public class MCatchManager : CStateBase<MouseStateManager>
{
    public MCatchManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        ExecuteEvents.Execute<IFadeInterfase>(
        target: this.m_cOwner.targetCamera.gameObject,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeOut());
        m_cOwner.m_SEAudio.Play((int)SEAudioType.eSE_Debuff);   // 捕まえられたSE？
        m_cOwner.PlayAnimation(EMouseAnimation.Wait);
    }

    public override void Execute()
    {
        //Debug.Log("MouseState : Catch");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // 速度設定
        //m_cOwner.m_fmoveSpeed = m_cOwner.m_fDefaultSpeed;

        if (this.m_cOwner.targetCamera.gameObject.GetComponent<FadeEffect>().IsCompleteFlg)
        {
            // リスポーン処理
            RespawnPoint.Instance.Respawn(m_cOwner.gameObject);
            m_cOwner.ChangeState(0, EMouseState.Normal);

        }

        //// ゲームパッドの入力情報取得
        //m_cOwner.inputHorizontal = 0f;
        //m_cOwner.inputVertical = 0f;

        //m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        //m_cOwner.inputVertical = keyState.LeftStickAxis.y;

        //// カメラの方向から、x-z平面の単位ベクトルを取得
        //Vector3 cameraForward = Vector3.Scale(m_cOwner.targetCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        //// 移動量
        //Vector3 moveForward = cameraForward * m_cOwner.inputVertical + m_cOwner.targetCamera.transform.right * m_cOwner.inputHorizontal;

        ////m_cOwner.transform.position += moveForward * m_cOwner.m_fmoveSpeed * Time.deltaTime;

        //// キャラクターの向きを進行方向に
        //if (moveForward != Vector3.zero)
        //{
        //    m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);
        //}


        // Debug:ステート変更
    }

    public override void Exit()
    {
        ExecuteEvents.Execute<IFadeInterfase>(
        target: this.m_cOwner.targetCamera.gameObject,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeIn());

        m_cOwner.transform.localScale = m_cOwner.m_vDefaultScale;
    }
}
