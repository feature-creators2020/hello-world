using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHumanMassage : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_lgMessage = new List<GameObject>();
    private List<int> m_liPlayedIndex = new List<int>();
    private List<int> m_liWaitingForPlayback = new List<int>();
    private int m_iPlaying = -1;
    [SerializeField]
    private AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        ShowHumanMessage(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_iPlaying > -1)
        {

        }
        else if(m_liWaitingForPlayback.Count > 0)
        {
            ShowHumanMessage(m_liWaitingForPlayback[0]);
            m_liWaitingForPlayback.RemoveAt(0);
        }
    }

    public void ShowHumanMessage(int MessageIndex)
    {
        if(m_iPlaying > -1)
        {
            m_liWaitingForPlayback.Add(MessageIndex);
        }
        else if(m_liPlayedIndex.IndexOf(MessageIndex) == -1)
        {
            m_iPlaying = MessageIndex;
            m_liPlayedIndex.Add(MessageIndex);
        }
    }
}
