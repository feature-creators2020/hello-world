using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDroneState
{
    Stay,
    Move,
    TargetMove,
    Around,
    ItemDrop,
    MaxState
}

public class DroneStateManager : CStateObjectBase<DroneStateManager, EDroneState>
{

    //[System.NonSerialized]
    public float StateTime;                 // 状態遷移する時間
    public int NowState;                           // 今の状態

    [System.NonSerialized]
    public PlayerManager m_PlayerManager;           // 目標とするプレイヤーの情報を取得する
    [System.NonSerialized]
    public ItemManager m_ItemManager;               // ランダムにアイテムをドロップするときに使う

    public GameObject m_gTarget;                    // 目標のオブジェクト
    public Vector3 m_vTargetPos;                    // 目標地点の位置情報

    public float m_fSpeed = 3f;                          // 移動速度

    // Start is called before the first frame update
    void Start()
    {
        var StateMachine = new CStateMachine<DroneStateManager>();
        m_cStateMachineList.Add(StateMachine);

        var Stay = new DStayManager(this);
        var Move = new DMoveManager(this);
        var TargetMove = new DTargetMoveManager(this);
        var Around = new DAroundManager(this);
        var ItemDrop = new DItemDropManager(this);

        m_cStateList.Add(Stay);
        m_cStateList.Add(Move);
        m_cStateList.Add(TargetMove);
        m_cStateList.Add(Around);
        m_cStateList.Add(ItemDrop);

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EDroneState.Stay]);

        StateTime = 0f;
        NowState = 0;

        var managerobject = ManagerObjectManager.Instance;
        m_PlayerManager = managerobject.GetGameObject("PlayerManager").GetComponent<PlayerManager>();
        m_ItemManager = managerobject.GetGameObject("ItemManager").GetComponent<ItemManager>();
        m_vTargetPos = new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-100f, 100f));
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        // 状態遷移処理
        if (m_cStateMachineList[0].GetCurrentState() != m_cStateList[(int)EDroneState.TargetMove])
        {
            StateTime += Time.deltaTime;
            if (StateTime >= 10.0f)
            {
                NowState++;
                if (NowState >= (int)EDroneState.MaxState)
                {
                    NowState = 0;
                }
                m_cStateMachineList[0].ChangeState(m_cStateList[NowState]);
                StateTime = 0f;
            }
        }
    }

    // 追従するターゲットを設定する
    public void ChangeTarget()
    {
        float TDistance = 0;
        // ターゲット情報を初期化
        m_gTarget = m_PlayerManager.GetGameObject(0, "Mouse"); ;

        var MouseList = m_PlayerManager.GetGameObjectsList("Mouse");
        for (int i = 0; i < MouseList.Count; i++)
        {
            // 現在のオブジェクト情報
            var targetObj = m_PlayerManager.GetGameObject(i, "Mouse");
            Debug.Log("TargetTestObject : " + targetObj.name + i);
            float nDis = Vector3.Distance(targetObj.transform.position, this.transform.position);
            //if (m_gTarget == null)
            //{
            //    // ターゲット情報が入っていないとき
            //    m_gTarget = m_PlayerManager.GetGameObject(i, "Mouse");
            //    TDistance = nDis;
            //}
            //else
            //{
            if (TDistance == 0)
            {
                TDistance = nDis;
            }

            if (nDis <= TDistance)
            {
                // 現在のターゲットの方が距離が短いとき
                TDistance = nDis;
                // 更新前のターゲット情報
                var OldTarget = m_gTarget;
                m_gTarget = targetObj;
                if (IsCanTarget())
                {
                    // 追跡
                }
                else
                {
                    // 別の目標を追跡
                    m_gTarget = OldTarget;
                    TDistance = 0;
                }
            }
            //}

        }
    }

    public void UpdateTargetPosition()
    {
        Debug.Log("TargetObject : " + m_gTarget.name);
        var target = m_gTarget.transform.position;
        m_vTargetPos = new Vector3(target.x, this.transform.position.y, target.z);
    }

    public bool IsCanTarget()
    {
        if (m_gTarget != null) {
            var TargetScript = m_gTarget.GetComponent<MouseStateManager>();
            // パイプ、リスポーン時は追跡しない
            if (TargetScript.GetCurrentState() != TargetScript.GetStateBase(EMouseState.Pipe) &&
                TargetScript.GetCurrentState() != TargetScript.GetStateBase(EMouseState.Catch))
            {
                return true;
            }
        }
        return false;
    }
}
