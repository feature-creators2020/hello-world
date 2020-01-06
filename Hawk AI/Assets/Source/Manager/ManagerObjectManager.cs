using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Hack : child object list
public enum EManagerObject
{

    eSTOCK,
    eTIME,
    eMAP,
    eOBJECT,
    eSCENE,
    eFADE,
    eGAME,
};

public class ManagerObjectManager : SingletonMonoBehaviour<ManagerObjectManager>,IGeneralInterface
{

    [SerializeField]
    protected string m_strTag;

    protected List<GameObject> m_cGameObjects = new List<GameObject>();

    // Start is called before the first frame update
    public virtual void Start()
    {
        GeneralInit();
    }

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

    // Update is called once per frame
    public virtual void Update()
    {
        GeneralUpdate();

        DebugUpdate();

        
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

    public virtual void OnDestroy()
    {
        GeneralRelease();
       
    }

    public virtual void DebugUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    foreach (var obj in m_cGameObjects)
        //    {
        //        Debug.Log(obj.name);
        //    }
        //}
    }
    public virtual GameObject GetGameObject(int _ID)
    {
        return m_cGameObjects[_ID];
    }

    public virtual GameObject GetGameObject(string _Str)
    {

        GameObject obj = null;

        foreach (var val in m_cGameObjects)
        {
            if (val.name == _Str)
            {
                obj = val;
                break;
            }
        }

        return obj;
    }

    public virtual List<GameObject> GetGameObjectsList()
    {
        return m_cGameObjects;
    }


}