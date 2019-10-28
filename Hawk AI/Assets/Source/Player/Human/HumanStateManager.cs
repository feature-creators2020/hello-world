using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

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
    public ItemManager m_Itemmanager;                    // アイテム管理

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
        var ItemManagerObject = GameObject.FindWithTag("ItemManager").gameObject;
        m_Itemmanager = ItemManagerObject.GetComponent<ItemManager>();
    }

    // Update is called once per frame
    public override void Update()
    {
        var playerNo = GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // 各状態の処理
        base.Update();

        // ゲームパッドの入力情報取得
        inputHorizontal = 0f;
        inputVertical = 0f;

        inputHorizontal = keyState.LeftStickAxis.x;
        inputVertical = keyState.LeftStickAxis.y;

        // カメラの方向から、x-z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(targetCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 移動量
        Vector3 moveForward = cameraForward * inputVertical + targetCamera.transform.right * inputHorizontal;

        this.transform.position += moveForward * m_fmoveSpeed * Time.deltaTime;

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(moveForward);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter! : " + other);
        // トラップに当たる
        if (LayerMask.LayerToName(other.gameObject.layer) == "Trap")
        {
            // ネズミ捕り
            if (other.gameObject.tag == "Mousetrap")
            {
                ChangeState(0, EHumanState.SlowDown);
            }
        }

        // ドアに当たる
        if (other.tag == "DoorArea")
        {
            // アイテム設置不可能
            m_canPut = false;

            // 対象のドア情報を取得
            GDoorData = other.gameObject;

            // 状態を切り替える
            ChangeState(0, EHumanState.Door);
        }

        // アイテム取得
        if (LayerMask.LayerToName(other.gameObject.layer) == "Item")
        {
            if (m_sItemData == null)
            {
                // ネズミ捕り
                if (other.gameObject.tag == "MouseTrapItem")
                {
                    m_sItemData = "MouseTrap";
                    Debug.Log("GetItem");
                }

                // 取得したのでオブジェクトを消す
                Destroy(other.gameObject);
            }
            Debug.Log("Item : " + m_sItemData);
        }
    }

    void OnTriggerExit(Collider other)
    {
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
                // プレハブを取得
                //var item = m_Itemmanager;
                //GameObject prefab;
                // プレハブからインスタンスを生成
                //Instantiate(prefab, this.transform.position, Quaternion.identity);
                // 所持アイテム情報を削除
                m_sItemData = null;
            }
        }
    }
}
