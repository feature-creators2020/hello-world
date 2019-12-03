using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashImageController : MonoBehaviour
{
    [SerializeField]
    private float FlashRate = 0f;

    private bool m_bContinueFlashFlg = false;
    private float m_fCountRate = 0f;
    private Image m_cImage;

    // Start is called before the first frame update
    void Start()
    {
        m_cImage = this.gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //m_fCountTime += Time.deltaTime;
        if (m_bContinueFlashFlg == true)
        {
            m_fCountRate += FlashRate;
            m_cImage.color = new Color(m_cImage.color.r, m_cImage.color.g, m_cImage.color.b, m_fCountRate);
                
            if(m_fCountRate >= 1f)
            m_bContinueFlashFlg = false;
        }
        else
        {
            m_fCountRate -= FlashRate;
            m_cImage.color = new Color(m_cImage.color.r, m_cImage.color.g, m_cImage.color.b, m_fCountRate);

            if (m_fCountRate <= 0f)
                m_bContinueFlashFlg = true;
        }

    }


}
