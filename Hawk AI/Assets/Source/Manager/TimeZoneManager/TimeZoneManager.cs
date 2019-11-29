using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IETimeZone : IEventSystemHandler
{
    ETimeZone TimeZoneStatus { get; set; }
}


public enum ETimeZone
{
    eMooning,
    eEvenning
}


public class TimeZoneManager : GeneralManager, IETimeZone
{
    [SerializeField]
    private GameObject m_cTimeManager = null;

    [SerializeField]
    private GameObject m_cDirectionaLight = null;

    [SerializeField]
    private float m_fLerpRotationX;

    [SerializeField]
    private float m_fLerpRotationTime;

    public ETimeZone TimeZoneStatus
    {
        get { return m_eTimeZone; }
        set { m_eTimeZone = value; }
    }

    private bool m_bCoroutineFlg = false;
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
            if (m_bCoroutineFlg != true)
            {
                m_bCoroutineFlg = true;
                StartCoroutine(LightingCoroutine());
            }

            m_eTimeZone = ETimeZone.eEvenning;
        }
    }

    IEnumerator LightingCoroutine()
    {
        float lerpVal = 0f;
        Vector3 Start = m_cDirectionaLight.transform.position;
        Vector3 End = new Vector3(
            m_fLerpRotationX,
            m_cDirectionaLight.transform.position.y,
            m_cDirectionaLight.transform.position.z);

        while (lerpVal <= 1f)
        {//閉まる時間補間
            m_cDirectionaLight.transform.rotation = Quaternion.Euler(
                Vector3.Lerp(Start, End, lerpVal));
            lerpVal += Time.deltaTime / m_fLerpRotationTime;
            yield return null;
        }

        if (Start != End)
        {//強硬手段
            m_cDirectionaLight.transform.rotation = Quaternion.Euler(End);
        }
    }
}
