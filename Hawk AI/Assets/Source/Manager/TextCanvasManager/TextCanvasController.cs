using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface ITextCanvasController : IEventSystemHandler
{
    void ChangeText(int _Page, float _FadeTime);
}



public class TextCanvasController : MonoBehaviour, ITextCanvasController
{
    private List<GameObject> m_cObjectList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            m_cObjectList.Add(gameObject.transform.GetChild(i).gameObject);
            m_cObjectList[i].SetActive(false);
        }

        //m_cTextList[0].text = StringList[0];

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText(int _Page,float _FadeTime)
    {
        //if(_Page == 0)
        //{
        //    m_cObjectList[_Page].SetActive(true);
        //}
        //else
        //{

        //}

        if (_Page != 0)
        {
            ExecuteEvents.Execute<ITextCanvas>(
            target: m_cObjectList[_Page - 1],
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.EndText());
        }
        else
        {
            ExecuteEvents.Execute<ITextCanvas>(
            target: m_cObjectList[_Page],
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.EndText());
        }

        if (gameObject.transform.childCount != _Page)
        {
            m_cObjectList[_Page].SetActive(true);

            ExecuteEvents.Execute<ITextCanvas>(
               target: m_cObjectList[_Page],
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.ChangeText(_FadeTime));

        }
    }
}
