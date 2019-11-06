using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAroundManager : CStateBase<DroneStateManager>
{
    public DAroundManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        Debug.Log("DroneAround");
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}
