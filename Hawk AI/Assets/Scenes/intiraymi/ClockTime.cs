using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTime : GeneralObject
{
    //private bool m_bTimeFlag = false;
    //private float m_fNowTime = 0;
    //private float m_fEndTime = 0;
    private float m_fHandAngle = 0;
    //public GameObject Hand;

    //void Init()
    //{
    //    m_bTimeFlag = true;
    //    m_fEndTime = 180.0f;
    //    Debug.Log(m_fHandAngle);
    //}

    //void Update()
    //{
    //    if (m_bTimeFlag)
    //    {
    //        m_fNowTime += 1f;
    //        m_fHandAngle = (m_fNowTime / m_fEndTime) * 360.0f;
    //        Hand.transform.eulerAngles = new Vector3(0, 0, -m_fHandAngle);
    //        Debug.Log(m_fHandAngle);

    //        if (m_fHandAngle <= 360.0f)
    //        {
    //            m_fHandAngle = 360.0f;
    //            m_bTimeFlag = false;
    //        }
    //    }
    //}
    public override void GeneralInit()
    {
        base.GeneralInit();
        Debug.Log(m_fHandAngle);
    }
}
