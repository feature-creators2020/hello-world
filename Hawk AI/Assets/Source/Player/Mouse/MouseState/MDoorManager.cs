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
        //m_cOwner.EOldState = EMouseState.Door;
    }
}
