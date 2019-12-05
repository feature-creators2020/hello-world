using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDroneInterfase : IEventSystemHandler
{
    void ChangeMoveState();
}


public enum EDroneState
{
    Stay,
    Move,
    TargetMove,
    Around,
    ItemDrop,
    MaxState
}

public class DroneStateManager : CStateObjectBase<DroneStateManager, EDroneState>, IDroneInterfase
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
    [System.NonSerialized]
    public TimeZoneManager m_TimeZoneManager;           // 昼夜の状態を取得する

    public GameObject m_gTarget;                    // 目標のオブジェクト
    public Vector3 m_vTargetPos;                    // 目標地点の位置情報

    public GameObject m_gItemInfo;                  // ドロップするアイテム情報
    public GameObject m_gDropObject;                // ドロップボックスのプレハブ

    public float m_fSpeed = 3f;                          // 移動速度

    public bool isReverse = false;                  // 巡回経路の逆順をする場合使用

    public float m_fItemDropTime = 20f;                   // アイテムを落とす間隔
    [System.NonSerialized]
    public float m_fItemDropCount;                  // アイテムを落とす間隔の処理
    [System.NonSerialized]
    public bool m_bCanDropItem;                     // アイテムを落とす判定
    [System.NonSerialized]
    public bool m_bIsNight = false;                 // 夜状態か
    [System.NonSerialized]
    public List<bool> m_bCheckResult_list;                   // ターゲット切り替えの時、対象にターゲットできたか
    public int m_nTargetNum;                        // どちらのネズミを追跡しているか

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
        m_fItemDropCount = 0f;
        m_bIsNight = false;
        m_bCheckResult_list = new List<bool>();

        m_vTargetPos = new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-100f, 100f));
    }

    // Update is called once per frame
    void Update()
    {
        // マネージャー取得
        var managerobject = ManagerObjectManager.Instance;
        m_PlayerManager = managerobject.GetGameObject("PlayerManager").GetComponent<PlayerManager>();
        m_ItemManager = managerobject.GetGameObject("ItemManager").GetComponent<ItemManager>();
        m_DronePointManager = managerobject.GetGameObject("DronePointManager").GetComponent<DronePointManager>();
        m_TimeZoneManager = managerobject.GetGameObject("TimeZoneManager").GetComponent<TimeZoneManager>();

        // アイテムのドロップ間隔処理
        if (!m_bCanDropItem)
        {
            m_fItemDropCount -= Time.deltaTime;
            if(m_fItemDropCount <= 0f)
            {
                m_bCanDropItem = true;
                m_fItemDropCount = m_fItemDropTime;
            }
        }

        // 昼夜状態取得
        if (m_TimeZoneManager.TimeZoneStatus == ETimeZone.eEvenning) // タイムマネージャーから昼夜の状態を取得し、判定する
        {
            // 夜状態に切り替える
            m_bIsNight = true;
        }
        else
        {
            m_bIsNight = false;
        }

        base.Update();

        // 状態遷移処理
        if (m_cStateMachineList[0].GetCurrentState() != m_cStateList[(int)EDroneState.TargetMove])
        {
            StateTime += Time.deltaTime;
            if (StateTime >= 10.0f)
            {
                NowState++;
                if (NowState >= (int)EDroneState.ItemDrop)
                {
                    NowState = (int)EDroneState.Move;
                }
                m_cStateMachineList[0].ChangeState(m_cStateList[NowState]);
                StateTime = 0f;
            }
        }
    }

    // 追従するターゲットを設定する
    public bool ChangeTarget()
    {
        float TDistance = 0;
        // ターゲット情報を初期化
        var gTarget = m_PlayerManager.GetGameObject(0, "Mouse");

        var MouseList = m_PlayerManager.GetGameObjectsList("Mouse");
        for (int i = 0; i < MouseList.Count; i++)
        {
            // リストの情報がないとき
            if (m_bCheckResult_list.Count < MouseList.Count)
            {
                m_bCheckResult_list.Add(false);
            }

            // 現在のオブジェクト情報
            var targetObj = m_PlayerManager.GetGameObject(i, "Mouse");
            //Debug.Log("TargetTestObject : " + targetObj.name + i);
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
                var OldTarget = gTarget;
                gTarget = targetObj;
                if (IsCanTarget(gTarget, i))
                {
                    m_nTargetNum = i;
                    // 追跡
                    m_bCheckResult_list[i] = true;
                }
                else
                {
                    // 別の目標を追跡
                    gTarget = OldTarget;
                    TDistance = 0;
                    m_bCheckResult_list[i] = false;
                }
            }

        }

        // 判定結果を取得(全てfalseの場合のみreturn false)
        foreach(var val in m_bCheckResult_list)
        {
            if (val)
            {
                m_gTarget = gTarget;
                return true;
            }
        }
        return false;
    }

    // 追跡するターゲットの位置更新
    public void UpdateTargetPosition()
    {
        //Debug.Log("TargetObject : " + m_gTarget.name);
        var target = m_gTarget.transform.position;
        m_vTargetPos = new Vector3(target.x, this.transform.position.y, target.z);
    }

    public bool IsCanTarget(GameObject TargetObject)
    {
        // ルームマネージャー取得
        var room = RoomManager.Instance;

        //Debug.Log("DroneTargetObject : " + TargetObject.name);
        if (!ReferenceEquals(TargetObject, null))
        {
            if (TargetObject.tag == "Mouse")
            {
                var TargetScript = TargetObject.GetComponent<MouseStateManager>();
                // パイプ、リスポーン時は追跡しない
                if (TargetScript.GetCurrentState() != TargetScript.GetStateBase(EMouseState.Pipe) &&
                    TargetScript.GetCurrentState() != TargetScript.GetStateBase(EMouseState.Catch))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsCanTarget(GameObject TargetObject, int _num)
    {
        // ルームマネージャー取得
        var room = RoomManager.Instance;

        //Debug.Log("DroneTargetObject : " + TargetObject.name);
        if (!ReferenceEquals(TargetObject, null)) {
            if (TargetObject.tag == "Mouse")
            {
                var TargetScript = TargetObject.GetComponent<MouseStateManager>();
                // パイプ、リスポーン時は追跡しない
                if (TargetScript.GetCurrentState() != TargetScript.GetStateBase(EMouseState.Pipe) &&
                    TargetScript.GetCurrentState() != TargetScript.GetStateBase(EMouseState.Catch))
                {
                    // 昼の時、同じエリアに存在しているか
                    if (m_bIsNight)
                    {
                        return true;
                    }
                    else
                    {
                        bool result = false;    // 同じエリアにいるか
                        switch (_num)
                        {
                            case 0:
                                result = (room.GetDroneIn() == room.GetMouse01In());
                                break;
                            case 1:
                                result = (room.GetDroneIn() == room.GetMouse02In());
                                break;
                            default:
                                result = false;
                                break;
                        }
                        if (result)
                        {
                            return true;
                        }
                        // 同じエリアに存在していなかったため、falseを返す
                        return false;
                    }
                }
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
        var drop = Instantiate(m_gDropObject, new Vector3(thisPos.x, thisPos.y - 1f, thisPos.z), this.gameObject.transform.rotation).GetComponent<DropItem>();
        drop.SetPoint(m_gTarget);
        // 自身のステートを変えてもらうために渡しておく
        drop.SetDroneObject(this.gameObject);
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
            //Debug.Log("ListCount : " + m_DronePointManager.GetGameObjectsList().Count);
            if (CheckDistance(m_gTarget.transform.position))
            {
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
                    if (nextPoint >= m_DronePointManager.GetGameObjectsList().Count)
                    {
                        nextPoint = 0;
                    }
                    m_gTarget = GetPoint(nextPoint);
                    //Debug.Log("currentPoint : " + GetCurrentPoint().name);  
                    //Debug.Log("nextPosint : " + GetPoint(nextPoint).name);
                    NowPoint = nextPoint;
                    m_vTargetPos = m_gTarget.transform.position;
                }
            }
        }
    }

    public bool CheckDistance(Vector3 target)
    {
        var testTarget = new Vector3(target.x, this.transform.position.y, target.z);
        if(Vector3.Distance(testTarget, transform.position) <= m_fSpeed * 0.5f)
        {
            return true;
        }
        return false;
    }

    public void ChangeMoveState()
    {
        if (NowState == (int)EDroneState.Move)
        {
            ChangeState(0, EDroneState.Move);
        }
    }
}
