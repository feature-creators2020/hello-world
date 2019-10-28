using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mousetrap : GeneralObject
{

    bool isRide = false;
    GameObject TargetObject;

    public override void GeneralInit()
    {
        isRide = false;
    }

    public override void GeneralRelease()
    {
        var itemmanager = ManagerObjectManager.Instance.GetGameObject("ItemManager");
        var mousetrapmanager = itemmanager.GetComponent<ItemManager>().GetGameObject("MousetrapManager");
        mousetrapmanager.GetComponent<MousetrapManager>().Destroy(this.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (TargetObject == null)
            {
                TargetObject = other.gameObject;
                isRide = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (TargetObject == other.gameObject)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
