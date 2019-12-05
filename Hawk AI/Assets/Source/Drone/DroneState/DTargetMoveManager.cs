using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ターゲットに向かう状態
public class DTargetMoveManager : CStateBase<DroneStateManager>
{
    public DTargetMoveManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        //Debug.Log("DroneTargetMove");
        // ターゲットできたかで判定をする
        if (m_cOwner.ChangeTarget())
        {
            // ターゲットの位置を取得
            m_cOwner.UpdateTargetPosition();
        }
        else
        {
            // 待機状態に遷移→移動状態
            m_cOwner.ChangeState(0, EDroneState.Move);
            m_cOwner.NowState = (int)EDroneState.Move;
        }
    }

    public override void Execute()
    {
        // 追跡可能か
        if (m_cOwner.IsCanTarget(m_cOwner.m_gTarget))
        {
            // ターゲットの位置を更新
            m_cOwner.UpdateTargetPosition();
            // 滑らかに回転して移動したい
            var target = new Vector3(m_cOwner.m_vTargetPos.x, m_cOwner.transform.position.y, m_cOwner.m_vTargetPos.z);
            //float t = 0;
            //Quaternion.Slerp(m_cOwner.transform.rotation, Quaternion.LookRotation(target - m_cOwner.transform.position), t);
            //target - m_cOwner.transform.position
            m_cOwner.transform.rotation = Quaternion.Slerp(m_cOwner.transform.rotation, Quaternion.LookRotation(target - m_cOwner.transform.position), 0.1f);
            //m_cOwner.transform.rotation = Quaternion.LookRotation(target - m_cOwner.transform.position);
            m_cOwner.transform.position += m_cOwner.transform.forward * m_cOwner.m_fSpeed * Time.deltaTime;
            // 距離が一定の範囲内に入ると追従状態に移行
            if (Vector3.Distance(target, m_cOwner.transform.position) <= m_cOwner.m_fSpeed)
            {
                m_cOwner.ChangeState(0, EDroneState.Around);
            }
        }
        else
        {
            // 待機状態に遷移→移動状態
            m_cOwner.ChangeState(0, EDroneState.Move);
            m_cOwner.NowState = (int)EDroneState.Move;
        }
    }

    public override void Exit()
    {

    }
}
