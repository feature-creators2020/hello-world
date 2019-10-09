using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : CStateBase<GameManager>
{
    public GameMainManager(GameManager _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        Debug.Log(" NowState : GameMainManager");
    }
    public override void Execute()
    {
        if (Input.anyKeyDown)
        {
            this.m_cOwner.ChangeState(0, EGameState.End);
        }
    }

    public override void Exit()
    {

    }

}
