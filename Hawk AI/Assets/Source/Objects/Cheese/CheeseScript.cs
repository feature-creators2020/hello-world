using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICheeseInterfase : IEventSystemHandler
{
    void StartScaling(float _time);
    void SetDefault();
}


public class CheeseScript : MonoBehaviour, ICheeseInterfase
{
    float m_fMaxTime = 1f;      // 縮小時間
    float m_fScalingTime;       // 現在の縮小時間
    bool m_isScaling = false;   // 縮小可能か
    bool m_isSetting = false;   // 初期値を設定済みか
    Vector3 m_vDefaultScale;    // 元の大きさ
    Vector3 m_vTargetScale;     // 目的の縮小サイズ
    public GameObject m_CheeseObject;  // 縮小するチーズのオブジェクト情報
    Vector3 m_vDefaultPosition; // 元の位置
    Vector3 m_vTargetPosition;  // 目的の位置

    [SerializeField]
    public GameObject m_cCheeseEffects;

    // Start is called before the first frame update
    void Start()
    {
        //m_vDefaultScale = m_CheeseObject.transform.localScale;
        //m_isScaling = false;
        //m_vTargetScale = new Vector3(0.1f, 0.1f, 0.1f);
        //m_fScalingTime = 0f;
        //m_vDefaultPosition = m_CheeseObject.transform.position;
        //m_vTargetPosition = m_vDefaultPosition + new Vector3(0f, -0.6f, 0f);

    }

    void OnEnable()
    {
        if (!m_isSetting)
        {
            //m_CheeseObject = this.gameObject.transform.GetChild(0).gameObject;
            m_vDefaultScale = m_CheeseObject.transform.localScale;
            m_isScaling = false;
            m_vTargetScale = new Vector3(0.1f, 0.1f, 0.1f);
            m_fScalingTime = 0f;
            m_vDefaultPosition = m_CheeseObject.transform.position;
            m_vTargetPosition = m_vDefaultPosition + new Vector3(0f, -0.3f, 0f);
            m_isSetting = true;
        }

        ExecuteEvents.Execute<ICheeseEffect>(
        target: m_cCheeseEffects,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.Play());

        
    }


    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled)
        {
            if (m_isScaling)
            {
                m_CheeseObject.transform.localScale = Vector3.Lerp(m_vDefaultScale, m_vTargetScale, m_fScalingTime / m_fMaxTime);
                m_CheeseObject.transform.position = Vector3.Lerp(m_vDefaultPosition, m_vTargetPosition, m_fScalingTime / m_fMaxTime);
                if (m_fScalingTime < m_fMaxTime)
                {
                    m_fScalingTime += Time.deltaTime;
                }
            }
        }
        else
        {
            SetDefault();
        }
    }

    // サイズを縮小させるのを始める(time : 縮小時間)
    public void StartScaling(float _time)
    {
        m_fMaxTime = _time;
        m_isScaling = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Mouse")
        {
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Mouse")
        {
            m_isScaling = false;
            SetDefault();
        }
    }

    public void SetDefault()
    {
        m_fScalingTime = 0f;
        m_CheeseObject.transform.localScale = Vector3.Lerp(m_vDefaultScale, m_vTargetScale, 0f);
        m_CheeseObject.transform.position = Vector3.Lerp(m_vDefaultPosition, m_vTargetPosition, 0f);
        m_isScaling = false;


    }
}
