using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ターゲットの周りを飛んでいる状態
public class DAroundManager : CStateBase<DroneStateManager>
{
    public DAroundManager(DroneStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        Debug.Log("DroneAround");
        m_cOwner.NowState++;
    }

    public override void Execute()
    {
        //DebugMove();
        m_cOwner.UpdateTargetPosition();
        // 追跡可能か
        if (m_cOwner.IsCanTarget())
        {
            // 滑らかに回転して移動したい
            var target = m_cOwner.m_vTargetPos;
            // 距離が一定の範囲内に入ると追従状態に移行
            //if (Vector3.Distance(target, m_cOwner.transform.position) <= m_cOwner.m_fSpeed)
            {
                var position = Vector3.Lerp(m_cOwner.transform.position, target, 0.1f);
                //var moveFoward = Vector3.Normalize(position);
                m_cOwner.transform.rotation = Quaternion.LookRotation(position - m_cOwner.transform.position);
                //m_cOwner.transform.position += m_cOwner.transform.forward * m_cOwner.m_fSpeed * Time.deltaTime;
                m_cOwner.transform.position = position;
                //m_cOwner.ChangeState(0, EDroneState.Around);
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

    void DebugMove()
    {
        float moveX = Random.Range(-1f, 1f);
        float moveZ = Random.Range(-1f, 1f);
        m_cOwner.m_vTargetPos += new Vector3(moveX, 0f, moveZ);
    }
}
