using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DItemDropManager : CStateBase<DroneStateManager>
{
    public DItemDropManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        Debug.Log("DroneItemDrop");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
