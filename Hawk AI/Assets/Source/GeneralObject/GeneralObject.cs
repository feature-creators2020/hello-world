using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IGeneralInterface : IEventSystemHandler
{
    void GeneralInit();
    void GeneralUpdate();
    void GeneralRelease();
    GameObject GetGameObject(int _ID);
    GameObject GetGameObject(string _Str);
    List<GameObject> GetGameObjectsList();
}

public class GeneralObject : MonoBehaviour, IGeneralInterface
{
    public virtual void GeneralInit()
    {
        Debug.Log("GeneralInit");
    }

    public virtual void GeneralUpdate()
    {

    }

    public virtual void GeneralRelease()
    {
        Debug.Log("GeneralRelease");
    }


    public virtual GameObject GetGameObject(int _ID)
    {
        Debug.Log("GetGameObject");

        return this.gameObject;
    }

    public virtual GameObject GetGameObject(string _Str)
    {
        Debug.Log("GetGameObject");

        return this.gameObject;
    }

    public virtual List<GameObject> GetGameObjectsList()
    {
        Debug.Log("GetGameObjectsList");

        return null;
    }

    public virtual void OnDestroy()
    {
        GeneralRelease();
    }
}
