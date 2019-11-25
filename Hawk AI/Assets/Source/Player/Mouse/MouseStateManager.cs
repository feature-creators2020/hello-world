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
    ForcedWait
}

public interface IMouseInterface : IEventSystemHandler
{
    void Catched();

    void ChangeUpState(GameObject _Target);
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

    public List<GameObject> m_cPipeTransPosObj = new List<GameObject>();

    [System.NonSerialized]
    public MoveCollider hMoveColliderScript;    // 移動判定用スクリプト

    public float m_fPipeSpeed;                  // 計算時の速
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

        m_cStateList.Add(Normal);
        m_cStateList.Add(SlowDown);
        m_cStateList.Add(Door);
        m_cStateList.Add(Up);
        m_cStateList.Add(Pipe);
        m_cStateList.Add(Catch);
        m_cStateList.Add(Rail);
        m_cStateList.Add(ForcedWait);

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EMouseState.ForcedWait]);
        EOldState = EMouseState.ForcedWait;
    }

    // Update is called once per frame
    public override void Update()
    {
        //var playerNo = GamePadIndex;
        //var keyState = GamePad.GetState(playerNo, false);
        //var playerKeyNo = (KeyBoard.Index)playerNo;
        //var keyboardState = KeyBoard.GetState(KeyboardIndex, false);

        hMoveColliderScript = this.gameObject.GetComponent<MoveCollider>();


        // 各状態の処理
        base.Update();

        if(m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)EMouseState.ForcedWait])
        {
            return;
        }

        // レイキャストによる壁の当たり判定処理
        if (m_cStateMachineList[0].GetCurrentState() != m_cStateList[(int)EMouseState.Up])
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
                        if (LayerName == "Rail")
                        {
                            m_GTargetBoxObject = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
                        }
                        else
                        {
                            m_GTargetBoxObject = hit.collider.gameObject;
                        }
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
            if (Physics.Raycast(Downray, out Downhit, 1f))
            {
                //Debug.Log("DownRootObject : " + Downhit.collider.gameObject.transform.parent.gameObject.transform.gameObject.name);
                Debug.Log("DownHumanRayHit : " + Downhit.collider.gameObject.name);
                Debug.Log("DownHitTag : " + Downhit.collider.tag);

                var LayerName = LayerMask.LayerToName(Downhit.collider.gameObject.layer);
                var TagName = Downhit.collider.gameObject.tag;
                if (LayerName == "Rail")
                {
                    if (TagName == "Rail")
                    {
                        m_GTargetBoxObject = Downhit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
                        ChangeState(0, EMouseState.Rail);
                    }
                }
                else
                {
                    if (CheckCurrentState(EMouseState.Rail))
                    {
                        ChangeState(0, EOldState);
                    }
                }
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        var LayerName = LayerMask.LayerToName(other.gameObject.layer);
        var TagName = other.gameObject.tag;

        // トラップに当たる
        if (LayerName == "Trap")
        {
            // ネズミ捕り
            if (TagName == "Mousetrap")
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
            ScoreBoard.Instance.GetCheese();
            RespawnPoint.Instance.Respawn(this.gameObject);
            ShiftOtherGoal.Instance.Shift(other.gameObject);
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
            if (TagName == "Mousetrap")
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
                Destroy(other.gameObject); // トラップを削除する
            }
            
        }

        if (LayerName == "Gap")
        {
            ChangeState(0, EOldState);
        }

        //if (LayerName == "Pipe")
        //{
        //    ChangeState(0, EOldState);
        //}

        if (LayerName == "Goal")
        {
            ChangeState(0, EMouseState.Normal);      
        }

    }

    public virtual void Catched()
    {
        if (m_cStateMachineList[0].GetCurrentState() != m_cStateList[(int)EMouseState.Pipe])
        {
            //Debug.Log("Catched!");
            ChangeState(0, EMouseState.Catch);
        }
    }


    public bool IsMove(Vector3 movepos)
    {
        hMoveColliderScript.JudgeCollision();

        if (hMoveColliderScript.hit.distance <= 0.25f)
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

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Rail")
        {
            m_GTargetBoxObject = other.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            ChangeState(0, EMouseState.Rail);
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

}
