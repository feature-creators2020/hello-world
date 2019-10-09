using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// @name : GameManager
/// </summary>
/// 

public interface IGameInterface : IEventSystemHandler
{
    
}

public enum EGameState
{
    Ready,
    Main,
    End
}

public class GameManager : CStateObjectBase<GameManager, EGameState>, IGameInterface
{
    // Start is called before the first frame update
    void Start()
    {
        var StateMachine = new CStateMachine<GameManager>();
        m_cStateMachineList.Add(StateMachine);

        var Ready = new ReadyManager(this);
        var GameMain = new GameMainManager(this);
        var End = new EndManager(this);

        m_cStateList.Add(Ready);
        m_cStateList.Add(GameMain);
        m_cStateList.Add(End);

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EGameState.Ready]);
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();
    }
}
