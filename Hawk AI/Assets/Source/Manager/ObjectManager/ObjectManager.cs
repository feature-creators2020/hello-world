using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ObjectManager : GeneralManager
{
    // Start is called before the first frame update
    public override void GeneralInit()
    {
        base.GeneralInit();
    }

    // Update is called once per frame
    public override void GeneralUpdate()
    {
        base.GeneralUpdate();
    }

    public override void GeneralRelease()
    {
        base.GeneralRelease();
    }

    public override void DebugUpdate()
    {
        base.DebugUpdate();
    }

    public override GameObject GetGameObject(int _ID)
    {
        return base.GetGameObject(_ID);
    }
}
