using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 待機状態
public class DStayManager : CStateBase<DroneStateManager>
{
    public DStayManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        Debug.Log("DroneStay");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
