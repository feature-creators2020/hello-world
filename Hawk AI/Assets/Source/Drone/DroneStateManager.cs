using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public int NowPoint;                            // 現在の巡回地点

    [System.NonSerialized]
    public PlayerManager m_PlayerManager;           // 目標とするプレイヤーの情報を取得する
    [System.NonSerialized]
    public ItemManager m_ItemManager;               // ランダムにアイテムをドロップするときに使う
    [System.NonSerialized]
    public DronePointManager m_DronePointManager;   // 巡回する地点の情報を取得する

    public GameObject m_gTarget;                    // 目標のオブジェクト
    public Vector3 m_vTargetPos;                    // 目標地点の位置情報

    public GameObject m_gItemInfo;                  // ドロップするアイテム情報
    public GameObject m_gDropObject;                // ドロップボックスのプレハブ

    public float m_fSpeed = 3f;                          // 移動速度

    public bool isReverse = false;                  // 巡回経路の逆順をする場合使用

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
        NowPoint = 0;

        m_vTargetPos = new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-100f, 100f));
    }

    // Update is called once per frame
    void Update()
    {
        var managerobject = ManagerObjectManager.Instance;
        m_PlayerManager = managerobject.GetGameObject("PlayerManager").GetComponent<PlayerManager>();
        m_ItemManager = managerobject.GetGameObject("ItemManager").GetComponent<ItemManager>();
        m_DronePointManager = managerobject.GetGameObject("DronePointManager").GetComponent<DronePointManager>();

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
        m_gTarget = m_PlayerManager.GetGameObject(0, "Mouse");

        var MouseList = m_PlayerManager.GetGameObjectsList("Mouse");
        for (int i = 0; i < MouseList.Count; i++)
        {
            // 現在のオブジェクト情報
            var targetObj = m_PlayerManager.GetGameObject(i, "Mouse");
            Debug.Log("TargetTestObject : " + targetObj.name + i);
            float nDis = Vector3.Distance(targetObj.transform.position, this.transform.position);
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

        }
    }

    // 追跡するターゲットの位置更新
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

    // ドロップするアイテムをランダムで選ぶ
    public void SelectItem()
    {
        m_gItemInfo = null;
        var r_num = Random.Range(0, m_ItemManager.GetGameObjectsList().Count);
        m_gItemInfo = m_ItemManager.GetGameObject(r_num);

        // 対象のゲームオブジェクトマネージャーからプレハブ情報を取得
        ExecuteEvents.Execute<IItemInterface>(
        target: m_gItemInfo,
        eventData: null,
        functor: (recieveTarget, y) => m_gItemInfo = recieveTarget.GetPrefab());
    }

    // ドロップアイテムを生成する
    public void CreateDropBox()
    {
        m_gDropObject.GetComponent<DropItem>().SetItem(m_gItemInfo);
        var thisPos = this.gameObject.transform.position;
        Instantiate(m_gDropObject, new Vector3(thisPos.x, thisPos.y - 1f, thisPos.z), this.gameObject.transform.rotation);
    }

    // 巡回地点の取得(指定)
    public GameObject GetPoint(int PointNo)
    {
        return m_DronePointManager.GetGameObject(PointNo);
    }

    // 巡回地点の取得(現在)
    public GameObject GetCurrentPoint()
    {
        return m_DronePointManager.GetGameObject(NowPoint);
    }

    public void SelectPoint()
    {
        // 前の目標が巡回地点じゃないとき
        if(m_gTarget == null || m_gTarget.tag != "DronePoint")
        {
            var posDistance = Vector3.Distance(this.transform.position, m_DronePointManager.GetGameObject(0).transform.position);
            // 最短の巡回地点を取得
            for(int i=0;i< m_DronePointManager.GetGameObjectsList().Count; i++)
            {
                var testDistance = Vector3.Distance(this.transform.position, m_DronePointManager.GetGameObject(i).transform.position);
                if(posDistance >= testDistance)
                {
                    posDistance = testDistance;
                    NowPoint = i;
                    m_gTarget = GetCurrentPoint();
                    m_vTargetPos = m_gTarget.transform.position;
                }
            }
        }
        else
        {
            Debug.Log("ListCount : " + m_DronePointManager.GetGameObjectsList().Count);
            // 巡回で次の地点を取得する
            if (isReverse)
            {
                var nextPoint = NowPoint - 1;
                if (nextPoint <= 0)
                {
                    nextPoint = m_DronePointManager.GetGameObjectsList().Count - 1;
                }
                m_gTarget = GetPoint(nextPoint);
                NowPoint = nextPoint;
                m_vTargetPos = m_gTarget.transform.position;
            }
            else
            {
                var nextPoint = NowPoint + 1;
                if(nextPoint >= m_DronePointManager.GetGameObjectsList().Count)
                {
                    nextPoint = 0;
                }
                m_gTarget = GetPoint(nextPoint);
                Debug.Log("currentPoint : " + GetCurrentPoint().name);  
                Debug.Log("nextPosint : " + GetPoint(nextPoint).name);
                NowPoint = nextPoint;
                m_vTargetPos = m_gTarget.transform.position;
            }
            
        }
    }
}
