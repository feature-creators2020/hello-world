using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    [SerializeField]
    private Sprite m_cNormal;
    [SerializeField]
    private Sprite m_cStar;
    //[SerializeField]
    //private Sprite m_cLegend;

    private Image m_cImage = null;

    private Star m_cTarget = null;
    //private CHeroDetectionField m_cField = null;
    //private GameObject m_cAura;

    // Start is called before the first frame update
    void Start()
    {
        var obj = GameObject.FindGameObjectWithTag("Player");
        m_cTarget = obj.GetComponent<Star>();

        //var field = obj.transform.GetChild(2).gameObject;
        //m_cField = field.GetComponent<CHeroDetectionField>();

        m_cImage = GetComponent<Image>();
        if (m_cTarget.StarOrdinaly)
        {
            m_cImage.sprite = m_cStar;
        }
        else
        {
            m_cImage.sprite = m_cNormal;
        }

        //m_cAura = transform.root.GetChild(4).GetChild(2).gameObject;
        //m_cAura.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!global::PauseManager.IsPause)
        {
            if (m_cTarget.StarOrdinaly)
            {
                m_cImage.sprite = m_cStar;

                //if (m_cField.IsLegend)
                //{
                //    m_cAura.SetActive(true);
                //}
                //else
                //{
                //    m_cAura.SetActive(false);
                //}
            }
            else
            {
                //Normal
                m_cImage.sprite = m_cNormal;

                //m_cAura.SetActive(false);
            }


        }
    }
}
