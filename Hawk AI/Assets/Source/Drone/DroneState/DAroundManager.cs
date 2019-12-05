using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ターゲットの周りを飛んでいる状態
public class DAroundManager : CStateBase<DroneStateManager>
{
    public DAroundManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        //Debug.Log("DroneAround");
        m_cOwner.NowState++;
    }

    public override void Execute()
    {
        //DebugMove();
        m_cOwner.UpdateTargetPosition();
        // 追跡可能か
        if (m_cOwner.IsCanTarget(m_cOwner.m_gTarget))
        {
            // 滑らかに回転して移動したい
            var target = new Vector3(m_cOwner.m_vTargetPos.x, m_cOwner.transform.position.y, m_cOwner.m_vTargetPos.z);
            var distance = Vector3.Distance(target, m_cOwner.transform.position);
            if (distance > 0.01f)
            {
                m_cOwner.transform.rotation = Quaternion.Slerp(m_cOwner.transform.rotation, Quaternion.LookRotation(target - m_cOwner.transform.position), 0.1f);
            }
            m_cOwner.transform.position = Vector3.Lerp(m_cOwner.transform.position, target, 0.1f);
        }
        else
        {
            // 待機状態に遷移
            m_cOwner.ChangeState(0, EDroneState.Stay);
            // ↓追従時間を一定にするために待機のステート保持はしない
            //m_cOwner.NowState = (int)EDroneState.Stay;
        }
    }

    public override void Exit()
    {

    }

    void DebugMove()
    {
        float moveX = Random.Range(-1f, 1f);
        float moveZ = Random.Range(-1f, 1f);
        m_cOwner.m_vTargetPos += new Vector3(moveX, 0f, moveZ);
    }
}
