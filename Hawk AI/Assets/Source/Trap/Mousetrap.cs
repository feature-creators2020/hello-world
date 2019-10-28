using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mousetrap : GeneralObject
{


    public override void GeneralInit()
    {
        
    }

    public override void GeneralRelease()
    {
        var itemmanager = ManagerObjectManager.Instance.GetGameObject("ItemManager");
        var mousetrapmanager = itemmanager.GetComponent<ItemManager>().GetGameObject("MousetrapManager");
        mousetrapmanager.GetComponent<MousetrapManager>().Destroy(this.gameObject);
    }

}
