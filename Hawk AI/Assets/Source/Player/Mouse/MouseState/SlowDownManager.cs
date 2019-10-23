using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class SlowDownManager : CStateBase<MouseStateManager>
{


    public SlowDownManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        // 前の状態が速度低下以外のときは新しく時間を設定する
        if (m_cOwner.EOldState != EMouseState.SlowDown)
        {
            m_cOwner.m_fSlowTime = 10f;
        }
    }

    public override void Execute()
    {
        Debug.Log("State:SlowDown");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // 速度設定
        m_cOwner.m_fmoveSpeed = m_cOwner.m_fDefaultSpeed * m_cOwner.m_fSlowDownRate;

        // 経過時間処理
        m_cOwner.m_fSlowTime -= Time.deltaTime;
        // タイマーが過ぎたらnormalに遷移する
        if (m_cOwner.m_fSlowTime <= 0f)
        {
            m_cOwner.ChangeState(0, EMouseState.Normal);
        }

        // Debug:ステート変更
        if (GamePad.GetButtonDown(GamePad.Button.X, playerNo))
        {
            m_cOwner.ChangeState(0, EMouseState.Door);
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, playerNo))
        {
            m_cOwner.ChangeState(0, EMouseState.Normal);
        }
    }

    public override void Exit()
    {
        m_cOwner.EOldState = EMouseState.SlowDown;
    }

}
