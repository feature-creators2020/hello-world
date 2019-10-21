using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTime : GeneralManager
{
    private bool m_bTimeFlag = false;
    private float m_fNowTime = 0;
    private float m_fEndTime = 0;
    private float m_fHandAngle = 0;
    public GameObject Hand;

    public override void GeneralInit()
    {
        m_bTimeFlag = true;
        m_fEndTime = 180.0f;
    }

    public override void GeneralUpdate()
    {
        if (m_bTimeFlag)
        {
            m_fNowTime += Time.deltaTime;
            m_fHandAngle = (m_fNowTime / m_fEndTime) * 360.0f;
            Hand.transform.eulerAngles = new Vector3(0, 0, -m_fHandAngle);

            if (m_fHandAngle <= 360.0f)
            {
                m_fHandAngle = 360.0f;
                m_bTimeFlag = false;
            }
        }
    }
}
