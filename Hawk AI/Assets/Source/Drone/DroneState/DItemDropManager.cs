using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        // アイテムを落とせる状態か
        if (m_cOwner.m_bCanDropItem)
        {
            // アイテムマネージャーから落ちているアイテムを数える
            var manager = ManagerObjectManager.Instance.GetGameObject("ItemManager");
            var itemmanager = manager.GetComponent<ItemManager>();
            int listcnt = itemmanager.GetGameObjectsList().Count;
            int itemcnt = 0;
            foreach(var val in itemmanager.GetGameObjectsList())
            {
                ExecuteEvents.Execute<IGeneralInterface>(
                    target: val,
                    eventData: null,
                    functor: (recieveTarget, y) => itemcnt += recieveTarget.GetGameObjectsList().Count);

            }
            // アイテム数が一定数未満の時に落とす
            Debug.Log("ItemNum : " + itemcnt);
            if (itemcnt < 3)
            {
                // 目標地点にアイテムが存在しない場合落とす
                bool canDrop = false;
                ExecuteEvents.Execute<IDronePointInterface>(
                   target: m_cOwner.m_gTarget,
                   eventData: null,
                   functor: (recieveTarget, y) => canDrop = recieveTarget.IsDrop());
                Debug.Log("TargetPos : " + m_cOwner.m_gTarget.name);
                Debug.Log("CanDrop : " + canDrop);
                if (canDrop)
                {
                    Debug.Log("DropItem : " + m_cOwner.m_gItemInfo.name);
                    m_cOwner.CreateDropBox();
                    m_cOwner.m_bCanDropItem = false;
                }
            }
        }

        m_cOwner.ChangeState(0, EDroneState.Move);
        m_cOwner.NowState = (int)EDroneState.Move;
    }

    public override void Exit()
    {

    }
}
