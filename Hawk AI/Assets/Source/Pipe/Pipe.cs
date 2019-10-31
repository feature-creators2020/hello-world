using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EPipeState
{
    eLerpTransPosition,
    eMesh,
    eCollision,
}

public interface IPipeInterfase : IEventSystemHandler
{
    List<GameObject> GetPipeObjects { get; }
    List<GameObject> GetInversePipeObjects { get; }
}


public class Pipe : MonoBehaviour, IPipeInterfase
{
    private List<GameObject> m_cTransFormPositionObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> GetPipeObjects
    {
        get {
            m_cTransFormPositionObj = new List<GameObject>();

            int childCount = gameObject.transform.GetChild((int)EPipeState.eLerpTransPosition).gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                m_cTransFormPositionObj.Add(gameObject.transform.GetChild((int)EPipeState.eLerpTransPosition).gameObject.transform.GetChild(i).gameObject);
            }

            return m_cTransFormPositionObj;

        }
    }

    public List<GameObject> GetInversePipeObjects
    {
        get
        {
            m_cTransFormPositionObj = new List<GameObject>();

            int childCount = gameObject.transform.GetChild((int)EPipeState.eLerpTransPosition).gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                m_cTransFormPositionObj.Add(gameObject.transform.GetChild((int)EPipeState.eLerpTransPosition).gameObject.transform.GetChild(i).gameObject);
            }

            m_cTransFormPositionObj.Reverse();

            return m_cTransFormPositionObj;

        }
    }

}
