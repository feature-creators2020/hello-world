﻿using System.Collections;
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
    float EndOfTheTime { get; set; }
}



/// <summary>
/// @name : TimeManager
/// </summary>
public class TimeManager : GeneralManager
{
    private bool m_bTimeCounting = false;
    private float m_fNowCountTime = 0f;
    private float m_fEndOfTheTime = 0f;
    
    // Start is called before the first frame update
    public override void GeneralInit()
    {
        m_bTimeCounting = true;
    }

    // Update is called once per frame
    public override void GeneralUpdate()
    {
        if (m_bTimeCounting == true)
        {
            m_fNowCountTime += Time.deltaTime;
            Debug.Log("Time : " + m_fNowCountTime);

            if (m_fNowCountTime >= m_fEndOfTheTime)
            {
                m_bTimeCounting = false;
                Debug.Log("TimeUp!");
            }
        }
    }

    public override void GeneralRelease()
    {

    }

    void TimeStart()
    {
        m_bTimeCounting = true;
    }

    void TimePause()
    {
        m_bTimeCounting = false;
    }

    void TimeStop()
    {
        if(m_bTimeCounting != false)
        {
            m_bTimeCounting = false;
            m_fNowCountTime = 0f;
        }
    }

    void TimeEnd()
    {
        if (m_bTimeCounting != false)
        {
            m_bTimeCounting = false;
            m_fNowCountTime = 0f;
        }

    }

    bool IsTimeCounting
    {
        get { return m_bTimeCounting; }
    }

    float ExecuteTime
    {
        get { return m_fNowCountTime; }
    }

    float EndOfTheTime
    {
        get { return m_fEndOfTheTime; }
        set { m_fEndOfTheTime = value; }
    }

}