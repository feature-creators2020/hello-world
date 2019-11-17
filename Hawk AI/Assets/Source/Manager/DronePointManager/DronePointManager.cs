using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePointManager : GeneralManager
{

    public override void GeneralInit()
    {
        base.GeneralInit();

    }

    // Update is called once per frame
    public override void GeneralUpdate()
    {
        base.GeneralUpdate();
        DebugUpdate();
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

    public virtual void Destroy(GameObject _object)
    {
        m_cGameObjects.Remove(_object);
    }

}
