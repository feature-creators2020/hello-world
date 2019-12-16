using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using GamepadInput;
using KeyBoardInput;

/// <summary>
/// @name : GameManager
/// </summary>
/// 

public interface IGameInterface : IEventSystemHandler
{
    void ChangeState(EGameState eGameState);

    EGameState GetGameState();
}

public enum EGameState
{
    Ready,
    Main,
    End
}

public class GameManager : CStateObjectBase<GameManager, EGameState>, IGameInterface, IGeneralInterface
{
    [SerializeField]
    private EGameState NowManagerState;

    [SerializeField]
    public float ResultStatingTime;

    private static bool m_bHumanWin = true;
    private static int m_nKillCountByHuman1 = 0;
    private static int m_nKillCountByHuman2 = 0;
    private static int m_nEatCountByMouse1 = 0;
    private static int m_nEatCountByMouse2 = 0;

    public static bool IsHumanWin
    {
        get { return m_bHumanWin; }
        set { m_bHumanWin = value; }
    }

    public static int KillCountByHuman1
    {
        get { return m_nKillCountByHuman1; }
        set { m_nKillCountByHuman1 = value; }

    }
    public static int KillCountByHuman2
    {
        get { return m_nKillCountByHuman2; }
        set { m_nKillCountByHuman2 = value; }

    }

    public static int EatCountByMouse1
    {
        get { return m_nEatCountByMouse1; }
        set { m_nEatCountByMouse1 = value; }

    }
    public static int EatCountByMouse2
    {
        get { return m_nEatCountByMouse2; }
        set { m_nEatCountByMouse2 = value; }

    }


    // Start is called before the first frame update
    public virtual void GeneralInit()
    {


        var StateMachine = new CStateMachine<GameManager>();
        m_cStateMachineList.Add(StateMachine);

        var Ready = new ReadyManager(this);
        var GameMain = new GameMainManager(this);
        var End = new EndManager(this);

        m_cStateList.Add(Ready);
        m_cStateList.Add(GameMain);
        m_cStateList.Add(End);



        m_cStateMachineList[0].ChangeState(m_cStateList[(int)NowManagerState]);
    }

    // Update is called once per frame
    public virtual void GeneralUpdate()
    {
        //base.Update();
        this.DebugUpdate();

        Debug.Log("human1 kill : " + KillCountByHuman1);
        Debug.Log("human2 kill : " + KillCountByHuman1);
        Debug.Log("mouse1 eat : " + m_nEatCountByMouse1);
        Debug.Log("mouse2 eat : " + m_nEatCountByMouse2);
    }

    public virtual void GeneralRelease()
    {

    }

    public virtual GameObject GetGameObject(int _ID)
    {
        Debug.Log("GetGameObject");

        return this.gameObject;
    }

    public virtual GameObject GetGameObject(string _Str)
    {
        Debug.Log("GetGameObject");

        return this.gameObject;
    }

    public virtual List<GameObject> GetGameObjectsList()
    {
        Debug.Log("GetGameObjectsList");

        return null;
    }
    public virtual void ChangeState(EGameState eGameState)
    {
        m_cStateMachineList[0].ChangeState(m_cStateList[(int)eGameState]);

    }

    public virtual EGameState GetGameState()
    {
        if (m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)EGameState.Ready])
        {
            return EGameState.Ready;
        }
        else if (m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)EGameState.Main])
        {
            return EGameState.Main;
        }
        else
        {
            return EGameState.End;
        }

    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ExecuteEvents.Execute<ISceneInterfase>(
            target: ManagerObjectManager.Instance.GetGameObject("SceneManager"),
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.Title));
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

#if UNITY_EDITOR



#elif UNITY_STANDALONE_WIN
            System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
            Application.Quit();
#endif
        }


        if (Input.GetKey(KeyCode.Escape)) Quit();

    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
}
