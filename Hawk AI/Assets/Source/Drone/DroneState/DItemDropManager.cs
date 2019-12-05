using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// アイテムを落とす状態
public class DItemDropManager : CStateBase<DroneStateManager>
{
    public DItemDropManager(DroneStateManager _cOwner) : base(_cOwner) { }

    bool isMoved;
    bool isStateEnd;
    bool isDroped;      // 現在のステートでアイテムをドロップしたか

    public override void Enter()
    {
        //Debug.Log("DroneItemDrop");
        m_cOwner.SelectItem();
        isMoved = false;
        isStateEnd = false;
        isDroped = false;
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
            foreach (var val in itemmanager.GetGameObjectsList())
            {
                ExecuteEvents.Execute<IGeneralInterface>(
                    target: val,
                    eventData: null,
                    functor: (recieveTarget, y) => itemcnt += recieveTarget.GetGameObjectsList().Count);

            }
            // アイテム数が一定数未満の時に落とす
            //Debug.Log("ItemNum : " + itemcnt);
            //if (itemcnt < 3)
            //{
            // 目標地点にアイテムが存在しない場合落とす
            bool canDrop = false;
            ExecuteEvents.Execute<IDronePointInterface>(
               target: m_cOwner.m_gTarget,
               eventData: null,
               functor: (recieveTarget, y) => canDrop = recieveTarget.IsDrop());
            //Debug.Log("TargetPos : " + m_cOwner.m_gTarget.name);
            //Debug.Log("CanDrop : " + canDrop);
            if (canDrop)
            {
                // 移動してから落とす
                if (isMoved)
                {
                    //Debug.Log("DropItem : " + m_cOwner.m_gItemInfo.name);
                    m_cOwner.CreateDropBox();
                    m_cOwner.m_bCanDropItem = false;
                    isDroped = true;
                    isStateEnd = true;
                }
                else
                {
                    var target = new Vector3(m_cOwner.m_vTargetPos.x, m_cOwner.transform.position.y, m_cOwner.m_vTargetPos.z);
                    var distance = Vector3.Distance(target, m_cOwner.transform.position);
                    if(distance <= 0.1f)
                    {
                        isMoved = true;
                    }
                    m_cOwner.transform.rotation = Quaternion.Slerp(m_cOwner.transform.rotation, Quaternion.LookRotation(target - m_cOwner.transform.position), 0.1f);
                    m_cOwner.transform.position = Vector3.Lerp(m_cOwner.transform.position, target, 0.1f);
                }
            }
            else
            {
                isStateEnd = true;
            }
            //}
        }
        else
        {
            isStateEnd = true;
        }

        if (isStateEnd)
        {
            if (isDroped)
            {
                // アイテムが落ちきるまで待機
                m_cOwner.ChangeState(0, EDroneState.Stay);
            }
            else
            {
                m_cOwner.ChangeState(0, EDroneState.Move);
            }
            // 巡回時の状態の為、巡回状態を保持
            m_cOwner.NowState = (int)EDroneState.Move;
        }
    }

    public override void Exit()
    {

    }
}
