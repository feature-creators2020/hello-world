using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IGeneralInterface : IEventSystemHandler
{
    void GeneralInit();
    void GeneralUpdate();
    void GeneralRelease();

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

}
