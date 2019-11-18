using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IResultManagerInterfase
{
    void HawkAIWin();
    void MouseWin();
}

public enum EResultChildObj
{
    eBackScreen,
    eFont,
    eEffects
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
    [SerializeField]
    private Color m_cLeftBackColor;
    [SerializeField]
    private Color m_cRightBackColor;

    private GameObject m_cEffectController = null;
    private List<Image> m_cImageList = new List<Image>();



    // Start is called before the first frame update
    void Start()
    {
        // Hack : Make Chileren Class
        GameObject m_cEffectController = new GameObject();
        GameObject ImgaeObj = null;
        RectTransform CanvasRectTrans = null;
        RectTransform ImageRectTrans = null;

        //LeftBack
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eBackScreen).
            transform.GetChild((int)EResultImage.eLeftBack).gameObject;

        m_cImageList.Add(ImgaeObj.GetComponent<Image>());
        ImageRectTrans = ImgaeObj.GetComponent<RectTransform>();
        CanvasRectTrans = this.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        ImageRectTrans.localPosition -= new Vector3(CanvasRectTrans.sizeDelta.x / 2,0,0);

        //RightBack
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eBackScreen).
            transform.GetChild((int)EResultImage.eRightBack).gameObject;
        m_cImageList.Add(ImgaeObj.GetComponent<Image>());
        ImageRectTrans = ImgaeObj.GetComponent<RectTransform>();
        ImageRectTrans.localPosition += new Vector3(CanvasRectTrans.sizeDelta.x / 2, 0, 0);

        //LeftFont
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eFont).
            transform.GetChild(0).gameObject;
        m_cImageList.Add(ImgaeObj.GetComponent<Image>());

        //LeftFont
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eFont).
            transform.GetChild(1).gameObject;
        m_cImageList.Add(ImgaeObj.GetComponent<Image>());


        foreach(var val in m_cImageList)
        {
            val.sprite = null;
            val.color = Color.clear;
        }


        m_cEffectController = 
            this.gameObject.transform.GetChild((int)EResultChildObj.eEffects).gameObject;


        //勝ったほうの関数を呼ぶ
        if(GameManager.IsHumanWin)
        {
            HawkAIWin();
        }
        else
        {
            MouseWin();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        //    HawkAIWin();
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    MouseWin();
        //}

        if (GameManager.IsHumanWin)
        {
            ExecuteEvents.Execute<IEffectControllerInterface>(
            target: m_cEffectController,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.Stop((int)EResultFontEffect.eRight));

            ExecuteEvents.Execute<IEffectControllerInterface>(
            target: m_cEffectController,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.Play((int)EResultFontEffect.eLeft));

        }
        else
        {
            ExecuteEvents.Execute<IEffectControllerInterface>(
            target: m_cEffectController,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.Stop((int)EResultFontEffect.eLeft));

            ExecuteEvents.Execute<IEffectControllerInterface>(
            target: m_cEffectController,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.Play((int)EResultFontEffect.eRight));

        }
    }

    public void HawkAIWin()
    {
        //Hack : Fix Some Playing

        ExecuteEvents.Execute<IEffectControllerInterface>(
        target: m_cEffectController,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.Stop((int)EResultFontEffect.eRight));

        ExecuteEvents.Execute<IEffectControllerInterface>(
        target: m_cEffectController,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.Play((int)EResultFontEffect.eLeft));

        //Left
        GameObject ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eLeftBack).gameObject;
        m_cImageList[(int)EResultImage.eLeftBack].color = m_cLeftBackColor;
        m_cImageList[(int)EResultImage.eLeftFont].sprite = m_cWinSprite;
        m_cImageList[(int)EResultImage.eLeftFont].color = Color.white;
        m_cEffectController = this.gameObject.transform.GetChild((int)EResultChildObj.eEffects).gameObject;

    

        //Right
        ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eRightBack).gameObject;
        m_cImageList[(int)EResultImage.eRightBack].color = m_cRightBackColor;
        m_cImageList[(int)EResultImage.eRightFont].sprite = m_cLoseSprite;
        m_cImageList[(int)EResultImage.eRightFont].color = Color.white;


    }

    public void MouseWin()
    {
        //Hack : Fix Some Playing

        ExecuteEvents.Execute<IEffectControllerInterface>(
        target: m_cEffectController,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.Stop((int)EResultFontEffect.eLeft));

        ExecuteEvents.Execute<IEffectControllerInterface>(
        target: m_cEffectController,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.Play((int)EResultFontEffect.eRight));


        //Left
        GameObject ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eLeftBack).gameObject;
        m_cImageList[(int)EResultImage.eLeftBack].color = m_cLeftBackColor;
        m_cImageList[(int)EResultImage.eLeftFont].sprite = m_cLoseSprite;
        m_cImageList[(int)EResultImage.eLeftFont].color = Color.white;

        //Right
        ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eRightBack).gameObject;
        m_cImageList[(int)EResultImage.eRightBack].color = m_cRightBackColor;
        m_cImageList[(int)EResultImage.eRightFont].sprite = m_cWinSprite;
        m_cImageList[(int)EResultImage.eRightFont].color = Color.white;
    }
}
