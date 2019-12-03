using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedElevatorClose : CStateBase<FixedElevator>
{
    public FixedElevatorClose(FixedElevator _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        this.m_cOwner.PlayAnimation(FixedElevatorAnimation.Close);
        this.m_cOwner.FixedElevatorState = FixedElevatorState.Close;
    }

    public override void Execute()
    {
        if (this.m_cOwner.GetAnimation.isPlaying == false)
        {
            this.m_cOwner.ChangeState(0, FixedElevatorState.Stop);
        }
    }

    public override void Exit()
    {

    }

}
