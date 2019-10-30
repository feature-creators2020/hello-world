using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EObjectType
{
    eTestObject,
}

public class ObjectManager : GeneralManager
{
    // Start is called before the first frame update
    public override void GeneralInit()
    {
        base.GeneralInit();

        //GameObject obj = GameObject.Find("PlayerCamera 3");
        //obj.GetComponent<DepthTexture>().Initialize();
    }

    // Update is called once per frame
    public override void GeneralUpdate()
    {
        base.GeneralUpdate();
        DebugUpdate();
        //GameObject obj = GameObject.Find("PlayerCamera 3");
        //obj.GetComponent<DepthTexture>().SetMaterialProperties();

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

    public override List<GameObject> GetGameObjectsList()
    {
        return base.GetGameObjectsList();
    }
}
