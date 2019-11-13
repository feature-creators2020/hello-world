using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ターゲットに向かう状態
public class DTargetMoveManager : CStateBase<DroneStateManager>
{
    public DTargetMoveManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        Debug.Log("DroneTargetMove");
        m_cOwner.ChangeTarget();
        m_cOwner.UpdateTargetPosition();
    }

    public override void Execute()
    {
        m_cOwner.UpdateTargetPosition();
        // 追跡可能か
        if (m_cOwner.IsCanTarget())
        {
            // 滑らかに回転して移動したい
            var target = new Vector3(m_cOwner.m_vTargetPos.x, m_cOwner.transform.position.y, m_cOwner.m_vTargetPos.z);
            //float t = 0;
            //Quaternion.Slerp(m_cOwner.transform.rotation, Quaternion.LookRotation(target - m_cOwner.transform.position), t);
            //target - m_cOwner.transform.position
            m_cOwner.transform.rotation = Quaternion.LookRotation(target - m_cOwner.transform.position);
            m_cOwner.transform.position += m_cOwner.transform.forward * m_cOwner.m_fSpeed * Time.deltaTime;
            // 距離が一定の範囲内に入ると追従状態に移行
            if (Vector3.Distance(target, m_cOwner.transform.position) <= m_cOwner.m_fSpeed)
            {
                m_cOwner.ChangeState(0, EDroneState.Around);
            }
        }
        else
        {
            // 待機状態に遷移
            m_cOwner.ChangeState(0, EDroneState.Stay);
            m_cOwner.NowState = (int)EDroneState.Stay;
        }
    }

    public override void Exit()
    {

    }
}
