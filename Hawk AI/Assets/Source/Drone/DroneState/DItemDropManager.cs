using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムを落とす状態
public class DItemDropManager : CStateBase<DroneStateManager>
{
    public DItemDropManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        Debug.Log("DroneItemDrop");
        m_cOwner.SelectItem();
    }

    public override void Execute()
    {
        Debug.Log("DropItem : " + m_cOwner.m_gItemInfo.name);
        m_cOwner.CreateDropBox();
        m_cOwner.ChangeState(0, EDroneState.Move);
        m_cOwner.NowState = (int)EDroneState.Move;
    }

    public override void Exit()
    {

    }
}
