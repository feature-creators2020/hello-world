﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.EventSystems;
public enum EHumanState
{
    Normal,
    SlowDown,
    Door,
}

public class HumanStateManager : CStateObjectBase<HumanStateManager, EHumanState>
{
    [System.NonSerialized]
    public float inputHorizontal;               // コントローラーLスティック横軸情報
    [System.NonSerialized]
    public float inputVertical;                 // コントローラーLスティック縦軸情報
    public Camera targetCamera;                 // 対象のカメラ
    public GamePad.Index GamePadIndex;          // 対象のコントローラー

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

    [System.NonSerialized]
    public EHumanState EOldState;        // 前の状態を保持

    Transform char_trans;
    public LayerMask layermask;
    Vector3 char_velocity;
    Vector3 char_velocity_input;
    float RayLength;

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

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter! : " + other);
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
            Debug.Log(other.gameObject.name);
            Debug.Log(other.gameObject.transform.parent.Find("DoorScript").gameObject.name);

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
                Debug.Log(ItemManagerObject.name);
                m_Itemmanager = ItemManagerObject.GetComponent<ItemManager>();
                Debug.Log(m_Itemmanager.name);

                // ネズミ捕り
                if (other.gameObject.tag == "Mousetrap")
                {
                    m_sItemData = "MousetrapManager";
                    Debug.Log("GetItem");
                }

                // 取得したのでオブジェクトを消す
                Destroy(other.gameObject);
            }
            Debug.Log("Item : " + m_sItemData);
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
        Debug.Log("UseItem : " + m_sItemData);
        if(m_sItemData != null)
        {
            if (m_canPut) {
                Debug.Log("Put!");
                // プレハブを取得
                var item = m_Itemmanager.GetGameObject(m_sItemData);
                Debug.Log("ItemManager : " + item);
                // プレハブからインスタンスを生成
                ExecuteEvents.Execute<IItemInterface>(
                    target: item,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.Instant(this.transform));

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
}
