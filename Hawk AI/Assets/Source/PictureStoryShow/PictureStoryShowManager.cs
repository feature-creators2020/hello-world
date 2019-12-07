using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PictureStoryShowManager : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> SpriteList = new List<Sprite>();

    [SerializeField]
    private float ChangeSpriteTime;

    private float m_fCountTime = 0f;

    private int m_nNowPage = 0;
    private int m_nLastPage = 0;

    private Image m_cImage;

    [SerializeField]
    private GameObject m_cSceneObj;

    // Start is called before the first frame update
    void Start()
    {
        m_nLastPage = SpriteList.Count;
        m_cImage = this.gameObject.transform.GetChild(0).GetComponent<Image>();
        m_cImage.sprite = SpriteList[m_nNowPage];
    }

    // Update is called once per frame
    void Update()
    {
        m_fCountTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Return) || (m_fCountTime >= ChangeSpriteTime))
        {

            m_nNowPage++;
            m_fCountTime = 0f;

            if (m_nNowPage == m_nLastPage)
            {
                m_nNowPage = m_nLastPage -1;

                ExecuteEvents.Execute<ISceneInterfase>(
                target: m_cSceneObj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.Tutorial));

            }

            m_cImage.sprite = SpriteList[m_nNowPage];

            Debug.Log("m_nNowPage : " + m_nNowPage);
        }
    }
}
