using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarEnterBuilding : CStateBase<Star>
{

    private bool m_bEnterOut = true;        // 入るときか出るときかのフラグ
    private bool m_bOutFlag = true;         // 出るときの初期化フラグ
    private float m_fLerpCounter = 0.0f;    // 位置補間のカウント保持
    private int m_nHitLayer;                // Rayの当たるレイヤーの入れ子  
    private Vector3 m_vTarget;              // 補間の目標位置
    private Vector3 m_vInitPos;             // 位置補間のもとの位置
    private Vector3 m_vBackVec = new Vector3(0.0f,0.0f,1.0f);   // Rayを飛ばす方向
    private Vector3 m_vBoxCol = Vector3.zero;   // BoxCastの大きさ
    private GameObject Elevator;            // エレベーターオブジェクトの入れ子、ものが変わらないようにステートの最初にセット

    public StarEnterBuilding(Star _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        m_nHitLayer = LayerMask.GetMask(new string[] { "Elevator" });
        RaycastHit hit;
        var isRayHit = Physics.BoxCast(m_cOwner.transform.position, m_vBoxCol, m_vBackVec, out hit, Quaternion.identity, 100.0f, m_nHitLayer);
        // エレベーターに当たったら大丈夫、当たらなかったらWaitへ
        if (isRayHit)
        {
            Elevator = hit.collider.gameObject;
            bool isEnterTo = false;
            ExecuteEvents.Execute<IFixedElevatorInterface>(
                target: Elevator,
                eventData: null,
                functor: (recieveTarget, y) => isEnterTo = recieveTarget.IsEnter(this.m_cOwner.gameObject, ObjectNo.Star));

            if (!isEnterTo)
            {
                m_cOwner.ChangeState(0, StarState.Wait);
            }
        }
        else
        {
            m_cOwner.ChangeState(0, StarState.Wait);
        }

        // 移動をとめる
        m_cOwner.AddVelocity(new Vector3(0, 0, 0));
        m_cOwner.transform.rotation = Quaternion.Euler(0, 0, 0.0f); // 後ろを向きます
        // 初期化
        m_fLerpCounter = 0.0f;
        m_vTarget = m_vInitPos = m_cOwner.transform.position;
        m_vTarget.x = Elevator.transform.position.x;
        m_vTarget.z = 1.0f;
        m_vBoxCol = new Vector3(0.25f, 0.5f, 0.5f);
       
        m_bEnterOut = true;
        m_bOutFlag = true;
    }

    public override void Execute()
    {     
        if (m_bEnterOut)    // 建物に入る
        {
            m_fLerpCounter += Time.deltaTime;
            m_cOwner.transform.position = Vector3.Lerp(m_vInitPos, m_vTarget, m_fLerpCounter);
            m_cOwner.PlayStarAnimation(StarAnimation.Walk);

            if (m_vTarget.z - m_cOwner.transform.position.z <= 0.001)   // 一定距離以内に入ると反転して待機
            {
                // 移動先、移動前の情報を初期化
                m_bEnterOut = false;
                m_fLerpCounter = 0.0f;

                // アニメーションを変更
                m_cOwner.PlayStarAnimation(StarAnimation.Wait);

                // 移動完了でエレベーターに投げかける
                ExecuteEvents.Execute<IFixedElevatorInterface>(
                target: Elevator,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Enter(m_cOwner.gameObject, ObjectNo.Star));
            }
        }
        else
        {                    // 建物から出る
            if (!m_cOwner.ElevatorGetOff)
            {
                // アニメーションはずっと再生
                m_cOwner.PlayStarAnimation(StarAnimation.Walk);

                // 初期化
                if (m_bOutFlag)
                {
                    m_fLerpCounter = 0.0f;
                    m_vTarget = m_vInitPos = m_cOwner.transform.position;
                    m_vTarget.z = 0.0f;
                    m_bOutFlag = false;
                }

                // 位置を補間
                m_fLerpCounter += Time.deltaTime;
                m_cOwner.transform.position = Vector3.Lerp(m_vInitPos, m_vTarget, m_fLerpCounter);

                // 移動完了でステート遷移
                if (m_cOwner.transform.position.z - m_vTarget.z <= 0.001)
                {
                    m_cOwner.OnElevatorTakeOn();
                    m_cOwner.ChangeState(0, StarState.Wait);
                }
            }
            else
            {
                // エレベーターの中に入って振り向く補間
                m_fLerpCounter += Time.deltaTime;
                m_cOwner.transform.rotation = Quaternion.Lerp(m_cOwner.transform.rotation, Quaternion.Euler(0.0f,180.0f,0.0f), m_fLerpCounter);
            }
        }
    }

    public override void Exit()
    {
        m_cOwner.StarArrayPlay();
    }
}
