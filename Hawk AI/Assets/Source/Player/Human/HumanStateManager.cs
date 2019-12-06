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
    Up,
    Rail,
    ForcedWait,
    Catch,
    Put
}

public enum EHumanDirectionalState
{
    Forward,
    Left,
    Right,
    Back,
}

public enum EHumanAnimation
{
    Wait,
    Run,
    Catch,
    Put,
    Jump
}


public interface IHumanInterface : IEventSystemHandler
{
    void ChangeUpState(GameObject _Target);
}

public class HumanStateManager : CStateObjectBase<HumanStateManager, EHumanState>, IPlayerInterfase, IHumanInterface
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
    [System.NonSerialized]
    public float m_fLimitActionTime = 0.5f;                // アクション時間

    [System.NonSerialized]
    public HCatchZone hCatchZone;               // 捕獲判定用
    [System.NonSerialized]
    public MoveCollider hMoveColliderScript;    // 移動判定用スクリプト

    [System.NonSerialized]
    public Vector3 m_TargetBoxNomal;            // 上る段の面の法線ベクトル

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

    [System.NonSerialized]
    public GameObject m_GTargetBoxObject;

    public string[] AnimationString = { "Human4_Wait", "Human4_Run", "Human4_Catch", "Human4_Put", "Human4_Jump" };          // アニメーション名
    private int m_nAnimationNo;                                      // 再生中アニメーション番号
    private Animation m_cAnimation;                                  // アニメーション      

    public SEAudio m_SEAudio;           // se


    // Start is called before the first frame update
    void Start()
    {
        var StateMachine = new CStateMachine<HumanStateManager>();
        m_cStateMachineList.Add(StateMachine);

        var Normal = new HNormalManager(this);
        var SlowDown = new HSlowDownManager(this);
        var Door = new HDoorManager(this);
        var Up = new HUpManager(this);
        var Rail = new HRailManager(this);
        var ForcedWait = new HForcedWaitManager(this);
        var Catch = new HCatchManager(this);
        var Put = new HPutManager(this);

        m_cStateList.Add(Normal);
        m_cStateList.Add(SlowDown);
        m_cStateList.Add(Door);
        m_cStateList.Add(Up);
        m_cStateList.Add(Rail);
        m_cStateList.Add(ForcedWait);
        m_cStateList.Add(Catch);
        m_cStateList.Add(Put);

        m_cAnimation = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Animation>();

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EHumanState.ForcedWait]);

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
        // se取得
        m_SEAudio = ManagerObjectManager.Instance.GetGameObject("SEAudio").GetComponent<SEAudio>();

        // 各状態の処理
        base.Update();

        if(m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)EHumanState.ForcedWait])
        {
            return;
        }

        //var startpos = this.transform.position /*+ new Vector3(0f, -0.8f)*/;
        //Debug.DrawLine(startpos, startpos + this.transform.forward, Color.red);
        if (m_cStateMachineList[0].GetCurrentState() != m_cStateList[(int)EHumanState.Up])
        {
            Ray ray = new Ray(this.transform.position /*+ new Vector3(0f,-0.8f)*/, this.transform.forward);
            Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
            RaycastHit hit;
            // 正面方向にボックスキャスト(ベルトコンベアの側面に当たっているか)
            if (Physics.BoxCast(transform.position, transform.lossyScale * 0.5f, transform.forward, out hit, transform.rotation, 0.5f))
            {
                //Debug.Log("RootObject : " + hit.collider.gameObject.transform.root.gameObject.name);
                //Debug.Log("HumanRayHit : " + hit.collider.gameObject.name);
                //Debug.Log("HitTag : " + hit.collider.tag);
                var LayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                var TagName = hit.collider.gameObject.tag;
                //Debug.Log("LayerName : " + LayerName);
                if (LayerName == "Rail")
                {
                    if (TagName == "CanClimbing")
                    {
                        //Debug.Log("ChangeState");
                        m_GTargetBoxObject = hit.collider.gameObject;
                        m_TargetBoxNomal = hit.normal;
                        ChangeState(0, EHumanState.Up);
                        return;
                    }
                }
            }

            // 下に向けてボックスキャスト(ベルトコンベアに当たっているか)
            Ray Downray = new Ray(transform.position, -transform.up);
            RaycastHit Downhit;
            Debug.DrawLine(transform.position, transform.position - transform.up, Color.red);
            if (Physics.BoxCast(transform.position, transform.lossyScale * 0.5f, -transform.up, out Downhit))
            {
                //Debug.Log("DownRootObject : " + Downhit.collider.gameObject.transform.parent.parent.gameObject.name);
                //Debug.Log("DownHumanRayHit : " + Downhit.collider.gameObject.name);
                //Debug.Log("DownHitTag : " + Downhit.collider.tag);

                var LayerName = LayerMask.LayerToName(Downhit.collider.gameObject.layer);
                var TagName = Downhit.collider.gameObject.tag;
                if (LayerName == "Rail")
                {
                    if (TagName == "Rail")
                    {
                        m_GTargetBoxObject = Downhit.collider.gameObject.transform.parent.parent.gameObject;
                        ChangeState(0, EHumanState.Rail);
                    }
                }
                else
                {
                    if (CheckCurrentState(EHumanState.Rail))
                    {
                        ChangeState(0, EHumanState.Normal);
                    }
                }
            }

        }

    }

    public virtual bool UseItem(GamePad.Index playerNo, KeyBoard.Index playerKeyNo)
    {
        if (GamePad.GetButton(GamePad.Button.B, playerNo) || KeyBoard.GetButton(KeyBoard.Button.B, playerKeyNo))
        {
            // アイテムを所持しているか
            if (m_sItemData != null)
            {
                //　置く場所が適切かどうか
                if (IsUseItem() == true)
                {
                    if (CheckCurrentState(EHumanState.Put))
                    {
                        // 　アクション時間が経過しているか
                        if (m_fActionTime <= 0f)
                        {
                            //UseItem();
                            //m_SEAudio.Play((int)SEAudioType.eSE_FallOutItem);   // 設置SE
                            // アクション経過時間を再設定
                            m_fActionTime = m_fLimitActionTime;
                            ItemHolderManager.Instance.UsingFromHolder(0, playerNo, playerKeyNo);
                        }
                        else
                        {
                            // アクション時間を経過させる
                            m_fActionTime -= Time.deltaTime;
                            float TimeParLimitTime = (m_fLimitActionTime - m_fActionTime) / m_fLimitActionTime;
                            ItemHolderManager.Instance.UsingFromHolder(TimeParLimitTime, playerNo, playerKeyNo);
                        }
                    }
                    else
                    {
                        PlayAnimation(EHumanAnimation.Put);
                        ChangeState(0, EHumanState.Put);
                        return true;
                    }
                }
            }
        }
        else
        {
            // アクション経過時間を再設定
            m_fActionTime = m_fLimitActionTime;
            ItemHolderManager.Instance.UsingFromHolder(0, playerNo, playerKeyNo);
            ChangeState(0, EOldState);
        }
        return false;
    }


    // Hack : make function and optimize
    bool IsUseItem()
    {
        float horizon, vertical;

        horizon = Mathf.Abs(this.gameObject.transform.forward.x);
        vertical = Mathf.Abs(this.gameObject.transform.forward.z);

        //if (this.gameObject.transform.forward.x >= 0)
        //{
        //    if (this.gameObject.transform.forward.z >= 0)
        //    {//右上

        //        if (horizon >= vertical)
        //        {// 右
        //            return IsRightSetItem();
        //        }
        //        else
        //        {//　上

        //            return IsForwardSetItem();
        //        }
        //    }
        //    else
        //    {//右下
        //        if (horizon >= vertical)
        //        { //右
        //            return IsRightSetItem();
        //        }
        //        else
        //        { //　下
        //            return IsBackSetItem();
        //        }

        //    }
        //}
        //else
        //{
        //    if (this.gameObject.transform.forward.z >= 0)
        //    {//左上
        //        if (horizon >= vertical)
        //        {//　左
        //            return IsLeftSetItem();
        //        }
        //        else
        //        {　//　上
        //            return IsForwardSetItem();
        //        }

        //    }
        //    else
        //    {//左下
        //        if (horizon >= vertical)
        //        { //　左
        //            return IsLeftSetItem();
        //        }
        //        else
        //        {　//　下
        //            return IsBackSetItem();
        //        }
        //    }
        //}
        bool bReturnFlg = false;
        //Debug.Log("m_cSetItemCollider : " + m_cSetItemColliderObj.name);

        ExecuteEvents.Execute<ISettingTrapCollider>(
                target: m_cSetItemColliderObj,
                eventData: null,
                functor: (recieveTarget, y) => bReturnFlg = recieveTarget.GetHitFlg());
        //Debug.Log("HitFlag : " + bReturnFlg);

        return !bReturnFlg;
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

        //// ドアに当たる
        //if (other.tag == "DoorArea")
        //{
        //    // アイテム設置不可能
        //    m_canPut = false;

        //    // 対象のドア情報を取得
        //    GDoorData = other.gameObject.transform.parent.Find("DoorScript").gameObject;
        //    //Debug.Log(other.gameObject.name);
        //    //Debug.Log(other.gameObject.transform.parent.Find("DoorScript").gameObject.name);

        //    // 状態を切り替える
        //    ChangeState(0, EHumanState.Door);
        //}

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
                if(other.gameObject.tag == "MouseGetTrap")
                {
                    m_sItemData = "MouseGetTrapManager";
                }
                //CursorManager.Instance.GetItem(other.gameObject);
                ItemHolderManager.Instance.HoldItem(other.gameObject);
                // 取得したのでオブジェクトを消す
                Destroy(other.gameObject);
                m_SEAudio.Play((int)SEAudioType.eSE_GetTrap);   // アイテム取得SE
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
                    //Destroy(other.gameObject); // トラップを削除する
                }
            }
        }

        if (LayerMask.LayerToName(other.gameObject.layer) == "Door")
        {
            ChangeState(0, EOldState);
        }

        //if (other.tag == "DoorArea")
        //{
        //    // アイテム設置可能にする
        //    m_canPut = true;

        //    // 対象のドア情報を消す
        //    GDoorData = null;

        //    // 状態を切り替える
        //    ChangeState(0, EHumanState.Normal);
        //}


    }


    // アイテム使用処理
    public void UseItem()
    {
        //Debug.Log("UseItem : " + m_sItemData);
        if(m_sItemData != null)
        {
            //if (m_canPut) {
                //Debug.Log("Put!");
                // プレハブを取得
                var item = ManagerObjectManager.Instance.GetGameObject(m_sItemData);
                //Debug.Log("ItemManager : " + item);

                //Vector2Int[] MapPos = new Vector2Int[2];
                Vector3 vector3 = Vector3.zero;

                //switch (m_eHumanDirectionalState)
                //{
                //    case EHumanDirectionalState.Forward:
                //        vector3 = this.transform.position + new Vector3(0f, 0f, 1.5f);
                //        MapPos[0].x = PlayerMapPos.x;
                //        MapPos[0].y = PlayerMapPos.y + 1;
                //        MapPos[1].x = PlayerMapPos.x;
                //        MapPos[1].y = PlayerMapPos.y + 2;
                //        break;
                //    case EHumanDirectionalState.Left:
                //        vector3 = this.transform.position + new Vector3(-1.5f, 0f, 0f);
                //        MapPos[0].x = PlayerMapPos.x - 1;
                //        MapPos[0].y = PlayerMapPos.y;
                //        MapPos[1].x = PlayerMapPos.x - 2;
                //        MapPos[1].y = PlayerMapPos.y;
                //        break;
                //    case EHumanDirectionalState.Right:
                //        vector3 = this.transform.position + new Vector3(1.5f, 0f, 0f);
                //        MapPos[0].x = PlayerMapPos.x + 1;
                //        MapPos[0].y = PlayerMapPos.y;
                //        MapPos[1].x = PlayerMapPos.x + 2;
                //        MapPos[1].y = PlayerMapPos.y;
                //        break;
                //    case EHumanDirectionalState.Back:
                //        vector3 = this.transform.position + new Vector3(0f, 0f, -1.5f);
                //        MapPos[0].x = PlayerMapPos.x;
                //        MapPos[0].y = PlayerMapPos.y - 1;
                //        MapPos[1].x = PlayerMapPos.x;
                //        MapPos[1].y = PlayerMapPos.y - 2;
                //        break;

                //}

                var collider = m_cSetItemColliderObj.GetComponent<BoxCollider>();

                vector3 = new Vector3(m_cSetItemColliderObj.transform.position.x, -0.5f, m_cSetItemColliderObj.transform.position.z);
                vector3 += this.transform.forward * collider.center.z;

                // プレハブからインスタンスを生成
                ExecuteEvents.Execute<IItemInterface>(
                    target: item,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.Instant(vector3, this.transform.rotation));
                m_SEAudio.MultiplePlay((int)SEAudioType.eSE_SetTrap);

                //MapManager.Instance.MapData[MapPos[0].y][MapPos[0].x] = (int)ObjectNo.MOUSE_TRAP_LOW;
                //MapManager.Instance.MapData[MapPos[1].y][MapPos[1].x] = (int)ObjectNo.MOUSE_TRAP_LOW;

                // インスタンスにmapの位置を登録
                //ExecuteEvents.Execute<IMouseTrap>(
                //    target: item,
                //    eventData: null,
                //    functor: (recieveTarget, y) => recieveTarget.SetMapPosition(MapPos));

                ItemHolderManager.Instance.ReleaseItem(vector3);
                // 所持アイテム情報を削除
                m_sItemData = null;
                // 無敵状態にする
                m_isInvincible = true;
            //}
        }
    }

    public bool IsMove(Vector3 movepos)
    {
        hMoveColliderScript.JudgeCollision(movepos);

        if(hMoveColliderScript.hit.distance < 0.2f)
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

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Rail")
        {
            m_GTargetBoxObject = other.gameObject.transform.parent.parent.gameObject;
            ChangeState(0, EHumanState.Rail);
        }
    }

    public bool CheckCurrentState(EHumanState _state)
    {
        if(m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)_state])
        {
            return true;
        }
        return false;
    }

    public void ChangeUpState(GameObject _Target)
    {
        m_GTargetBoxObject = _Target;
        ChangeState(0, EHumanState.Up);
    }

    public void GravityOff()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void GravityOn()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        this.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionX;
        this.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionZ;
        this.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeRotation;
    }

    public void Move(Vector3 _moveForward)
    {
        // 移動判定
        for (int i = 0; i < 3; i++)
        {
            if (IsMove(_moveForward))
            {
                //Debug.Log("moving");
                break;
            }
            else
            {
                var correctionMove = hMoveColliderScript.hit.normal;
                var a = -Vector3.Dot(_moveForward, correctionMove);
                _moveForward = _moveForward + a * correctionMove;
            }
        }
        // 移動処理
        transform.position += _moveForward * m_fmoveSpeed * Time.deltaTime;

    }


    public void PlayAnimation(EHumanAnimation anim)
    {
        m_nAnimationNo = (int)anim;
        m_cAnimation.Play(AnimationString[m_nAnimationNo]);
    }

    public Animation GetAnimation { get { return m_cAnimation; } }


    // アニメーションイベント用関数
    public void OnFootEvent()
    {
        m_SEAudio.MultiplePlay((int)SEAudioType.eSE_HumanRunning);
    }

    public void OnCatchEvent()
    {
        m_fSlowDownRate = 0f;
        if (!ReferenceEquals(hCatchZone.TargetObject, null))
        {
            ExecuteEvents.Execute<IMouseInterface>(
                target: hCatchZone.TargetObject,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Catched());
            m_SEAudio.MultiplePlay((int)SEAudioType.eSE_MouseCatching);
        }
    }

    public void OnEndCatchEvent()
    {
        ChangeState(0, EOldState);
    }

    public void PutingEvent()
    {
        UseItem();
    }

    public void OnEndPutEvent()
    {
        ChangeState(0, EOldState);
    }
}
