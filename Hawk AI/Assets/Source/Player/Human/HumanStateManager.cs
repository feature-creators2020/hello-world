using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.EventSystems;
using KeyBoardInput;

public interface IPlayerInterfase : IEventSystemHandler
{
    void SetMapPos(Vector2Int vector2Int);
}

public enum EHumanState
{
    Normal,
    SlowDown,
    Door,
}

public enum EHumanDirectionalState
{
    Forward,
    Left,
    Right,
    Back,
}

public class HumanStateManager : CStateObjectBase<HumanStateManager, EHumanState>, IPlayerInterfase
{
    [System.NonSerialized]
    public float inputHorizontal;               // コントローラーLスティック横軸情報
    [System.NonSerialized]
    public float inputVertical;                 // コントローラーLスティック縦軸情報
    public Camera targetCamera;                 // 対象のカメラ
    public GamePad.Index GamePadIndex;          // 対象のコントローラー
    public KeyBoard.Index KeyboardIndex;        // 対象のキーボード

    [System.NonSerialized]
    public float m_fmoveSpeed;                  // 計算時の速度

    public float m_fDefaultSpeed;               // 基礎速度
    public float m_fSlowDownRate;               // ネズミこうを踏んだとき(倍率)
    public float m_fDoorSpeed;                  // ドアの下を通るとき

    [System.NonSerialized]
    public float m_fSlowTime;                   // 速度低下の効果時間経過
    public float m_fLimitSlowTime;              // 速度低下の効果時間

    [System.NonSerialized]
    public GameObject GDoorData;
    [System.NonSerialized]
    public string m_sItemData;                // 現在取得しているアイテム
    [System.NonSerialized]
    public bool m_canPut = true;
    [System.NonSerialized]
    public bool m_isInvincible = false;         // アイテムを設置したときにそのアイテムと干渉させない
    [System.NonSerialized]
    int m_nInTrapCnt;
    [System.NonSerialized]
    public ItemManager m_Itemmanager;                    // アイテム管理
    [System.NonSerialized]
    public float m_fActionTime;                    // アクション経過時間
    public float m_fLimitActionTime = 2f;                // アクション時間

    [System.NonSerialized]
    public HCatchZone hCatchZone;               // 捕獲判定用
    [System.NonSerialized]
    public MoveCollider hMoveColliderScript;    // 移動判定用スクリプト

    /*{
        get { return m_fmoveSpeed; }
        set { m_fmoveSpeed = value; }
    }*/

    public float RunRate                // 別状態の速度倍率
    {
        get { return RunRate; }
        set { RunRate = value; }
    }

    public Vector2Int PlayerMapPos = new Vector2Int();


    [System.NonSerialized]
    public EHumanState EOldState;        // 前の状態を保持

    Transform char_trans;
    public LayerMask layermask;
    Vector3 char_velocity;
    Vector3 char_velocity_input;
    float RayLength;
    EHumanDirectionalState m_eHumanDirectionalState; //人の向き

    private GameObject m_cSetItemColliderObj; 


    // Start is called before the first frame update
    void Start()
    {
        var StateMachine = new CStateMachine<HumanStateManager>();
        m_cStateMachineList.Add(StateMachine);

        var Normal = new HNormalManager(this);
        var SlowDown = new HSlowDownManager(this);
        var Door = new HDoorManager(this);

        m_cStateList.Add(Normal);
        m_cStateList.Add(SlowDown);
        m_cStateList.Add(Door);

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EHumanState.Normal]);


        // 初期設定
        m_sItemData = null;
        m_nInTrapCnt = 0;
        //
        m_cSetItemColliderObj = this.gameObject.transform.GetChild(2).gameObject;
    }

    void Awake()
    {
        char_trans = transform;
        RayLength = 2.5f;
    }

    // Update is called once per frame
    public override void Update()
    {
        hCatchZone = this.transform.Find("CatchZone").GetComponent<HCatchZone>();
        hMoveColliderScript = this.gameObject.GetComponent<MoveCollider>();

        // 各状態の処理
        base.Update();
    }

    public virtual void UseItem(GamePad.Index playerNo, KeyBoard.Index playerKeyNo)
    {
        if (GamePad.GetButton(GamePad.Button.B, playerNo) || KeyBoard.GetButton(KeyBoard.Button.B, playerKeyNo))
        {
            // アイテムを所持しているか
            if (m_sItemData != null)
            {
                //　置く場所が適切かどうか
                if (IsUseItem() == true)
                {
                    // 　アクション時間が経過しているか
                    if (m_fActionTime <= 0f)
                    {
                        UseItem();
                        // アクション経過時間を再設定
                        m_fActionTime = m_fLimitActionTime;
                    }
                    else
                    {
                        // アクション時間を経過させる
                        m_fActionTime -= Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            // アクション経過時間を再設定
            m_fActionTime = m_fLimitActionTime;
        }
    }


    // Hack : make function and optimize
    bool IsUseItem()
    {
        float horizon, vertical;

        horizon = Mathf.Abs(this.gameObject.transform.forward.x);
        vertical = Mathf.Abs(this.gameObject.transform.forward.z);

        if (this.gameObject.transform.forward.x >= 0)
        {
            if (this.gameObject.transform.forward.z >= 0)
            {//右上

                if (horizon >= vertical)
                {// 右
                    return IsRightSetItem();
                }
                else
                {//　上

                    return IsForwardSetItem();
                }
            }
            else
            {//右下
                if (horizon >= vertical)
                { //右
                    return IsRightSetItem();
                }
                else
                { //　下
                    return IsBackSetItem();
                }

            }
        }
        else
        {
            if (this.gameObject.transform.forward.z >= 0)
            {//左上
                if (horizon >= vertical)
                {//　左
                    return IsLeftSetItem();
                }
                else
                {　//　上
                    return IsForwardSetItem();
                }

            }
            else
            {//左下
                if (horizon >= vertical)
                { //　左
                    return IsLeftSetItem();
                }
                else
                {　//　下
                    return IsBackSetItem();
                }
            }
        }


        return false;
    }

    private bool IsForwardSetItem()
    {
        int a, b;

        m_eHumanDirectionalState = EHumanDirectionalState.Forward;

        if (MapManager.Instance.MapData.Count >= PlayerMapPos.y + 1)
        {
            a = MapManager.Instance.MapData[PlayerMapPos.y + 1][PlayerMapPos.x];
            if (MapManager.Instance.MapData.Count >= PlayerMapPos.y + 2)
            {
                b = MapManager.Instance.MapData[PlayerMapPos.y + 2][PlayerMapPos.x];

                if ((a == (int)ObjectNo.NONE) && (b == (int)ObjectNo.NONE))
                {
                    bool bReturnFlg = false;
                    ExecuteEvents.Execute<ISettingTrapCollider>(
                    target: m_cSetItemColliderObj,
                    eventData: null,
                    functor: (recieveTarget, y) => bReturnFlg = recieveTarget.GetHitFlg());

                    if (bReturnFlg == false)
                        return true;
                }
            }
        }
        return false;
    }

    private bool IsLeftSetItem()
    {
        int a, b;

        m_eHumanDirectionalState = EHumanDirectionalState.Left;

        if (0 <= PlayerMapPos.x - 1)
        {
            a = MapManager.Instance.MapData[PlayerMapPos.y][PlayerMapPos.x - 1];
            if (0 <= PlayerMapPos.x - 2)
            {
                b = MapManager.Instance.MapData[PlayerMapPos.y][PlayerMapPos.x - 2];

                if ((a == (int)ObjectNo.NONE) && (b == (int)ObjectNo.NONE))
                {
                    bool bReturnFlg = false;
                    ExecuteEvents.Execute<ISettingTrapCollider>(
                    target: m_cSetItemColliderObj,
                    eventData: null,
                    functor: (recieveTarget, y) => bReturnFlg = recieveTarget.GetHitFlg());

                    if (bReturnFlg == false)
                        return true;
                }
            }
        }

        return false;
    }

    private bool IsRightSetItem()
    {
        int a, b;

        m_eHumanDirectionalState = EHumanDirectionalState.Right;

        if (MapManager.Instance.MapData[PlayerMapPos.y].Length >= PlayerMapPos.x + 1)
        {
            a = MapManager.Instance.MapData[PlayerMapPos.y][PlayerMapPos.x + 1];
            if (MapManager.Instance.MapData[PlayerMapPos.y].Length >= PlayerMapPos.x + 2)
            {
                b = MapManager.Instance.MapData[PlayerMapPos.y][PlayerMapPos.x + 2];

                if ((a == (int)ObjectNo.NONE) && (b == (int)ObjectNo.NONE))
                {
                    bool bReturnFlg = false;
                    ExecuteEvents.Execute<ISettingTrapCollider>(
                    target: m_cSetItemColliderObj,
                    eventData: null,
                    functor: (recieveTarget, y) => bReturnFlg = recieveTarget.GetHitFlg());
              
                    if(bReturnFlg == false)
                    return true;
                }
            }
        }

        return false;
    }


    private bool IsBackSetItem()
    {
        int a, b;

        m_eHumanDirectionalState = EHumanDirectionalState.Back;

        if (0 <= PlayerMapPos.y - 1)
        {
            a = MapManager.Instance.MapData[PlayerMapPos.y - 1][PlayerMapPos.x];
            if (0 <= PlayerMapPos.y - 2)
            {
                b = MapManager.Instance.MapData[PlayerMapPos.y - 2][PlayerMapPos.x];

                if ((a == (int)ObjectNo.NONE) && (b == (int)ObjectNo.NONE))
                {
                    bool bReturnFlg = false;
                    ExecuteEvents.Execute<ISettingTrapCollider>(
                    target: m_cSetItemColliderObj,
                    eventData: null,
                    functor: (recieveTarget, y) => bReturnFlg = recieveTarget.GetHitFlg());

                    if (bReturnFlg == false)
                        return true;
                }
            }
        }

        return false;
    }


    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter! : " + other);
        // トラップに当たる
        if (LayerMask.LayerToName(other.gameObject.layer) == "Trap")
        {
            if (m_isInvincible == false)
            {
                // ネズミ捕り
                if (other.gameObject.tag == "Mousetrap")
                {
                    ChangeState(0, EHumanState.SlowDown);
                }
            }
            else
            {
                m_nInTrapCnt++;
            }
        }

        // ドアに当たる
        if (other.tag == "DoorArea")
        {
            // アイテム設置不可能
            m_canPut = false;

            // 対象のドア情報を取得
            GDoorData = other.gameObject.transform.parent.Find("DoorScript").gameObject;
            //Debug.Log(other.gameObject.name);
            //Debug.Log(other.gameObject.transform.parent.Find("DoorScript").gameObject.name);

            // 状態を切り替える
            ChangeState(0, EHumanState.Door);
        }

        // アイテム取得
        if (LayerMask.LayerToName(other.gameObject.layer) == "Item")
        {
            if (m_sItemData == null)
            {
                // アイテムマネージャー取得
                var ItemManagerObject = ManagerObjectManager.Instance.GetGameObject("ItemManager");
                //Debug.Log(ItemManagerObject.name);
                m_Itemmanager = ItemManagerObject.GetComponent<ItemManager>();
                //Debug.Log(m_Itemmanager.name);

                // ネズミ捕り
                if (other.gameObject.tag == "Mousetrap")
                {
                    m_sItemData = "MousetrapManager";
                    //Debug.Log("GetItem");
                }

                // 取得したのでオブジェクトを消す
                Destroy(other.gameObject);
            }
            //Debug.Log("Item : " + m_sItemData);
        }


    }

    void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Trap")
        {
            if (m_isInvincible == false)
            {
                // ネズミ捕り
                if (other.gameObject.tag == "Mousetrap")
                {
                    m_fSlowTime = m_fLimitSlowTime; // 無敵状態を解除する
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Trap")
        {
            if (m_isInvincible == true)
            {
                // ネズミ捕り
                if (other.gameObject.tag == "Mousetrap")
                {
                    m_nInTrapCnt--;
                    if (m_nInTrapCnt <= 0)
                    {
                        m_isInvincible = false; // 無敵状態を解除する
                    }
                }
            }
            else
            {
                // ネズミ捕り
                if (other.gameObject.tag == "Mousetrap")
                {
                    Destroy(other.gameObject); // トラップを削除する
                }
            }
        }

        if (LayerMask.LayerToName(other.gameObject.layer) == "Door")
        {
            ChangeState(0, EOldState);
        }

        if (other.tag == "DoorArea")
        {
            // アイテム設置可能にする
            m_canPut = true;

            // 対象のドア情報を消す
            GDoorData = null;

            // 状態を切り替える
            ChangeState(0, EHumanState.Normal);
        }


    }


    // アイテム使用処理
    public void UseItem()
    {
        //Debug.Log("UseItem : " + m_sItemData);
        if(m_sItemData != null)
        {
            if (m_canPut) {
                Debug.Log("Put!");
                // プレハブを取得
                var item = ManagerObjectManager.Instance.GetGameObject(m_sItemData);
                Debug.Log("ItemManager : " + item);

                Vector2Int[] MapPos = new Vector2Int[2];
                Vector3 vector3 = Vector3.zero;

                switch (m_eHumanDirectionalState)
                {
                    case EHumanDirectionalState.Forward:
                        vector3 = this.transform.position + new Vector3(0f, 0f, 1.5f);
                        MapPos[0].x = PlayerMapPos.x;
                        MapPos[0].y = PlayerMapPos.y + 1;
                        MapPos[1].x = PlayerMapPos.x;
                        MapPos[1].y = PlayerMapPos.y + 2;
                        break;
                    case EHumanDirectionalState.Left:
                        vector3 = this.transform.position + new Vector3(-1.5f, 0f, 0f);
                        MapPos[0].x = PlayerMapPos.x - 1;
                        MapPos[0].y = PlayerMapPos.y;
                        MapPos[1].x = PlayerMapPos.x - 2;
                        MapPos[1].y = PlayerMapPos.y;
                        break;
                    case EHumanDirectionalState.Right:
                        vector3 = this.transform.position + new Vector3(1.5f, 0f, 0f);
                        MapPos[0].x = PlayerMapPos.x + 1;
                        MapPos[0].y = PlayerMapPos.y;
                        MapPos[1].x = PlayerMapPos.x + 2;
                        MapPos[1].y = PlayerMapPos.y;
                        break;
                    case EHumanDirectionalState.Back:
                        vector3 = this.transform.position + new Vector3(0f, 0f, -1.5f);
                        MapPos[0].x = PlayerMapPos.x;
                        MapPos[0].y = PlayerMapPos.y - 1;
                        MapPos[1].x = PlayerMapPos.x;
                        MapPos[1].y = PlayerMapPos.y - 2;
                        break;

                }

                // プレハブからインスタンスを生成
                ExecuteEvents.Execute<IItemInterface>(
                    target: item,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.Instant(vector3, Quaternion.identity));

                MapManager.Instance.MapData[MapPos[0].y][MapPos[0].x] = (int)ObjectNo.MOUSE_TRAP_LOW;
                MapManager.Instance.MapData[MapPos[1].y][MapPos[1].x] = (int)ObjectNo.MOUSE_TRAP_LOW;

                // インスタンスにmapの位置を登録
                ExecuteEvents.Execute<IMouseTrap>(
                    target: item,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.SetMapPosition(MapPos));


                // 所持アイテム情報を削除
                m_sItemData = null;
                // 無敵状態にする
                m_isInvincible = true;
            }
        }
    }

    public bool IsMove(Vector3 movepos)
    {
        hMoveColliderScript.JudgeCollision();

        if(hMoveColliderScript.hit.distance <= 0.25f)
        {
            return false;
        }

        return true;
    }

    public void RayJudge(Vector3 MoveForward)
    {
        RaycastHit hit;
        //**************壁にぶつかった際の移動を制限******************
        char_velocity = char_velocity_input;

        // メモ:前後と左右もそれぞれ別ける必要があるかもしれない
        //      進行方向が斜めの可能性があるため、当たったオブジェクトのベクトルが必要・・・？
        //前後方向                
        if ((Physics.Raycast(this.transform.position, targetCamera.transform.forward,out hit, RayLength,
            layermask, QueryTriggerInteraction.Ignore) && char_velocity.z <= 0) ||
            (Physics.Raycast(this.transform.position, -targetCamera.transform.forward, RayLength, layermask,
            QueryTriggerInteraction.Ignore) && char_velocity.z >= 0))
        {
            MoveForward = new Vector3(char_velocity.x, char_velocity.y, 0);
        }
        //左右方向              
        if ((Physics.Raycast(this.transform.position, targetCamera.transform.right, RayLength,
          layermask, QueryTriggerInteraction.Ignore) && char_velocity.x <= 0) ||
          (Physics.Raycast(this.transform.position, -targetCamera.transform.right, RayLength, layermask,
           QueryTriggerInteraction.Ignore) && char_velocity.x >= 0))
        {
            MoveForward = new Vector3(0, char_velocity.y, char_velocity.z);
        }

        transform.position += MoveForward * m_fmoveSpeed * Time.deltaTime;
    }

    public void SetMapPos(Vector2Int vector2Int)
    {
        PlayerMapPos = vector2Int;
    }
}
