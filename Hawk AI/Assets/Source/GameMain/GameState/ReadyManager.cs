using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyManager : CStateBase<GameManager>
{
    public ReadyManager(GameManager _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : ReadyManager");
    }
    public override void Execute()
    {
        if(Input.anyKeyDown)
        {
            this.m_cOwner.ChangeState(0, EGameState.Main);
        }
    }

    public override void Exit()
    {

    }
}
