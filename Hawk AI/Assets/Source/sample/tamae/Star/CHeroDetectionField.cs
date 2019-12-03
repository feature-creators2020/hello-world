using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IHeroDetectionInterface : IEventSystemHandler
{
    void OnRemoveObject(GameObject obj);
}

public class CHeroDetectionField : MonoBehaviour, IHeroDetectionInterface
{

    private GameObject m_cAura = null;

    private Star m_cOwnerComponent = null;

    private BoxCollider m_cCollider = null;

    private List<GameObject> m_lFanList = new List<GameObject>();

    [SerializeField]
    private int m_nPartitionCount = 5;

    private bool m_bAuraFlag = false;

    [SerializeField]
    private float m_fRadiusMagnification = 1f;

    private Vector3 m_vSize;
    private float m_fInitialRadius;

    public int FanCount
    {
        get
        {
            return m_lFanList.Count;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_cAura = transform.GetChild(0).gameObject;

        m_cOwnerComponent = transform.parent.GetComponent<Star>();
        m_cCollider = GetComponent<BoxCollider>();
        m_fInitialRadius = m_cCollider.size.z;
        m_vSize = m_cCollider.size;

        m_cAura.SetActive(m_bAuraFlag);
    }

    // Update is called once per frame
    void Update()
    {
        if (!global::PauseManager.IsPause)
        {
            if (!m_cOwnerComponent.StarOrdinaly)
            {
                m_cAura.SetActive(false);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }

        //Debug.Log(m_lFanList.Count);
    }

    void Detection()
    {
        if (m_lFanList.Count >= m_nPartitionCount)
        {
            m_bAuraFlag = true;
            Vector3 vec = m_vSize;
            vec.z = m_fInitialRadius + m_fRadiusMagnification;
            m_cCollider.size = vec;
        }
        else
        {
            m_bAuraFlag = false;
            m_cCollider.size = m_vSize;
        }
        m_cAura.SetActive(m_bAuraFlag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_cOwnerComponent.StarOrdinaly)
        {
            if (other.gameObject.tag == "FanMain")
            {
                m_lFanList.Add(other.gameObject);
                Detection();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_cOwnerComponent.StarOrdinaly)
        {
            if (other.gameObject.tag == "FanMain")
            {
                m_lFanList.Remove(other.gameObject);
                Detection();
            }
        }
    }

    public void OnRemoveObject(GameObject obj)
    {
        m_lFanList.Remove(obj);
        Detection();
    }

    public bool IsLegend
    {
        get
        {
            return m_bAuraFlag;
        }
    }

}
