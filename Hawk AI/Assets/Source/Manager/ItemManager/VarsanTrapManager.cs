using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarsanTrapManager : GeneralManager, IItemInterface
{
    [SerializeField]
    protected GameObject PrefabObject;


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

    public virtual void Instant(Transform _transform)
    {
        Vector3 setpos = new Vector3(_transform.position.x, _transform.position.y, _transform.position.z);
        //Debug.Log("Position : " + setpos);
        var Object = Instantiate(PrefabObject, setpos, _transform.rotation);
        m_cGameObjects.Add(Object);
    }
    public virtual void Instant(Vector3 _pos, Quaternion _qua)
    {
        Vector3 setpos = _pos;
        //Debug.Log("Position : " + setpos);
        var Object = Instantiate(PrefabObject, setpos, _qua);
        m_cGameObjects.Add(Object);
    }


    public virtual void Destroy(GameObject _object)
    {
        m_cGameObjects.Remove(_object);
    }

    public virtual GameObject GetPrefab()
    {
        return PrefabObject;
    }

    public virtual string GetTag()
    {
        return m_strTag;
    }
}
