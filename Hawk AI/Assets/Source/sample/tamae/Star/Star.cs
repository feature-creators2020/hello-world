using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum StarState
{
    Wait,
    Walk,
    Jump,
    Falling,
    Landimg,
    PutCone,
    CollectCone,
    TelBox,
    EnterBuilding
};

public enum StarAnimation
{
    Wait,
    Walk,
    JumpUp,
    JumpDuration,
    Landing,
    PutCone,
    Fall,
}

public enum EffectType
{
    JUMP,
    LANDING
}


public interface IStarInterface : IEventSystemHandler
{
    void OnTelboxIn(GameObject telbox);
    void OnTelboxOut();
    void OnRemoveObject(GameObject obj);
    void OnMoveObject(Vector3 vec);
    void AddVelocity(Vector3 vec);
    void OnElevatorTakeOn();
    void OnElevatorGetOff();
}

public class Star : CStateObjectBase<Star, StarState>, IStarInterface
{
    // アニメーション名
    private string[] AnimationString ={
    "StarWait0526",
    "StarWalk0526",
    "StarJumpUp0526",
    "StarJumpDuration0526",
    "StarLanding0526",
    "StarPutCone0526",
    "StarFall0526",
    };

    public bool StarOrOrdinaly = true;                  // star状態かodinary状態か star:true/ordinaly:false
    private bool m_bRightorLeft = true;                 // 右向き左向き   right:true/left:false
    private bool m_bTelboxOn = false;                   // 電話ボックスに入れるか
    private bool m_bElevatorOn = true;                  // エレベーターに乗っているか
    private bool m_bTelboxIn = false;                   // 電話ボックスに入っているか
    private bool m_bPylonFlag = false;                  // 初期は消去不要
    private bool m_bAnimationFlag = true;              // アニメーション再生フラグ
    private bool m_bIsMovedFlag = false;                // 動くブロックの重なりをふさぐフラグ
    private bool m_bVerticalInitFlag = false;           // 縦配列初期化用
    private bool m_bHorizontalInitFlag = false;         // 横配列初期化用
    private bool m_bIsHavingCone = false;               // コーンを持っているか否か
    private bool m_bArrayUpdateFlag = true;             // 配列アップデートをするか
    private bool m_bGameOver = false;                   // ゲームオーバーフラグ

    private int m_nAnimationNo = 0;                     // 再生中アニメーション番号
    //private int m_nPylonNum = 0;                        // 現在の所持コーン数
    private int m_nPylonNum = 0;
    private int m_nIndexVertical = 0;                   // 縦の配列番号
    private int m_nIndexHorizontal = 0;                 // 横の配列番号
    private int m_nInitVertical = 0;                    // 初期の縦配列番号
    private int m_nInitHorizontal = 0;                  // 初期の横配列番号

    private const int m_nisGroundStateChange = 5;       // 地面にいるという判定の許容秒数
    private int m_nHitLayer;                            // 当たるレイヤーの入れ子
    private float m_fgroundDistance = 0f;               // 地面までの距離入れ子
    private float m_fRayOffsetHeight = 0.005f;
    private float m_fRayHeightCorrection = 0f;          // レイの高さの補正値
    private float m_fGroundDistanceLimit = 0.05f;       // 地面までの距離の判定許容距離

    private Vector3 m_vBoxOffset = new Vector3(0.0f, 0.005f, 0.0f);
    private Vector3 m_vBoxCorrection = Vector3.zero;
    private Vector3 m_vInitPosition = Vector3.zero;     // 初期ポジション

    private const float m_fRaycastSearchDistance = 100.0f;                  // Rayの距離

    private Animation[] animation = new Animation[2];   // アニメーション      
    private AnimationState m_cAnimState;                // アニメーション管理
    private Rigidbody rigidbody;                        // リジッドボディー
    private CapsuleCollider Capcol;                     // ボックスコライダー取得
    private BoxCollider Boxcol;

    MapManager map;                                     // マップマネージャーのインスタンス入れ子

    private GameObject StarPlayer;                      // 子オブジェクトのスターを取得
    private GameObject StaffPlayer;                     // 子オブジェクトの清掃員を取得
    private GameObject StarUI;                          // 子オブジェクトのUIを取得
    //private GameObject Aura;

    private GameObject m_cTelbox;                       // 電話ボックス
    private GameObject m_cCone;                         // コーン情報入れコーン

    public GameObject[] Effect;                         // スターの扱うエフェクトを選択

    [SerializeField] private float MoveAmount = 1.0f;   // 移動量
    [SerializeField] private float JumpTime = 1.0f;     // ジャンプ時間(秒)
    [SerializeField] private float JumpHeight = 3.0f;   // starのジャンプの高さ(ブロック)
    [SerializeField] private float FallSpeed = 1.0f;    // starのジャンプの高さ(ブロック)
    //[SerializeField] private int GameOverTime = 10;     // ゲームオーバーの秒数    

    private List<Collision> m_lCollisionList = new List<Collision>();  // 当たったオブジェクトを保持

    //public virtual void Start()
    //{
    //    StarStart();
    //}

    public void StarStart()
    {
        var Wait = new StarWait(this);
        var Move = new StarWalk(this);
        var Jump = new StarJump(this);
        var Fall = new StarFalling(this);
        var Landing = new StarLanding(this);
        var PutCone = new StarPutCone(this);
        var CollectCone = new StarCollectCone(this);
        var TelBox = new StarTelBox(this);
        var EnterBuilding = new StarEnterBuilding(this);

        m_cStateList.Add(Wait);
        m_cStateList.Add(Move);
        m_cStateList.Add(Jump);
        m_cStateList.Add(Fall);
        m_cStateList.Add(Landing);
        m_cStateList.Add(PutCone);
        m_cStateList.Add(CollectCone);
        m_cStateList.Add(TelBox);
        m_cStateList.Add(EnterBuilding);

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)StarState.Wait]);

        FadeStarAnimation(StarAnimation.Wait);
    }

    public void StarUpdate()
    {
        if (!m_bGameOver && !GoalScript.GoalState)
        {
            if (global::PauseManager.IsPause == false)
            {//WARNING : IsPauseがfalseの時にポーズ解除
                base.StateUpdate();
            }
        }
        else
        {
            AddVelocity(Vector3.zero);
            StarUI.SetActive(false);
        }

        m_bIsMovedFlag = false;

        if (m_bArrayUpdateFlag)
        {
            IndexUpdate();
        }
    }

    // Update is called once per frame
    //public override void Update()
    //{
    //    StarUpdate();

    //    //Debug.Log("State:" + m_cStateMachineList[0].GetCurrentState());
    //}

    // 初期化処理
    public void Initialize()
    {
        StarPlayer = transform.GetChild(0).gameObject;
        StaffPlayer = transform.GetChild(1).gameObject;
        StarUI = transform.GetChild(2).gameObject;

        m_bArrayUpdateFlag = true;
        m_bGameOver = false;
        m_nPylonNum = 0;

        animation[0] = StarPlayer.GetComponent<Animation>();
        animation[1] = StaffPlayer.GetComponent<Animation>();
        rigidbody = this.GetComponent<Rigidbody>();
        Capcol = this.GetComponent<CapsuleCollider>();

        map = MapManager.Instance;

        m_vInitPosition = transform.position;
        m_fGroundDistanceLimit = m_fGroundDistanceLimit - (Capcol.center.y / 2);
        m_vBoxCorrection = new Vector3(Capcol.radius - 0.001f, Capcol.height * 0.5f, Capcol.radius - 0.001f);

        var StateMachine = new CStateMachine<Star>();
        m_cStateMachineList.Add(StateMachine);

        if (!StarOrOrdinaly)
        {
            StarPlayer.SetActive(false);
        }
        else
        {
            StaffPlayer.SetActive(false);
        }
    }

    // 速度代入
    public void AddVelocity(Vector3 vec)
    {
        rigidbody.velocity = vec;
    }

    // コーンを置けますか？
    public bool IsPutCone()
    {
        // カラーコーンを持っている
        if (m_nPylonNum != 0)
        {
            if (map.FrontMapData[m_nIndexVertical][m_nIndexHorizontal] == 0 && map.FrontMapData[m_nIndexVertical + 1][m_nIndexHorizontal] != 0 && map.BackMapData[m_nIndexVertical][m_nIndexHorizontal] == 0)
            {
                Vector3 Direction = Vector3.zero;
                bool flag = false;
                int direction = 0;

                // もう一マス先のオブジェクトが何をしようとしているかを判別
                if (map.FrontMapData[m_nIndexVertical][m_nIndexHorizontal + 1] == (int)ObjectNo.Fan)
                {
                    var obj = Reboot.RebootFanManager.Instance.GetRebootStandingFan(m_nIndexVertical, m_nIndexHorizontal + 1);
                    Direction = obj.Direction;
                    direction = 1;
                    flag = obj.IsMoving;
                }
                else if (map.FrontMapData[m_nIndexVertical][m_nIndexHorizontal - 1] == (int)ObjectNo.Fan)
                {
                    var obj = Reboot.RebootFanManager.Instance.GetRebootStandingFan(m_nIndexVertical, m_nIndexHorizontal + 1);
                    Direction = obj.Direction;
                    direction = -1;
                    flag = obj.IsMoving;
                }
                else if (map.FrontMapData[m_nIndexVertical][m_nIndexHorizontal + 1] == (int)ObjectNo.Paparazzi)
                {
                    var obj = Reboot.RebootFanManager.Instance.GetRebootLoiteringFan(m_nIndexVertical, m_nIndexHorizontal + 1);
                    Direction = obj.Direction;
                    direction = 1;
                    flag = obj.IsMoving;
                }
                else if (map.FrontMapData[m_nIndexVertical][m_nIndexHorizontal - 1] == (int)ObjectNo.Paparazzi)
                {
                    var obj = Reboot.RebootFanManager.Instance.GetRebootLoiteringFan(m_nIndexVertical, m_nIndexHorizontal - 1);
                    Direction = obj.Direction;
                    direction = -1;
                    flag = obj.IsMoving;
                }
                else if (map.WoodenMapData[m_nIndexVertical][m_nIndexHorizontal + 1] == (int)ObjectNo.WoodenBox)
                {
                    var obj = WoodenManager.Instance.GetWoodenBlockIndex(m_nIndexHorizontal + 1, m_nIndexVertical);
                    Direction = obj.Direction;
                    direction = 1;
                    flag = obj.Slave;
                }
                else if (map.WoodenMapData[m_nIndexVertical][m_nIndexHorizontal - 1] == (int)ObjectNo.WoodenBox)
                {
                    var obj = WoodenManager.Instance.GetWoodenBlockIndex(m_nIndexHorizontal - 1, m_nIndexVertical);
                    Direction = obj.Direction;
                    direction = -1;
                    flag = obj.Slave;
                }

                if (flag)
                {
                    if (m_nIndexHorizontal + (int)Direction.x != m_nIndexHorizontal + direction)
                    {
                        return false;
                    }
                }

                m_nPylonNum--;
                return true;
            }
        }
        return false;
    }

    // コーンを回収できますか？
    public bool IsCollectCone()
    {
        if (m_nPylonNum == 0 && MapManager.Instance.FrontMapData[Vertical][Horizontal] == (int)ObjectNo.ColorCone)
        {
            float offset = 0.0f;
            if (RightLeft)
            {
                offset = -0.25f;
            }
            else
            {
                offset = 0.25f;
            }

            Vector3 vec = transform.position;
            vec.x = vec.x + offset;

            RaycastHit hit;
            int Layer = LayerMask.GetMask(new string[] { "Color Cone" });
            var IsRayHit = Physics.Raycast(vec, transform.TransformDirection(Vector3.forward), out hit, 0.6f, Layer);
            if (IsRayHit)
            {
                MapManager.Instance.FrontMapData[Vertical][Horizontal] = 0;
                m_cCone = hit.collider.gameObject;
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    // コーンオブジェクトを削除
    public void CollectCone()
    {
        Destroy(m_cCone);
        m_nPylonNum++;
    }

    // 地面と当たっているか
    public bool CheckGroundDintance()
    {

        RaycastHit hit;

        m_nHitLayer = LayerMask.GetMask(new string[] { "Stage", "MoveBlock" });

        var isRayHit = Physics.BoxCast(transform.position + m_vBoxOffset, /*Boxcol.size * 0.5f*/m_vBoxCorrection, Vector3.Normalize(transform.TransformDirection(Vector3.down)), out hit, Quaternion.identity, m_fGroundDistanceLimit, m_nHitLayer);

        if (isRayHit)
        {

            m_fgroundDistance = hit.distance;
            //Debug.Log("Object:" + hit.collider.gameObject);
            //Debug.Log("Distance:" + hit.distance);
        }
        else
        {
            m_fgroundDistance = float.MaxValue;
        }

        if (m_fgroundDistance < m_fGroundDistanceLimit)
        {
            //{//ヒットしていなければ処理を実行しない
            //TODO: 慣性を働かせる場合、条件を追加
            //Debug.Log("Destance:" + m_fgroundDistance);
            //Debug.Log("Object:" + hit[(int)rayType].collider.gameObject.name);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MoveBlock"))
            {
                ExecuteEvents.Execute<IMoveBlockInterface>(
                target: hit.collider.gameObject,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.OnEnterMoveBlock());
                //Debug.Log("Move");
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    // 左右に向かせます
    public void Direction()
    {
        if (m_bRightorLeft)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    // 当たったらコリジョンリストに追加
    private void OnCollisionEnter(Collision col)
    {
        m_lCollisionList.Add(col);
    }

    private void OnTriggerEnter(Collider col)
    {
        // ファンのコライダーに当たるとゲームオーバー
        if (col.gameObject.layer == LayerMask.NameToLayer("Fan") && StarOrOrdinaly)
        {
            m_bGameOver = true;
        }
    }

    private void OnCollisionStay(Collision col)
    {
    }

    // 離れたらコリジョンリストから削除
    private void OnCollisionExit(Collision col)
    {
        m_lCollisionList.Remove(col);
        if (col.gameObject.layer == LayerMask.NameToLayer("MoveBlock"))
        {
            ExecuteEvents.Execute<IMoveBlockInterface>(
            target: col.gameObject,
             eventData: null,
             functor: (recieveTarget, y) => recieveTarget.OnExitMoveBlock());
        }
    }

    // 配列の更新　プレイヤー用
    private void IndexUpdate()
    {
        float vertical = transform.position.y - m_vInitPosition.y;
        float horizontal = transform.position.x - m_vInitPosition.x;
        int x, y;

        x = y = 0;

        // 0～0.5までの場合補正しない
        if (vertical > 0.5f)
        {
            y = -((int)((vertical + 0.5f) * 10) / 10);
        }
        if (vertical < -0.5f)
        {
            y = -((int)((vertical - 0.5f) * 10) / 10);
        }

        if (horizontal > 0.5f)
        {
            x = (int)((horizontal + 0.5f) * 10) / 10;
        }
        if (horizontal < 0.5f)
        {
            x = (int)((horizontal - 0.5f) * 10) / 10;
        }

        map.PlayerMapData[m_nIndexVertical][m_nIndexHorizontal] = 0;

        m_nIndexVertical = m_nInitVertical + y;
        m_nIndexHorizontal = m_nInitHorizontal + x;

        map.PlayerMapData[m_nIndexVertical][m_nIndexHorizontal] = 1;
    }

    // スターのアクティブを設定
    public void StarActive(bool active)
    {
        StarPlayer.SetActive(active);
    }

    // 清掃員のアクティブを設定
    public void StaffActive(bool active)
    {
        StaffPlayer.SetActive(active);
    }

    // 電話ボックス格納用
    public GameObject TelBox
    {
        get
        {
            return m_cTelbox;
        }
    }

    // カラーコーンゲッター
    public GameObject ColorCone
    {
        get
        {
            return m_cCone;
        }
    }

    // カラーコーンの数ゲッター
    public int PylonNum
    {
        get
        {
            return m_nPylonNum;
        }
    }

    // 配列のアップデートを止める
    public void StarArrayStop()
    {
        m_bArrayUpdateFlag = false;
        map.PlayerMapData[Vertical][Horizontal] = 0;
    }

    // 配列の更新を再開
    public void StarArrayPlay()
    {
        m_bArrayUpdateFlag = true;
    }

    // 電話ボックスに入れます
    public void OnTelboxIn(GameObject telbox)
    {
        m_bTelboxOn = true;
        m_cTelbox = telbox;
    }

    // 電話ボックスに入れません
    public void OnTelboxOut()
    {
        m_bTelboxOn = false;
    }

    // エレベーターに乗っています
    public void OnElevatorTakeOn()
    {
        m_bElevatorOn = true;
    }

    // エレベーターに乗っていません
    public void OnElevatorGetOff()
    {
        m_bElevatorOn = false;
    }

    // ゲームオブジェクトを取り除きます
    public void OnRemoveObject(GameObject obj)
    {
        for (var i = 0; i < m_lCollisionList.Count; i++)
        {
            if (m_lCollisionList[i].gameObject == obj)
            {
                m_lCollisionList.RemoveAt(i);
            }
        }
    }

    // エレベーターの移動量を足しこむ
    public void OnMoveObject(Vector3 vec)
    {
        if (!m_bIsMovedFlag)
        {
            Vector3 position = transform.position;
            position += vec;
            transform.position = position;
            m_bIsMovedFlag = true;
        }
    }

    // 当たっているコリジョンの数
    public int CollisionNum
    {
        get
        {
            return m_lCollisionList.Count;
        }
    }

    /// <summary>
    /// アニメーション通常再生
    /// </summary>
    /// <param name="anim">アニメーション番号</param>
    public void PlayStarAnimation(StarAnimation anim)
    {
        m_nAnimationNo = (int)anim;
        if (StarOrOrdinaly)
        {
            animation[0].Play(AnimationString[m_nAnimationNo]);
        }
        else
        {
            animation[1].Play(AnimationString[m_nAnimationNo]);
        }
        m_bAnimationFlag = false;
    }

    /// <summary>
    /// アニメーションフェードアウト再生
    /// </summary>
    /// <param name="anim">AnimationNumber</param>
    public void FadeStarAnimation(StarAnimation anim)
    {

        m_nAnimationNo = (int)anim;

        if (StarOrOrdinaly)
        {
            animation[0].CrossFade(AnimationString[m_nAnimationNo]);
        }
        else
        {
            animation[1].CrossFade(AnimationString[m_nAnimationNo]);
        }
        m_bAnimationFlag = false;
    }

    // アニメーションストップ
    public void StopAnimation()
    {
        if (StarOrOrdinaly)
        {
            animation[0].Stop();
        }
        else
        {
            animation[1].Stop();
        }
    }

    // AnimationClip.Length　Getter
    public float AnimationClipLength
    {
        get
        {
            if (StarOrOrdinaly)
            {
                return animation[0].GetClip(AnimationString[m_nAnimationNo]).length;
            }
            else
            {
                return animation[1].GetClip(AnimationString[m_nAnimationNo]).length;
            }
        }
    }

    /// <summary>
    /// AnimationClip.length Getter
    /// </summary>
    /// <param name="anim">AnimationNumber</param>
    /// <returns></returns>
    public float GetAnimationClipLength(StarAnimation anim)
    {
        if (StarOrOrdinaly)
        {
            return animation[0].GetClip(AnimationString[(int)anim]).length;
        }
        else
        {
            return animation[1].GetClip(AnimationString[(int)anim]).length;
        }
    }

    /// <summary>
    /// アニメーションステートをセット
    /// </summary>
    /// <param name="speed">アニメーションスピード、マイナスの値を入れると逆再生</param>
    /// <param name="animationlength">アニメーションの長さ</param>
    public void SetStarAnimationState(int speed, float animationlength)
    {
        foreach (AnimationState AnimState0 in animation[0])
        {
            AnimState0.speed = speed;
            AnimState0.time = animationlength;
        }
        foreach (AnimationState AnimState1 in animation[1])
        {
            AnimState1.speed = speed;
            AnimState1.time = animationlength;
        }
    }

    // 設定されたエフェクトのゲームオブジェクトを返す
    public GameObject GetEffect(EffectType type)
    {
        return Effect[(int)type];
    }

    // 電話ボックスに入っているか否か
    public bool TelboxIn
    {
        set
        {
            m_bTelboxIn = value;
        }
        get
        {
            return m_bTelboxIn;
        }
    }

    // スターか一般人かの状態ゲッター/セッター
    public bool StarOrdinaly
    {
        set
        {
            StarOrOrdinaly = value;
        }
        get
        {
            return StarOrOrdinaly;
        }
    }

    // 右向きか左向きかのゲッター/セッター
    public bool RightLeft
    {
        set
        {
            m_bRightorLeft = value;
        }
        get
        {
            return m_bRightorLeft;
        }
    }

    // エレベーターから降りれるか否か
    public bool ElevatorGetOff
    {
        get
        {
            return m_bElevatorOn;
        }
    }

    // 電話ボックスに入っているかのゲッター
    public bool StarOnTelbox
    {
        get
        {
            return m_bTelboxOn;
        }
    }

    // アニメーションを再生しない場合はfalseを代入してあげてください
    public void NonAnimation()
    {
        m_bAnimationFlag = false;
    }

    // ジャンプ時間
    public float Starjumptime
    {
        get
        {
            if (StarOrOrdinaly)
            {
                return JumpTime * 1.5f;
            }
            else
            {
                return JumpTime;
            }
        }
    }

    // ジャンプの高さ
    public float Starjumpheight
    {
        get
        {
            if (StarOrOrdinaly)
            {
                return JumpHeight * 2;
            }
            else
            {
                return JumpHeight;
            }
        }
    }

    // 落ちる速度
    public float StarFallSpeed
    {
        get
        {
            return FallSpeed;
        }
    }

    // 移動量のゲッター
    public float StarWalkSpeed
    {
        get
        {
            return MoveAmount * 3;
        }
    }

    // 縦の配列ゲッター
    public int Vertical
    {
        get
        {
            return m_nIndexVertical;
        }
    }

    // 縦の配列初期化用セッター(初期化以外で使用厳禁)
    public int InitVertical
    {
        set
        {
            if (!m_bVerticalInitFlag)
            {
                m_nInitVertical = value;
                m_nIndexVertical = value;
                m_bVerticalInitFlag = false;
            }
        }
    }

    // 横の配列ゲッター
    public int Horizontal
    {
        get
        {
            return m_nIndexHorizontal;
        }
    }

    // 横の配列初期化用セッター(初期化以外で使用厳禁)
    public int InitHorizontal
    {
        set
        {
            if (!m_bHorizontalInitFlag)
            {
                m_nInitHorizontal = value;
                m_nIndexHorizontal = value;
                m_bHorizontalInitFlag = true;
            }
        }
    }

    // ゲームオーバーフラグゲッター
    public bool IsGameOver
    {
        get
        {
            return m_bGameOver;
        }
    }
}
