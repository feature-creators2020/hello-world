using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : CStateBase<GameManager>
{
    public EndManager(GameManager _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : EndManager");
    }
    public override void Execute()
    {
        if (Input.anyKeyDown)
        {
            this.m_cOwner.ChangeState(0, EGameState.Ready);
        }
    }

    public override void Exit()
    {

    }
}
