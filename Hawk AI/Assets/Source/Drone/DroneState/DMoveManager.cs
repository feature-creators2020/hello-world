using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 自由移動状態
public class DMoveManager : CStateBase<DroneStateManager>
{
    public DMoveManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        Debug.Log("DroneMove");
        m_cOwner.SelectPoint();
    }

    public override void Execute()
    {
        // 滑らかに回転して移動したい
        var target = new Vector3(m_cOwner.m_vTargetPos.x, m_cOwner.transform.position.y, m_cOwner.m_vTargetPos.z);
        //float t = 0;
        //Quaternion.Slerp(m_cOwner.transform.rotation, Quaternion.LookRotation(target - m_cOwner.transform.position), t);
        //target - m_cOwner.transform.position
        m_cOwner.transform.rotation = Quaternion.LookRotation(target - m_cOwner.transform.position);
        m_cOwner.transform.position += m_cOwner.transform.forward * m_cOwner.m_fSpeed * Time.deltaTime;
        // 距離が一定の範囲内に入ると追従状態に移行
        if (Vector3.Distance(target, m_cOwner.transform.position) <= m_cOwner.m_fSpeed * 0.1f)
        {
            Debug.Log("SelectPoint");
            m_cOwner.SelectPoint();
        }

    }

    public override void Exit()
    {

    }
}
