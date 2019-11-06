using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMoveManager : CStateBase<DroneStateManager>
{
    public DMoveManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        Debug.Log("DroneMove");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
