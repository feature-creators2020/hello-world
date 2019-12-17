using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.EventSystems;
using KeyBoardInput;

public enum EMouseState
{
    Normal,
    SlowDown,
    Door,
    Up,
    Pipe,
    Catch,
    Rail,
    ForcedWait,
    GetCheese,
    VarsanDown
}

public enum EMouseAnimation
{
    Wait,
    Run,
    Slow,
    Eat,
    VarsanDown_Start,
    VarsanDown_Wait
}

public interface IMouseInterface : IEventSystemHandler
{
    void Catched(GameObject _Owner);

    void ChangeUpState(GameObject _Target);

    void SetCollapse();

    void SetDefaultSize();

    int GetRoomID();

    void SetRoomID(int _id);

    void StartVarsan();

    bool GetVarsan();

    void StopVarsan();

    void EndVarsan();


}

public class MouseStateManager : CStateObjectBase<MouseStateManager, EMouseState>, IMouseInterface
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
    public GameObject m_GTargetBoxObject;       // 上る段ボールのオブジェクト
    [System.NonSerialized]
    public Vector3 m_TargetBoxNomal;            // 上る段の面の法線ベクトル

    public List<GameObject> m_cPipeTransPosObj = new List<GameObject>();

    [System.NonSerialized]
    public MoveCollider hMoveColliderScript;    // 移動判定用スクリプト

    public float m_fPipeSpeed;                  // 計算時の速

    [System.NonSerialized]
    public bool m_bIsNight = false;             // 夜状態か
    [System.NonSerialized]
    public float m_fNightSpeedRate = 1.5f;      // 夜状態の移動速度倍率
    [System.NonSerialized]
    public TimeZoneManager m_TimeZoneManager;           // 昼夜の状態を取得する

    public Rigidbody m_rb;                      // rigidbodyで移動する

    public SEAudio m_SEAudio;           // se


    [System.NonSerialized]
    public string[] AnimationString = { "Mouse4_Wait", "Mouse4_Run", "Mouse4_Slow", "Mouse4_Eat" };          // アニメーション名
    private int m_nAnimationNo;                                      // 再生中アニメーション番号
    private Animation m_cAnimation;                                  // アニメーション      


    public Vector3 m_vDefaultScale;                 // 元の大きさを保持
    public GameObject m_GRailObject;                // ベルトコンベアの情報
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
    public EMouseState EOldState;        // 前の状態を保持

    public bool m_isOnRail;                // ベルトコンベアの上にいるか

    [SerializeField]
    private int m_nRoomID;                      // 現在いるルームの情報

    private bool m_isVarsan;                    // バルサンを受けているの状態

    private float m_fVarsanTimeCount;           // バルサンを受けている状態にカウントをする

    [SerializeField]
    private GameObject m_gVarsanEffect;         // バルサンのエフェクト用

    // Start is called before the first frame update
    void Start()
    {
        var StateMachine = new CStateMachine<MouseStateManager>();
        m_cStateMachineList.Add(StateMachine);

        var Normal = new MNormalManager(this);
        var SlowDown = new MSlowDownManager(this);
        var Door = new MDoorManager(this);
        var Up = new MUpManager(this);
        var Pipe = new MPipeMoveManager(this);
        var Catch = new MCatchManager(this);
        var Rail = new MRailManager(this);
        var ForcedWait = new MForcedWaitManager(this);
        var GetCheese = new MGetCheeseManager(this);
        var VarsanDown = new MVarsanDownManager(this);

        m_cStateList.Add(Normal);
        m_cStateList.Add(SlowDown);
        m_cStateList.Add(Door);
        m_cStateList.Add(Up);
        m_cStateList.Add(Pipe);
        m_cStateList.Add(Catch);
        m_cStateList.Add(Rail);
        m_cStateList.Add(ForcedWait);
        m_cStateList.Add(GetCheese);
        m_cStateList.Add(VarsanDown);

        m_cAnimation = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animation>();

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EMouseState.ForcedWait]);
        EOldState = EMouseState.ForcedWait;
        m_vDefaultScale = this.transform.localScale;
        m_isOnRail = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        // デバッグ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartVarsan();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StopVarsan();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EndVarsan();
        }


        // マネージャー取得
        var managerobject = ManagerObjectManager.Instance;
        m_TimeZoneManager = managerobject.GetGameObject("TimeZoneManager").GetComponent<TimeZoneManager>();
        // se取得
        m_SEAudio = managerobject.GetGameObject("SEAudio").GetComponent<SEAudio>();


        // 移動判定用コライダー取得
        hMoveColliderScript = this.gameObject.GetComponent<MoveCollider>();
        // 移動用rigidbody
        m_rb = this.gameObject.GetComponent<Rigidbody>();

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
        //Debug.Log("isNight : " + m_bIsNight);
        // 速度設定
        SetMoveSpeed(m_bIsNight);

        // 各状態の処理
        base.Update();

        if(m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)EMouseState.ForcedWait])
        {
            return;
        }

        // レイキャストによる壁の当たり判定処理
        if ((!CheckCurrentState(EMouseState.Up)) && (!CheckCurrentState(EMouseState.GetCheese)))
        {
            Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 0.5f, Color.red);
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.5f))
            {
                var LayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                var TagName = hit.collider.gameObject.tag;
                if (LayerName == "Box" || LayerName == "Rail")
                {
                    if (TagName == "CanClimbing")
                    {
                        //Debug.Log(hit.collider.gameObject.transform.position);
                        //if (LayerName == "Rail")
                        //{
                        //    m_GTargetBoxObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
                        //}
                        //else
                        //{
                            m_GTargetBoxObject = hit.collider.gameObject;
                        //}
                        m_TargetBoxNomal = hit.normal;
                        if (m_cStateMachineList[0].GetCurrentState() != m_cStateList[(int)EMouseState.Pipe])
                        {
                            ChangeState(0, EMouseState.Up);
                        }
                        return;
                    }
                }
            }

            Ray Downray = new Ray(transform.position, -transform.up);
            RaycastHit Downhit;
            Debug.DrawLine(transform.position, transform.position - transform.up, Color.red);
            if (Physics.BoxCast(transform.position, transform.lossyScale * 0.5f, -transform.up, out Downhit))
            {
                //Debug.Log("DownRootObject : " + Downhit.collider.gameObject.transform.parent.gameObject.transform.gameObject.name);
                //Debug.Log("DownHumanRayHit : " + Downhit.collider.gameObject.name);
                //Debug.Log("DownHitTag : " + Downhit.collider.tag);

                var LayerName = LayerMask.LayerToName(Downhit.collider.gameObject.layer);
                var TagName = Downhit.collider.gameObject.tag;
                if (LayerName == "Rail")
                {
                    if (TagName == "Rail")
                    {
                        m_GTargetBoxObject = Downhit.collider.gameObject.transform.parent.parent.gameObject;
                        ChangeState(0, EMouseState.Rail);
                    }
                }
                else
                {
                    if (CheckCurrentState(EMouseState.Rail))
                    {
                        ChangeState(0, EMouseState.Normal);
                    }
                }
            }

        }
        if (m_isOnRail)
        {
            ChangeState(0, EMouseState.Rail);
            return;
        }

        VarsanUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
        var LayerName = LayerMask.LayerToName(other.gameObject.layer);
        var TagName = other.gameObject.tag;

        // トラップに当たる
        if (LayerName == "Trap")
        {
            // ネズミ捕り
            if ((TagName == "Mousetrap") || (TagName == "MouseGetTrap"))
            {
                ChangeState(0, EMouseState.SlowDown);
            }
        }

        // 隙間
        if (LayerName == "Gap")
        {
            ChangeState(0, EMouseState.Door);
        }

        // ゴール地点
        if (LayerName == "Goal")
        {
            m_GTargetBoxObject = other.gameObject;
            if (!CheckCurrentState(EMouseState.GetCheese))
            {
                ChangeState(0, EMouseState.GetCheese);
            }
        }

        if (LayerName == "Pipe")
        {

            if (m_cStateMachineList[0].GetCurrentState() != m_cStateList[(int)EMouseState.Pipe])
            {
                if (other.gameObject.name == "Collision_1")
                {

                    ExecuteEvents.Execute<IPipeInterfase>(
                    target: other.gameObject.transform.parent.parent.gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => m_cPipeTransPosObj = recieveTarget.GetPipeObjects);

                }
                else if (other.gameObject.name == "Collision_2")
                {

                    ExecuteEvents.Execute<IPipeInterfase>(
                    target: other.gameObject.transform.parent.parent.gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => m_cPipeTransPosObj = recieveTarget.GetInversePipeObjects);

                }
            }

            ChangeState(0, EMouseState.Pipe);
        }

    }

    void OnTriggerStay(Collider other)
    {
        var LayerName = LayerMask.LayerToName(other.gameObject.layer);
        var TagName = other.gameObject.tag;

        if (LayerName == "Trap")
        {
            // ネズミ捕り
            if ((TagName == "Mousetrap") || (TagName == "MouseGetTrap"))
            {
                m_fSlowTime = m_fLimitSlowTime; // 無敵状態を解除する
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        var LayerName = LayerMask.LayerToName(other.gameObject.layer);
        var TagName = other.gameObject.tag;

        if (LayerName == "Trap")
        {
            // ネズミ捕り
            if (TagName == "Mousetrap")
            {
                //Destroy(other.gameObject); // トラップを削除する
                ChangeState(0, EOldState);
            }
            SetDefaultSize();
        }

        if (LayerName == "Gap")
        {
            ChangeState(0, EOldState);
        }

        //if (LayerName == "Pipe")
        //{
        //    ChangeState(0, EOldState);
        //}

        //if (LayerName == "Goal")
        //{

        //    ChangeState(0, EMouseState.Normal);      
        //}

    }

    public virtual void Catched(GameObject _Owner)
    {
        if (m_cStateMachineList[0].GetCurrentState() != m_cStateList[(int)EMouseState.Pipe])
        {
            //Debug.Log("Catched!");

            // Hack : PlayerManager実装
            var PlayerManager = ManagerObjectManager.Instance.GetGameObject("PlayerManager").
                GetComponent<PlayerManager>();
            var HumanList = PlayerManager.GetGameObjectsList("Human");

            GameObject Player = new GameObject();

            for (int i = 0; i < HumanList.Count; i++)
            {
                if (_Owner == PlayerManager.GetGameObject(i, "Human"))
                {
                    if (i == 0)
                    {
                        GameManager.KillCountByHuman1++;
                    }
                    else
                    {
                        GameManager.KillCountByHuman2++;
                    }
                    break;
                }
            }
            ChangeState(0, EMouseState.Catch);
        }
    }


    public bool IsMove(Vector3 movepos)
    {
        hMoveColliderScript.JudgeCollision(movepos);

        if (hMoveColliderScript.hit.distance < 0.2f)
        {
            return false;
        }

        return true;
    }


    public CStateBase<MouseStateManager> GetCurrentState()
    {
        return m_cStateMachineList[0].GetCurrentState();
    }

    public CStateBase<MouseStateManager> GetStateBase(EMouseState _state)
    {
        return m_cStateList[(int)_state];
    }

    void OnCollisionEnter(Collision other)
    {
        // 壁ずり処理
        foreach (var val in other.contacts)
        {
            if (val.otherCollider.gameObject == this.gameObject)
            {
                var a = -Vector3.Dot(m_rb.velocity, val.normal);
                m_rb.velocity = m_rb.velocity + a * val.normal;
            }
        }
        if(other.gameObject.tag == "Rail")
        {
            m_isOnRail = true;
            m_GRailObject = other.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Rail")
        {
            m_GRailObject = other.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            ChangeState(0, EMouseState.Rail);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Rail")
        {
            m_isOnRail = false;
        }
    }

    public bool CheckCurrentState(EMouseState _state)
    {
        if (m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)_state])
        {
            return true;
        }
        return false;
    }

    public void ChangeUpState(GameObject _Target)
    {
        m_GTargetBoxObject = _Target;
        ChangeState(0, EMouseState.Up);
    }

    public void GravityOff()
    {
        m_rb.useGravity = false;
        m_rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void GravityOn()
    {
        m_rb.useGravity = true;
        m_rb.constraints = RigidbodyConstraints.None;
        m_rb.constraints |= RigidbodyConstraints.FreezePositionX;
        m_rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        m_rb.constraints |= RigidbodyConstraints.FreezeRotation;
    }

    public void SetMoveSpeed(bool isnight)
    {
        if (isnight)
        {
            // 夜状態。速度倍率を掛ける
            m_fmoveSpeed = m_fDefaultSpeed * m_fNightSpeedRate;
        }
        else
        {
            // 昼状態。
            m_fmoveSpeed = m_fDefaultSpeed;
        }
        //Debug.Log("MouseSpeed : " + m_fmoveSpeed);
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

    public void PlayAnimation(EMouseAnimation anim)
    {
        m_nAnimationNo = (int)anim;
        m_cAnimation.Play(AnimationString[m_nAnimationNo]);
    }

    public Animation GetAnimation { get { return m_cAnimation; } }

    public void SetCollapse()
    {
        //m_vDefaultScale = this.transform.localScale;
        this.transform.localScale = new Vector3(m_vDefaultScale.x, 0.1f, m_vDefaultScale.z);
    }

    public void SetDefaultSize()
    {
        this.transform.localScale = m_vDefaultScale;
    }

    public int GetRoomID()
    {
        return m_nRoomID;
    }

    public void SetRoomID(int _id)
    {
        m_nRoomID = _id;
    }

    public void VarsanUpdate()
    {
        // バルサンの処理
        if (m_isVarsan)
        {
            m_fVarsanTimeCount += Time.deltaTime;
            // 一定時間バルサンを受けていると、気絶する状態になる
            if (m_fVarsanTimeCount >= 5.0f)
            {
                // 気絶状態の時は処理しない
                if (true)
                {
                    // ステートを変える
                    ChangeState(0, EMouseState.VarsanDown);
                }
            }
        }
        else
        {
            m_fVarsanTimeCount = 0f;
        }
    }

    public void StartVarsan()
    {
        if (!m_isVarsan)
        {
            m_isVarsan = true;
            // バルサンの状態になるので、エフェクトを再生させる
            ExecuteEvents.Execute<IValsanEffect>(
                    target: m_gVarsanEffect,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.Play((int)GamePadIndex));
            Debug.Log("MouseVarsan" + (int)GamePadIndex);
            //m_SEAudio.MultiplePlay((int)SEAudioType.eSE_MouseCatching);
        }
    }

    public bool GetVarsan()
    {
        return m_isVarsan;
    }

    public void StopVarsan()
    {
        // バルサンのエフェクトを一時停止する
        if (m_isVarsan)
        {
            m_isVarsan = false;
            ExecuteEvents.Execute<IValsanEffect>(
                target: m_gVarsanEffect,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Stop((int)GamePadIndex));

        }
    }

    public void EndVarsan()
    {
        m_isVarsan = false;
        m_fVarsanTimeCount = 0f;
        // エフェクトも止める
        ExecuteEvents.Execute<IValsanEffect>(
            target: m_gVarsanEffect,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.End());
        ChangeState(0, EMouseState.Normal);
        //m_SEAudio.MultiplePlay((int)SEAudioType.eSE_MouseCatching);
    }


    // アニメーションイベント用関数
    public void MouseRunEvent()
    {
        m_SEAudio.MultiplePlay((int)SEAudioType.eSE_MouseRunning);
    }

    public void OnEatEvent()
    {
        ExecuteEvents.Execute<ICheeseInterfase>(
                    target: m_GTargetBoxObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.StartScaling(4f));
        
    }
}
