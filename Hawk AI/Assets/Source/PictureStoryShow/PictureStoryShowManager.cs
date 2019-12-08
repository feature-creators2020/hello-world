using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GamepadInput;
using KeyBoardInput;


public enum EPictureStoryShowChild
{
    eBackGroundImage,
    eBackImage,
    eFrontImage,
    eTextImage
}


public class PictureStoryShowManager : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> SpriteList = new List<Sprite>();

    [SerializeField]
    private List<string> StringList = new List<string>();

    [SerializeField]
    private float ChangeSpriteTime;
    private float m_fCountTime = 0f;
    private float m_flerpVal = 0f;

    private int m_nNowPage = 0;
    private int m_nLastPage = 0;

    private bool m_bStateFlg = false;

    private List<Image> m_cImageList = new List<Image>();
    private List<Text> m_cTextList = new List<Text>();

    [SerializeField]
    private GameObject m_cSceneObj;

    [SerializeField]
    private GameObject m_cFadeObj;

    // Start is called before the first frame update
    void Start()
    {
        m_nLastPage = SpriteList.Count;

        for(int i = 0; i < gameObject.transform.childCount;i++)
        {
            if(gameObject.transform.GetChild(i).GetComponent<Image>() != null)
                m_cImageList.Add(gameObject.transform.GetChild(i).GetComponent<Image>());

            if (gameObject.transform.GetChild(i).GetComponent<Text>() != null)
                m_cTextList.Add(gameObject.transform.GetChild(i).GetComponent<Text>());
        }

        m_cImageList[(int)EPictureStoryShowChild.eBackImage].sprite = SpriteList[m_nNowPage + 1];
        m_cImageList[(int)EPictureStoryShowChild.eFrontImage].sprite = SpriteList[m_nNowPage];
        m_cTextList[0].text = StringList[m_nNowPage];

        ExecuteEvents.Execute<IFadeInterfase>(
        target: m_cFadeObj,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.CallFadeIn());

        
    }

    // Update is called once per frame
    void Update()
    {
        m_fCountTime += Time.deltaTime;

        if ((Input.anyKeyDown) || (m_fCountTime >= ChangeSpriteTime) ||
            (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any) || 
            KeyBoard.GetButtonDown(KeyBoard.Button.B, KeyBoard.Index.Any)))
        {

            m_fCountTime = 0f;

            if (m_nNowPage == (m_nLastPage - 1))
            {
                m_nNowPage = m_nLastPage -1;

                m_bStateFlg = true;
                ExecuteEvents.Execute<IFadeInterfase>(
                target: m_cFadeObj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.CallFadeOut());

                m_cImageList[(int)EPictureStoryShowChild.eBackImage].sprite = SpriteList[m_nNowPage];
                m_cTextList[0].text = StringList[m_nNowPage];
            }
            else
            {
                StartCoroutine(
                    SlideCoroutine(
                    new Vector3(m_cImageList[(int)EPictureStoryShowChild.eBackImage].rectTransform.rect.width, 0f, 0f),
                    Vector3.zero,
                    m_cImageList[(int)EPictureStoryShowChild.eBackImage].rectTransform)
                    );

                m_cTextList[0].text = StringList[m_nNowPage + 1];
                m_cImageList[(int)EPictureStoryShowChild.eBackImage].sprite = SpriteList[m_nNowPage + 1];

                StartCoroutine(
                    SlideCoroutine(
                    Vector3.zero,
                    new Vector3(-m_cImageList[(int)EPictureStoryShowChild.eFrontImage].rectTransform.rect.width, 0f, 0f),
                    m_cImageList[(int)EPictureStoryShowChild.eFrontImage].rectTransform)
                    );

                //m_cTextList[0].text = StringList[m_nNowPage];
                m_cImageList[(int)EPictureStoryShowChild.eFrontImage].sprite = SpriteList[m_nNowPage];

            }
            
            m_nNowPage++;

            Debug.Log("m_nNowPage : " + m_nNowPage);
        }


        if(m_bStateFlg == true)
        {
            float flerp = m_cFadeObj.GetComponent<FadeManager>().m_flerpVal;
            if (flerp >= 1f)
            {
                ExecuteEvents.Execute<ISceneInterfase>(
                target: m_cSceneObj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.Tutorial));
            }
        }
    }


    IEnumerator SlideCoroutine(Vector3 _StartAngle, Vector3 _EndAngle,RectTransform _RectTransform)
    {
        m_flerpVal = 0f;

        while (m_flerpVal <= 1f)
        {//時間補間
            m_flerpVal += Time.deltaTime / 0.5f;
            _RectTransform.localPosition
                = Vector3.Lerp(_StartAngle, _EndAngle, m_flerpVal);
            yield return null;
        }

        //強制補正
        if(_StartAngle != _EndAngle)
        {
            _RectTransform.localPosition = _EndAngle;
        }
    }
}
