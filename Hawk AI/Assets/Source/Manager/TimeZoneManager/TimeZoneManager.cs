using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ETimeZone
{
    eMooning,
    eEvenning
}


public class TimeZoneManager : GeneralManager
{
    private GameObject m_cTimeManager = null;

    public ETimeZone TimeZoneStatus
    {
        get { return m_eTimeZone; }
        set { m_eTimeZone = value; }
    }

    private ETimeZone m_eTimeZone = ETimeZone.eMooning;

    public override void GeneralInit()
    {
    }

    public override void GeneralUpdate()
    {
        Stating();
    }

    public override void GeneralRelease()
    {

    }

    public override void DebugUpdate()
    {


    }

    private void Stating()
    {
        // Hack : Call func At Once
        m_cTimeManager = ManagerObjectManager.Instance.GetGameObject("TimeManager");

        float NowTime = 0f;
        float EndTime = 0f;

        ExecuteEvents.Execute<ITimeManager>(
        target: m_cTimeManager,
        eventData: null,
        functor: (recieveTarget, y) => NowTime = recieveTarget.ExecuteTime);

        ExecuteEvents.Execute<ITimeManager>(
        target: m_cTimeManager,
        eventData: null,
        functor: (recieveTarget, y) => EndTime = recieveTarget.EndOfTheTime);

        if (NowTime == 0f)
            return;

        if (NowTime >= (EndTime / 2))
        {
            m_eTimeZone = ETimeZone.eEvenning;
        }
    }
}
