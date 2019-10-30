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

    }

    public virtual void GeneralUpdate()
    {

    }

    public virtual void GeneralRelease()
    {
    }


    public virtual GameObject GetGameObject(int _ID)
    {

        return this.gameObject;
    }

    public virtual GameObject GetGameObject(string _Str)
    {

        return this.gameObject;
    }

    public virtual List<GameObject> GetGameObjectsList()
    {

        return null;
    }

    public virtual void OnDestroy()
    {
        GeneralRelease();
    }
}
