using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTargetMoveManager : CStateBase<DroneStateManager>
{
    public DTargetMoveManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        Debug.Log("DroneTargetMove");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
