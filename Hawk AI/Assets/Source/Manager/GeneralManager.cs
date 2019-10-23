using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GeneralManager : MonoBehaviour, IGeneralInterface
{
    [SerializeField]
    protected string m_strTag;

    protected List<GameObject> m_cGameObjects = new List<GameObject>();

    public virtual void GeneralInit()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag(m_strTag))
        {
            m_cGameObjects.Add(obj);

            ExecuteEvents.Execute<IGeneralInterface>(
                target: obj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.GeneralInit());

        }
    }

    public virtual void GeneralUpdate()
    {
        foreach (var obj in m_cGameObjects)
        {
            ExecuteEvents.Execute<IGeneralInterface>(
                 target: obj,
                 eventData: null,
                 functor: (recieveTarget, y) => recieveTarget.GeneralUpdate());

        }
    }

    public virtual void GeneralRelease()
    {
        foreach (var obj in m_cGameObjects)
        {
            ExecuteEvents.Execute<IGeneralInterface>(
                 target: obj,
                 eventData: null,
                 functor: (recieveTarget, y) => recieveTarget.GeneralUpdate());

        }
    }

    public virtual void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var obj in m_cGameObjects)
            {
                Debug.Log(obj.name);
            }
        }
    }
    public virtual void OnDestroy()
    {
        GeneralRelease();

    }

    public virtual GameObject GetGameObject(int _ID)
    {
        return m_cGameObjects[_ID];
    }

    public virtual List<GameObject> GetGameObjectsList()
    {
        return m_cGameObjects;
    }
}
