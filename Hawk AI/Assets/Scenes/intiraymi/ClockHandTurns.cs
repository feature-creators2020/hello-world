using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockHandTurns : MonoBehaviour
{
    private bool m_bTimeFlag = false;
    private float m_fNowTime = 0;
    private float m_fEndTime = 0;
    private float m_fHandAngle = 0;
    public GameObject Hand;
    // Start is called before the first frame update
    void Start()
    {
        m_bTimeFlag = true;
        m_fEndTime = 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bTimeFlag)
        {
            m_fNowTime += Time.deltaTime;
            m_fHandAngle = (m_fNowTime / m_fEndTime);
            Hand.transform.eulerAngles = new Vector3(0, 0, -m_fHandAngle * 360.0f);
            Hand.transform.localPosition = new Vector3(0.2f * Mathf.Sin(2 * Mathf.PI * m_fHandAngle), 0.2f * Mathf.Cos(2 * Mathf.PI * m_fHandAngle), 0);
            Debug.Log(m_fNowTime);
            if (m_fHandAngle >= 1.0f)
            {
                m_fHandAngle = 0;
                m_bTimeFlag = false;
            }
        }
    }
}
