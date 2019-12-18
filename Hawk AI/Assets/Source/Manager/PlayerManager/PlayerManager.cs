using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : GeneralManager
{
    [SerializeField]
    protected string m_strTag2;
    protected List<GameObject> m_cGameObjects2 = new List<GameObject>();

    // Start is called before the first frame update
    public override void GeneralInit()
    {
        base.GeneralInit();
        foreach (var obj in GameObject.FindGameObjectsWithTag(m_strTag2))
        {
            m_cGameObjects2.Add(obj);

            ExecuteEvents.Execute<IGeneralInterface>(
                target: obj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.GeneralInit());

        }
    }

    // Update is called once per frame
    public override void GeneralUpdate()
    {
        base.GeneralUpdate();
        base.DebugUpdate();
        foreach (var obj in m_cGameObjects2)
        {
            ExecuteEvents.Execute<IGeneralInterface>(
                 target: obj,
                 eventData: null,
                 functor: (recieveTarget, y) => recieveTarget.GeneralUpdate());

        }
    }

    public override void GeneralRelease()
    {
        base.GeneralRelease();
        foreach (var obj in m_cGameObjects2)
        {
            ExecuteEvents.Execute<IGeneralInterface>(
                 target: obj,
                 eventData: null,
                 functor: (recieveTarget, y) => recieveTarget.GeneralRelease());

        }
    }

    public override void DebugUpdate()
    {
        base.DebugUpdate();
    }

    public GameObject GetGameObject(int _ID, string strTag)
    {
        if (strTag == m_strTag)
        {
            return base.GetGameObject(_ID);
        }
        else
        {
            return m_cGameObjects2[_ID];
        }
    }

    public override GameObject GetGameObject(string _Str)
    {
        //Debug.Log(_Str);
        GameObject obj = null;
        if (_Str == m_strTag)
        {
            foreach (var val in m_cGameObjects)
            {
                //Debug.Log(gameObject.name + "->name : " + val.name);
                if (val.name == _Str)
                {
                    obj = val;
                    break;
                }
            }
        }
        else
        {
            foreach (var val in m_cGameObjects2)
            {
                //Debug.Log(gameObject.name + "->name : " + val.name);
                if (val.name == _Str)
                {
                    obj = val;
                    break;
                }
            }

        }
        return obj;
    }

    public List<GameObject> GetGameObjectsList(string strTag)
    {
        if (strTag == m_strTag)
        {
            return m_cGameObjects;
            //Debug.Log(m_cGameObjects.ToString() + "->tag1");
        }
        else
        {
            return m_cGameObjects2;
            //Debug.Log(m_cGameObjects2.ToString() + "->tag2");
        }
    }

    public int GetPlayerNum(GameObject _object)
    {
        int playerNum = 99;

        // プレイヤー種類判定
        if(_object.tag == "Human")
        {
            // 人で検索
            for (int i = 0; i < GetGameObjectsList("Human").Count; i++)
            {
                if (_object == GetGameObject(i, "Human"))
                {
                    playerNum = i;
                    break;
                }
            }
        }
        else if(_object.tag == "Mouse")
        {
            // ネズミで検索
            for (int i = 0; i < GetGameObjectsList("Mouse").Count; i++)
            {
                if (_object == GetGameObject(i, "Mouse"))
                {
                    playerNum = i;
                    break;
                }
            }
        }
        else
        {
            // どちらでもない
            playerNum = 99;
        }

        return playerNum;
    }
}
