using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface IResultManagerInterfase
{
    void HawkAIWin();
    void MouseWin();
}

public enum EResultCanvas
{
    eBackScreen,
    eFont
}

public enum EResultImage
{
    eLeftBack,
    eRightBack,
    eLeftFont,
    eRightFont
}

public class ResultManager : MonoBehaviour, IResultManagerInterfase
{
    [SerializeField]
    private Sprite m_cWinSprite;

    [SerializeField]
    private Sprite m_cLoseSprite;

    private List<Image> m_cImageList = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        // Hack : Make Chileren Class

        GameObject ImgaeObj = null;
        RectTransform CanvasRectTrans = null;
        RectTransform ImageRectTrans = null;

        //LeftBack
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultCanvas.eBackScreen).
            transform.GetChild((int)EResultImage.eLeftBack).gameObject;

        m_cImageList.Add(ImgaeObj.GetComponent<Image>());
        ImageRectTrans = ImgaeObj.GetComponent<RectTransform>();
        CanvasRectTrans = this.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        ImageRectTrans.localPosition -= new Vector3(CanvasRectTrans.sizeDelta.x / 2,0,0);

        //RightBack
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultCanvas.eBackScreen).
            transform.GetChild((int)EResultImage.eRightBack).gameObject;
        m_cImageList.Add(ImgaeObj.GetComponent<Image>());
        ImageRectTrans = ImgaeObj.GetComponent<RectTransform>();
        ImageRectTrans.localPosition += new Vector3(CanvasRectTrans.sizeDelta.x / 2, 0, 0);

        //LeftFont
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultCanvas.eFont).
            transform.GetChild(0).gameObject;
        m_cImageList.Add(ImgaeObj.GetComponent<Image>());

        //LeftFont
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultCanvas.eFont).
            transform.GetChild(1).gameObject;
        m_cImageList.Add(ImgaeObj.GetComponent<Image>());


        foreach(var val in m_cImageList)
        {
            val.sprite = null;
            val.color = Color.clear;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            HawkAIWin();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            MouseWin();
        }

    }

    public void HawkAIWin()
    {
        //Left
        GameObject ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eLeftBack).gameObject;
        m_cImageList[(int)EResultImage.eLeftBack].color = Color.green;
        m_cImageList[(int)EResultImage.eLeftFont].sprite = m_cWinSprite;
        m_cImageList[(int)EResultImage.eLeftFont].color = Color.white;

        //Right
        ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eRightBack).gameObject;
        m_cImageList[(int)EResultImage.eRightBack].color = Color.red;
        m_cImageList[(int)EResultImage.eRightFont].sprite = m_cLoseSprite;
        m_cImageList[(int)EResultImage.eRightFont].color = Color.white;

    }

    public void MouseWin()
    {
        //Left
        GameObject ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eLeftBack).gameObject;
        m_cImageList[(int)EResultImage.eLeftBack].color = Color.red;
        m_cImageList[(int)EResultImage.eLeftFont].sprite = m_cLoseSprite;
        m_cImageList[(int)EResultImage.eLeftFont].color = Color.white;

        //Right
        ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eRightBack).gameObject;
        m_cImageList[(int)EResultImage.eRightBack].color = Color.green;
        m_cImageList[(int)EResultImage.eRightFont].sprite = m_cWinSprite;
        m_cImageList[(int)EResultImage.eRightFont].color = Color.white;


    }
}
