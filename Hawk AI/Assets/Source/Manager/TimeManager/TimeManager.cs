using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public interface ITimeManager : IEventSystemHandler
{
    void TimeStart();
    void TimePause();
    void TimeStop();
    void TimeEnd();

    bool IsTimeCounting { get; }
    float ExecuteTime { get; }
    float EndOfTheTime { get;}
}



/// <summary>
/// @name : TimeManager
/// </summary>
public class TimeManager : GeneralManager, ITimeManager
{
    [SerializeField]
    private float m_fEndOfTheTime;

    private bool m_bTimeCounting = false;
    private float m_fNowCountTime = 0f;
    
    // Start is called before the first frame update
    public override void GeneralInit()
    {
        //m_bTimeCounting = true;
    }

    // Update is called once per frame
    public override void GeneralUpdate()
    {
        if (m_bTimeCounting == true)
        {
            m_fNowCountTime += Time.deltaTime;
            //Debug.Log("Time : " + m_fNowCountTime);

            if (m_fNowCountTime >= m_fEndOfTheTime)
            {
                m_bTimeCounting = false;
                Debug.Log("TimeUp!");
            }
        }

        DebugUpdate();

    }

    public override void GeneralRelease()
    {

    }

    public override void DebugUpdate()
    {
        if (m_bTimeCounting == true)
        {
            if(Input.GetKey(KeyCode.Alpha8))
            {
                m_fNowCountTime += 3f;
                Debug.Log("Forward Track Skipping Time : " + m_fNowCountTime);
            }

            if (Input.GetKey(KeyCode.Alpha7))
            {
                m_fNowCountTime -= 3f;
                Debug.Log("ReWind Track Time : " + m_fNowCountTime);
            }

        }
    }

    public void TimeStart()
    {
        m_bTimeCounting = true;
    }

    public void TimePause()
    {
        m_bTimeCounting = false;
    }

    public void TimeStop()
    {
        if(m_bTimeCounting != false)
        {
            m_bTimeCounting = false;
            m_fNowCountTime = 0f;
        }
    }

    public void TimeEnd()
    {
        if (m_bTimeCounting != false)
        {
            m_bTimeCounting = false;
            m_fNowCountTime = 0f;
        }

    }

    public bool IsTimeCounting
    {
        get { return m_bTimeCounting; }
    }

    public float ExecuteTime
    {
        get { return m_fNowCountTime; }
    }

    public float EndOfTheTime
    {
        get { return m_fEndOfTheTime; }
        //set { m_fEndOfTheTime = value; }
    }

}
