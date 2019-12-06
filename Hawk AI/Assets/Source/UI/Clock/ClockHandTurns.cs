using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClockHandTurns : GeneralObject
{
    private bool m_bTimeFlag = false;
    private float m_fNowTime = 0;
    private float m_fEndTime = 0;
    private float m_fHandAngle = 0;
    private GameObject Hand;
    private GameObject ClockBoard;
    //デバッグ用
    public float GameTime;
    private GameObject TimeManager;

    // Start is called before the first frame update
    void Start()
    {
        ExecuteEvents.Execute<ITimeManager>(
                target: TimeManager,
                eventData: null,
                functor: (recieveTarget, y) => m_fNowTime = recieveTarget.ExecuteTime);
        Hand = this.transform.Find("Hand").gameObject;
        ClockBoard = this.transform.Find("ClockBoard").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        TimeManager = ManagerObjectManager.Instance.GetGameObject("TimeManager");

        ExecuteEvents.Execute<ITimeManager>(
        target: TimeManager,
        eventData: null,
        functor: (recieveTarget, y) => m_fNowTime = recieveTarget.ExecuteTime);

        ExecuteEvents.Execute<ITimeManager>(
        target: TimeManager,
        eventData: null,
        functor: (recieveTarget, y) => m_bTimeFlag = recieveTarget.IsTimeCounting);

        m_fEndTime = GameTime;


        if (m_bTimeFlag && SceneManager.GetActiveScene().name == "GameMain")
        {
            //m_fNowTime += Time.deltaTime;
            m_fHandAngle = (m_fNowTime / m_fEndTime);
            Hand.transform.eulerAngles = new Vector3(0, 0, -m_fHandAngle * 360.0f);
            //Hand.transform.localPosition = new Vector3(0.2f * Mathf.Sin(2 * Mathf.PI * m_fHandAngle), 0.2f * Mathf.Cos(2 * Mathf.PI * m_fHandAngle), 0);
            if (m_fEndTime - m_fNowTime < 11f)
            {
                Color color = Hand.GetComponent<Image>().color;
                Hand.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a - 0.01f);
                CountDownAnimation.Instance.SetCount10(m_fEndTime - m_fNowTime - 1);
            }
            if (m_fHandAngle >= 17f / 18f)
            {
                float colorchenge = Mathf.Cos(4 * Mathf.PI * m_fNowTime) * 0.3f;
                ClockBoard.GetComponent<Image>().color = new Color(1, 0.7f + colorchenge, 0.7f + colorchenge);
            }
            else if (m_fHandAngle >= 5f / 6f)
            {
                float colorchenge = Mathf.Cos(2 * Mathf.PI * m_fNowTime) * 0.3f;
                ClockBoard.GetComponent<Image>().color = new Color(1, 0.7f + colorchenge, 0.7f + colorchenge);
            }
            else if (m_fHandAngle > 0f)
            {
                Color color = Hand.GetComponent<Image>().color;
                Hand.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a + 0.01f);
            }
            if (m_fHandAngle >= 1.0f)
            {
                m_fHandAngle = 0;
                m_bTimeFlag = false;
            }
            //Debug.Log(m_fEndTime - m_fNowTime);
        }
    }
}

