using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface ITextCanvas : IEventSystemHandler
{
    void ChangeText(float _FadeTime);

    void EndText();
}

public class TextCanvas : MonoBehaviour, ITextCanvas
{
    [SerializeField]
    private Color DstColor = new Color();

    [SerializeField]
    private List<string> StringList = new List<string>();

    [SerializeField]
    private float BiasTime;

    private List<Text> m_cTextList = new List<Text>();

    private bool m_isTextFlg = false;

    private int m_nNowPage = 0;

    private float m_fColorCount = 0f;

    private float m_fFadeTime = 0f;

    // Start is called before the first frame update
    public void TextCanvasStart()
    {
        m_fColorCount = BiasTime;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).GetComponent<Text>() != null)
            {
                m_cTextList.Add(gameObject.transform.GetChild(i).GetComponent<Text>());
                m_cTextList[i].color = new Color(0f, 0f, 0f, 0f);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (m_isTextFlg == true)
        {
            if (m_nNowPage == m_cTextList.Count)
            {
                //m_nNowPage--;
            }
            else
            {
                if (m_fColorCount >= 1f)
                {
                    m_fColorCount = BiasTime;
                    m_nNowPage++;
                }
                else
                {
                    m_fColorCount += (Time.deltaTime / m_fFadeTime) ;
                    m_cTextList[m_nNowPage].color = new Color(DstColor.r, DstColor.g, DstColor.b, m_fColorCount);
                }
            }
        }
    }

    public void ChangeText(float _FadeTime)
    {
        m_fFadeTime = _FadeTime / m_cTextList.Count;
        m_isTextFlg = true;

        m_nNowPage = 0;
        m_fColorCount = BiasTime;
    }

    public void EndText()
    {
        m_isTextFlg = false;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

    }
}
