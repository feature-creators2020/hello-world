
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


/// <summary>
/// 固定エレベータ
/// </summary>

public interface IFixedElevatorInterface : IEventSystemHandler
{
    void Enter(GameObject gameObject, ObjectNo no);
    void Open();
    void Close();
    bool IsEnter(GameObject gameObject, ObjectNo no);
    bool IsOpen();
    bool IsClose();
    void SetPosition(int _x, int _y);
    void SetUseIcon(bool _isUse);
}

public enum FixedElevatorState
{
    Stop,
    Move,
    Open,
    Close,
}

public enum FixedElevatorAnimation
{
    Open,
    Close,
}

public enum FixedElevatorChildElement
{
    MyObj,
    ButtonIcon,
    Light,
    Floor,
    EnterIcon,
    MaxNum,
}


public class FixedElevator : CStateObjectBase<FixedElevator, FixedElevatorState>, IFixedElevatorInterface
{
    const int CAN_USE_NO = 90;
    const int CAN_NOT_USE_NO = 91;
    private string[] AnimationString = { "Open", "Close" };          // アニメーション名
    private int m_nAnimationNo;                                      // 再生中アニメーション番号
    private Animation m_cAnimation;                                  // アニメーション      
    private MapManager m_cMapManager;
    private FixedElevatorState m_eMyState;
    private Vector2Int m_vec2Pos;                                    //二次元配列状の自身の座標
    private GameObject m_cEnterObject;                               //エレベータに入っているオブジェクト
    private ObjectNo m_eEnterobjectNo;                               //エレベータに入っているオブジェクトのID
    private List<bool> m_isFlgList = new List<bool>();
    [SerializeField] private float m_fMoveTime;                      //エレベーターの移動時間
    [SerializeField] private bool m_isUseIcon;                       //ボタンアイコンオブジェクトを使うかどうか
    [SerializeField] private bool m_isUseLight;                      //ライトオブジェクトを使うかどうか
    [SerializeField] private bool m_isUseFloor;                      //床オブジェクトを使うかどうか
    [SerializeField] private bool m_isUseEnterIcon;                  //エンターアイコンを使うかどうか
    [SerializeField] private SceneObject TutorialStage;                    //チュートリアルのステージ

    public void Awake()
    {
        //マネージャーへ追加
        Reboot.ElevatorManager.Instance.m_lElevatorObjectList.Add(this);
    }

    public void ElevatorStart()
    {
        //ステート情報の保持
        var StateMachine = new CStateMachine<FixedElevator>();
        m_cStateMachineList.Add(StateMachine);

        var Stop = new FixedElevatorStop(this);
        var Move = new FixedElevatorMove(this);
        var Open = new FixedElevatorOpen(this);
        var Close = new FixedElevatorClose(this);

        m_cStateList.Add(Stop);
        m_cStateList.Add(Move);
        m_cStateList.Add(Open);
        m_cStateList.Add(Close);

        m_isFlgList.Add(true);          //自身
        m_isFlgList.Add(m_isUseIcon);
        m_isFlgList.Add(m_isUseLight);
        m_isFlgList.Add(m_isUseFloor);
        m_isFlgList.Add(m_isUseEnterIcon);

        m_cEnterObject = null;
        m_eEnterobjectNo = ObjectNo.None;
        m_cAnimation = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animation>();
        m_cMapManager = MapManager.Instance;

        //自身の使用可能状態の判別
        SettingIsUsing();

        //チュートリアルステージ設定
        TutorialStageSetting();
    }

    public override void StateUpdate()
    {
        if (global::PauseManager.IsPause == false)
        {
            base.StateUpdate();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ElevatorStart();
    }

    // Update is called once per frame
    //public override void Update()
    //{
    //    if (global::PauseManager.IsPause == false)
    //    {
    //        base.Update();
    //    }
    //}

    //自身の使用可能状態の判別
    private void SettingIsUsing()
    {
        switch (m_cMapManager.BackMapData[Position.y][Position.x])
        {
            case CAN_USE_NO:
                m_eMyState = FixedElevatorState.Open;
                m_cStateMachineList[0].ChangeState(m_cStateList[(int)FixedElevatorState.Open]);

                for (int i = 1; i < this.gameObject.transform.childCount; i++)
                {
                    if (m_isFlgList[i] == true)
                    {
                        this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
                    }

                }

                break;

            case CAN_NOT_USE_NO:
                m_eMyState = FixedElevatorState.Close;
                m_cStateMachineList[0].ChangeState(m_cStateList[(int)FixedElevatorState.Close]);

                for (int i = 1; i < this.gameObject.transform.childCount; i++)
                {
                    this.gameObject.transform.GetChild(i).gameObject.SetActive(false);

                    if ((int)FixedElevatorChildElement.Floor == i)
                    {//例外処理
                        if (m_isFlgList[(int)FixedElevatorChildElement.Floor] == true)
                        {
                            this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                }
                break;

            default:
                m_eMyState = FixedElevatorState.Stop;
                m_cStateMachineList[0].ChangeState(m_cStateList[(int)FixedElevatorState.Stop]);
                break;
        }
    }



    public void Enter(GameObject gameObject, ObjectNo no)
    {

        //マップデータ更新
        if (m_isUseIcon == true)
        {
            this.gameObject.transform.GetChild((int)FixedElevatorChildElement.ButtonIcon).gameObject.SetActive(false);
        }

        if (m_isUseLight == true)
        {
            this.gameObject.transform.GetChild((int)FixedElevatorChildElement.Light).gameObject.SetActive(false);
        }

        //入場処理　CAUT　： 格納するマップに注意
        switch (no)
        {
            case ObjectNo.Star:
               
                MapManager.BackMapData[Position.y][Position.x] = CAN_NOT_USE_NO;

                break;

            case ObjectNo.Paparazzi:

                MapManager.FrontMapData[Position.y][Position.x] = (int)ObjectNo.None;
                MapManager.BackMapData[Position.y][Position.x] = CAN_NOT_USE_NO;

                break;
        }

        this.ChangeState(0, FixedElevatorState.Move);

    }

    //入れるかどうか
    public bool IsEnter(GameObject gameObject, ObjectNo no)
    {
        if (m_cEnterObject != null)
        {
            return false;
        }

        m_eEnterobjectNo = no;
        m_cEnterObject = gameObject;

        return true;
    }




    public void Open()
    {
        if (FixedElevatorState != FixedElevatorState.Move)
        {
            m_cStateMachineList[0].ChangeState(m_cStateList[(int)FixedElevatorState.Open]);
        }
    }


    public bool IsOpen()
    {
        if(IsCurrentState(0,FixedElevatorState.Open))
        {
            return true;
        }

        return false;
    }

    public void Close()
    {
        if (FixedElevatorState != FixedElevatorState.Move)
        {
            m_cStateMachineList[0].ChangeState(m_cStateList[(int)FixedElevatorState.Close]);
        }
    }

    public bool IsClose()
    {
        if (IsCurrentState(0, FixedElevatorState.Close))
        {
            return true;
        }
        return false;
    }


    public void SetPosition(int _x, int _y)
    {
        m_vec2Pos = new Vector2Int(_x, _y);
    }


    public void SetUseIcon(bool _isUse)
    {
        m_isUseIcon = _isUse;


        switch (m_cMapManager.BackMapData[Position.y][Position.x])
        {
            case CAN_USE_NO:
                if (m_isUseIcon == true)
                {
                    this.gameObject.transform.GetChild((int)FixedElevatorChildElement.ButtonIcon).gameObject.SetActive(true);
                }
                else
                {
                    this.gameObject.transform.GetChild((int)FixedElevatorChildElement.ButtonIcon).gameObject.SetActive(false);
                }
                break;

            case CAN_NOT_USE_NO:
                this.gameObject.transform.GetChild((int)FixedElevatorChildElement.ButtonIcon).gameObject.SetActive(false);

                break;

        }
    }

    public void PlayAnimation(FixedElevatorAnimation anim)
    {
        m_nAnimationNo = (int)anim;
        m_cAnimation.Play(AnimationString[m_nAnimationNo]);
    }

    private void TutorialStageSetting()
    {
        if (TutorialStage == SceneManager.GetActiveScene().name)
        {
            int index = (int)FixedElevatorChildElement.ButtonIcon;
            m_isUseIcon = true;
            m_isFlgList[index] = true;

            switch (m_cMapManager.BackMapData[Position.y][Position.x])
            {
                case CAN_USE_NO:
                    this.gameObject.transform.GetChild(index).gameObject.SetActive(true);
                    break;

                case CAN_NOT_USE_NO:
                    this.gameObject.transform.GetChild(index).gameObject.SetActive(false);

                    break;
            }
        }
    }

    public GameObject EnterObject
    {
        get { return m_cEnterObject; }
        set { m_cEnterObject = value; }
    }

    public ObjectNo EnterObjectNo
    {
        get { return m_eEnterobjectNo; }
        set { m_eEnterobjectNo = value; }
    }

    public Animation GetAnimation { get { return m_cAnimation; } }

    public FixedElevatorState FixedElevatorState
    {
        set { m_eMyState = value; }
        get { return m_eMyState; }
    }

    public MapManager MapManager
    {
        get { return m_cMapManager; }
    }

    public Vector2Int Position
    {
//        set { m_vec2Pos = value; }
        get { return m_vec2Pos; }
    }

    public bool IsUseIcon
    {
        get { return m_isUseIcon;  }
        set { m_isUseIcon = value; }
    }

    public bool IsUseLight
    {
        get { return m_isUseLight; }
        set { m_isUseLight = value; }
    }

    public bool IsUseEnterIcon
    {
        get { return m_isUseEnterIcon; }
        set { m_isUseEnterIcon = value; }
    }

    public bool IsFlg(FixedElevatorChildElement _ChildEle)
    {
        return m_isFlgList[(int)_ChildEle];
    }

    public float MoveTime{ get { return m_fMoveTime; }}
    public int CanUseNo { get { return CAN_USE_NO; } }
    public int CanNotUseNo { get { return CAN_NOT_USE_NO; } }
}
