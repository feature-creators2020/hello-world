using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    private static bool m_bHumanWin = false;

    public static bool IsHumanWin
    {
        get { return m_bHumanWin; }
        set { m_bHumanWin = value; }
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
        if(m_cStateMachineList[0].GetCurrentState() == m_cStateList[(int)EGameState.Ready])
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

}
