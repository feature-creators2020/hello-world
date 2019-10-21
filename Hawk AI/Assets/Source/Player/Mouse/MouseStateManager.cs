using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public enum EMouseState
{
    Normal,
    SlowDown,
    Door
}

public class MouseStateManager : CStateObjectBase<MouseStateManager, EMouseState>
{

    public float inputHorizontal;              // コントローラーLスティック横軸情報
    public float inputVertical;                // コントローラーLスティック縦軸情報
    public Camera targetCamera;         // 対象のカメラ
    public GamePad.Index GamePadIndex;  // 対象のコントローラー

    public float m_fmoveSpeed;           // 固定の速度
    public float m_fSlowDownRate;        // ネズミこうを踏んだとき
    public float m_fDoorRate;            // ドアの下を通るとき
    /*{
        get { return m_fmoveSpeed; }
        set { m_fmoveSpeed = value; }
    }*/

    public float RunRate                // 別状態の速度倍率
    {
        get { return RunRate; }
        set { RunRate = value; }
    }               


    // Start is called before the first frame update
    void Start()
    {
        var StateMachine = new CStateMachine<MouseStateManager>();
        m_cStateMachineList.Add(StateMachine);

        var Normal = new NormalManager(this);
        var SlowDown = new SlowDownManager(this);
        var Door = new DoorManager(this);

        m_cStateList.Add(Normal);
        m_cStateList.Add(SlowDown);
        m_cStateList.Add(Door);

        m_cStateMachineList[0].ChangeState(m_cStateList[(int)EMouseState.Normal]);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
