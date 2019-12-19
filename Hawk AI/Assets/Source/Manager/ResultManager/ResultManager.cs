
//#define _Beta_Result
#define _Master_Result

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GamepadInput;
using KeyBoardInput;



public interface IResultManagerInterfase : IEventSystemHandler
{
    void HawkAIWin();
    void MouseWin();
    void SetIsOk(int id, bool isVal);

}

#if _Beta_Result

public enum EResultChildObj
{
    eBackScreen,
    eFont,
    eWinEffects,
    eFloorSpotLights,
    ePlayerModel,
    eFloorModel,
    eLoseEffects,
}

public enum EResultImage
{
    eLeftBack,
    eRightBack,
    eLeftFont,
    eRightFont
}

public enum ESpotLightImage
{
    eListPullNo_Left,
    eListPullNo_Right,
    eLeftSpotLightImage,
    eRightSpotLightImage
}

public enum EFloorSpotLightObject
{
    eLeft,
    eRight,
}
#endif //_Beta_Result

public class ResultManager : MonoBehaviour, IResultManagerInterfase
{

#if _Beta_Result
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
    private List<GameObject> m_cSpotLightList = new List<GameObject>();
    private List<GameObject> m_cFloorSpotLightList = new List<GameObject>();

#endif //_Beta_Result

    [SerializeField]
    private GameObject[] HumanObjects;
    [SerializeField]
    private GameObject[] MouseObjects;

    [SerializeField]
    private List<GameObject> PlayerRecordsUI = new List<GameObject>();

    private int PhaseCount = 0;
    private bool[] m_bOKFlg = new bool[4];
    private bool[] m_bNotDoubleTapFlg = new bool[4];

    // Start is called before the first frame update
    void Start()
    {
#if _Beta_Result

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

        //LeftSpotLight
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eBackScreen).
                    transform.GetChild((int)ESpotLightImage.eLeftSpotLightImage).gameObject;
        m_cSpotLightList.Add(ImgaeObj);
        ImgaeObj.SetActive(false);

        //RightSpotLight
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eBackScreen).
                    transform.GetChild((int)ESpotLightImage.eRightSpotLightImage).gameObject;
        m_cSpotLightList.Add(ImgaeObj);
        ImgaeObj.SetActive(false);

        //LeftFont
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eFont).
            transform.GetChild(0).gameObject;
        m_cImageList.Add(ImgaeObj.GetComponent<Image>());

        //LeftFont
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eFont).
            transform.GetChild(1).gameObject;
        m_cImageList.Add(ImgaeObj.GetComponent<Image>());


        //LeftFloorSpotLight
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eFloorSpotLights).
                    transform.GetChild((int)EFloorSpotLightObject.eLeft).gameObject;
        m_cFloorSpotLightList.Add(ImgaeObj);
        ImgaeObj.SetActive(false);

        //RightFloorSpotLight
        ImgaeObj = this.gameObject.transform.GetChild((int)EResultChildObj.eFloorSpotLights).
                    transform.GetChild((int)EFloorSpotLightObject.eRight).gameObject;
        m_cFloorSpotLightList.Add(ImgaeObj);
        ImgaeObj.SetActive(false);


        foreach (var val in m_cImageList)
        {
            val.sprite = null;
            val.color = Color.clear;
        }


        m_cEffectController = 
            this.gameObject.transform.GetChild((int)EResultChildObj.eWinEffects).gameObject;
        m_cEffectController.GetComponent<ResultFontEffect>().CallStart();

#endif //_Beta_Result


        //勝ったほうの関数を呼ぶ
        if (GameManager.IsHumanWin)
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
        switch (PhaseCount)
        {
            case 0:

                Phase_0();
                
                break;

            case 1:
                Phase_1();

                break;



        }
    }


    private void Phase_0()
    {

        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any) ||
              KeyBoard.GetButtonDown(KeyBoard.Button.B, KeyBoard.Index.Any))
        {

            foreach (var val in PlayerRecordsUI)
            {
                val.SetActive(true);
            }

            if (GameManager.IsHumanWin)
            {
                ExecuteEvents.Execute<IResultManagerInterfase>(
                target: PlayerRecordsUI[0],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.HawkAIWin());

            }
            else
            {
                ExecuteEvents.Execute<IResultManagerInterfase>(
                target: PlayerRecordsUI[0],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.MouseWin());
            }

            PhaseCount++;
        }
    }

    private void Phase_1()
    {
        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.One) ||
             KeyBoard.GetButtonDown(KeyBoard.Button.B, KeyBoard.Index.One))
        {
            if (m_bNotDoubleTapFlg[0] == false)
            {
                m_bNotDoubleTapFlg[0] = true;
                ExecuteEvents.Execute<IRecordScreenCanvas>(
                target: PlayerRecordsUI[0],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.ChangeRecordScreen(0));

            }
        }

        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Two) ||
            KeyBoard.GetButtonDown(KeyBoard.Button.B, KeyBoard.Index.Two))
        {
            if (m_bNotDoubleTapFlg[1] == false)
            {
                m_bNotDoubleTapFlg[1] = true;

                ExecuteEvents.Execute<RecordScreenCanvas>(
                target: PlayerRecordsUI[0],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.ChangeRecordScreen(1));
            }

        }

        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Three) ||
            KeyBoard.GetButtonDown(KeyBoard.Button.B, KeyBoard.Index.Three))
        {
            if (m_bNotDoubleTapFlg[2] == false)
            {
                m_bNotDoubleTapFlg[2] = true;

                ExecuteEvents.Execute<IRecordScreenCanvas>(
                target: PlayerRecordsUI[0],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.ChangeRecordScreen(2));
            }
        }

        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Four) ||
             KeyBoard.GetButtonDown(KeyBoard.Button.B, KeyBoard.Index.Four))
        {
            if (m_bNotDoubleTapFlg[3] == false)
            {
                m_bNotDoubleTapFlg[3] = true;

                ExecuteEvents.Execute<IRecordScreenCanvas>(
                target: PlayerRecordsUI[0],
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.ChangeRecordScreen(3));
            }
        }


        foreach (var val in m_bOKFlg)
        {
            if (val == false)
                return;
        }

        var obj = ManagerObjectManager.Instance.GetGameObject("GameManager");

        ExecuteEvents.Execute<IGameInterface>(
        target: obj,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.ChangeState(EGameState.End));

    }


    public void HawkAIWin()
    {

#if _Beta_Result

        GameObject LoseEffectObj = gameObject.transform.GetChild((int)EResultChildObj.eLoseEffects).gameObject;
        m_cEffectController = this.gameObject.transform.GetChild((int)EResultChildObj.eWinEffects).gameObject;

        //Left is Winner
        GameObject ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eLeftBack).gameObject;
        m_cImageList[(int)EResultImage.eLeftBack].color = m_cLeftBackColor;
        m_cImageList[(int)EResultImage.eLeftFont].sprite = m_cWinSprite;
        m_cImageList[(int)EResultImage.eLeftFont].color = Color.white;
        m_cSpotLightList[(int)ESpotLightImage.eListPullNo_Left].SetActive(true);
        m_cFloorSpotLightList[(int)EFloorSpotLightObject.eLeft].SetActive(true);

        // Back Light Effects
        ExecuteEvents.Execute<IEffectControllerInterface>(
        target: m_cEffectController,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.Play((int)EResultFontEffect.eLeft));

        //Right is Loser
        ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eRightBack).gameObject;
        m_cImageList[(int)EResultImage.eRightBack].color = m_cRightBackColor;
        m_cImageList[(int)EResultImage.eRightFont].sprite = m_cLoseSprite;
        m_cImageList[(int)EResultImage.eRightFont].color = Color.white;

        m_cImageList[(int)EResultImage.eRightFont].rectTransform.localScale = new Vector3(
            m_cImageList[(int)EResultImage.eRightFont].rectTransform.localScale.x / 2,
            m_cImageList[(int)EResultImage.eRightFont].rectTransform.localScale.y / 2,
            m_cImageList[(int)EResultImage.eRightFont].rectTransform.localScale.z);


        ExecuteEvents.Execute<IEffectControllerInterface>(
        target: m_cEffectController,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.Stop((int)EResultFontEffect.eRight));

        //ExecuteEvents.Execute<ILoseEffectInterface>(
        //target: LoseEffectObj,
        //eventData: null,
        //functor: (recieveTarget, y) => recieveTarget.PlayEffects(ELoseSide.eRight,ELoseEffectsType.eWind));

        //ExecuteEvents.Execute<ILoseEffectInterface>(
        //target: LoseEffectObj,
        //eventData: null,
        //functor: (recieveTarget, y) => recieveTarget.PlayEffects(ELoseSide.eRight, ELoseEffectsType.eGaan));



#endif //_Beta_Result


        for (int i = 0; i < gameObject.transform.GetChild(1).gameObject.transform.childCount;i++)
        {
            ExecuteEvents.Execute<IResultManagerInterfase>(
            target: gameObject.transform.GetChild(1).gameObject.transform.GetChild(i).gameObject,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.HawkAIWin());

        }


        foreach (var val in HumanObjects)
        {
            ExecuteEvents.Execute<IResultHumanInterfase>(
            target: val,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.PlayWin());
        }

        foreach (var val in MouseObjects)
        {
            ExecuteEvents.Execute<IResultMouseInterfase>(
            target: val,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.PlayLose());
        }
    }

    public void MouseWin()
    {

#if _Beta_Result

        GameObject LoseEffectObj = gameObject.transform.GetChild((int)EResultChildObj.eLoseEffects).gameObject;
        m_cEffectController = this.gameObject.transform.GetChild((int)EResultChildObj.eWinEffects).gameObject;

        //Left is Loser
        GameObject ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eLeftBack).gameObject;
        m_cImageList[(int)EResultImage.eLeftBack].color = m_cLeftBackColor;
        m_cImageList[(int)EResultImage.eLeftFont].sprite = m_cLoseSprite;
        m_cImageList[(int)EResultImage.eLeftFont].color = Color.white;

        m_cImageList[(int)EResultImage.eLeftFont].rectTransform.localScale = new Vector3(
            m_cImageList[(int)EResultImage.eLeftFont].rectTransform.localScale.x / 2,
            m_cImageList[(int)EResultImage.eLeftFont].rectTransform.localScale.y / 2,
            m_cImageList[(int)EResultImage.eLeftFont].rectTransform.localScale.z);



        ExecuteEvents.Execute<IEffectControllerInterface>(
        target: m_cEffectController,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.Stop((int)EResultFontEffect.eLeft));

        

        //ExecuteEvents.Execute<ILoseEffectInterface>(
        //target: LoseEffectObj,
        //eventData: null,
        //functor: (recieveTarget, y) => recieveTarget.PlayEffects(ELoseSide.eLeft, ELoseEffectsType.eWind));

        //ExecuteEvents.Execute<ILoseEffectInterface>(
        //target: LoseEffectObj,
        //eventData: null,
        //functor: (recieveTarget, y) => recieveTarget.PlayEffects(ELoseSide.eLeft, ELoseEffectsType.eGaan));


        //Right is Winner
        ImgaeObj = this.gameObject.transform.GetChild(0).transform.GetChild((int)EResultImage.eRightBack).gameObject;
        m_cImageList[(int)EResultImage.eRightBack].color = m_cRightBackColor;
        m_cImageList[(int)EResultImage.eRightFont].sprite = m_cWinSprite;
        m_cImageList[(int)EResultImage.eRightFont].color = Color.white;
        m_cSpotLightList[(int)ESpotLightImage.eListPullNo_Right].SetActive(true);
        m_cFloorSpotLightList[(int)EFloorSpotLightObject.eRight].SetActive(true);

        ExecuteEvents.Execute<IEffectControllerInterface>(
        target: m_cEffectController,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.Play((int)EResultFontEffect.eRight));



#endif //_Beta_Result

        for (int i = 0; i < gameObject.transform.GetChild(1).gameObject.transform.childCount; i++)
        {
            ExecuteEvents.Execute<IResultManagerInterfase>(
            target: gameObject.transform.GetChild(1).gameObject.transform.GetChild(i).gameObject,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.MouseWin());

        }

        foreach (var val in HumanObjects)
        {
            ExecuteEvents.Execute<IResultHumanInterfase>(
            target: val,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.PlayLose());
        }

        foreach (var val in MouseObjects)
        {
            ExecuteEvents.Execute<IResultMouseInterfase>(
            target: val,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.PlayWin());
        }
    }

    public void SetIsOk(int id,bool isVal)
    {
        m_bOKFlg[id] = isVal;
    }
}
