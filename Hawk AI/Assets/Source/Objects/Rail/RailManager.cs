using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IRailManager : IEventSystemHandler
{
    void ChangeFloatingDirection();
}

public class RailManager : GeneralManager, IRailManager
{

    //CAUT : 管理するオブジェクトの取得方法を変更
    public override void GeneralInit()
    {
        for(int i = 0; i < this.gameObject.transform.childCount;i++)
        {
            m_cGameObjects.Add(this.gameObject.transform.GetChild(i).gameObject);
        }

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

    public void ChangeFloatingDirection()
    {
        foreach(var val in m_cGameObjects)
        {

            ExecuteEvents.Execute<IRailInterfase>(
            target: val,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.ChangeState());

        }
    }

}
