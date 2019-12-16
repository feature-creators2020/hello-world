using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IItemObjectInterface : IEventSystemHandler
{
    void SetPoint(GameObject _point);
}


public class Item : MonoBehaviour, IItemObjectInterface
{
    bool isLimitScale = false;
    Vector3 keepScale;
    GameObject m_gPointObject;
    [SerializeField] GameObject m_gItemManager;

    // Start is called before the first frame update
    void Start()
    {
        keepScale = this.transform.localScale;
        this.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        isLimitScale = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0f, 1f, 0f);
        if (!isLimitScale)
        {
            ScaleUp();
        }
    }

    void ScaleUp()
    {
        this.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        if (this.transform.localScale.x >= keepScale.x)
        {
            isLimitScale = true;
            ExecuteEvents.Execute<IDronePointInterface>(
                        target: m_gPointObject,
                        eventData: null,
                        functor: (recieveTarget, y) => recieveTarget.SetItem(this.gameObject));
        }
    }

    public void SetPoint(GameObject _point)
    {
        m_gPointObject = _point;
    }

    void OnEnable()
    {
        //ExecuteEvents.Execute<IDronePointInterface>(
        //            target: m_gPointObject,
        //            eventData: null,
        //            functor: (recieveTarget, y) => recieveTarget.SetItem(this.gameObject));

        // アイテムマネージャーから自身のアイテムマネージャーを取得し、リストに追加する
        //var manager = ManagerObjectManager.Instance.GetGameObject("ItemManager");
        var itemmanager = m_gItemManager.GetComponent<ItemManager>();
        string managertag = null;
        foreach (var val in itemmanager.GetGameObjectsList())
        {

            ExecuteEvents.Execute<IItemInterface>(
                target: val,
                eventData: null,
                functor: (recieveTarget, y) => managertag = recieveTarget.GetTag());
            if(managertag == this.gameObject.transform.parent.gameObject.tag)
            {
                ExecuteEvents.Execute<IGeneralInterface>(
                    target: val,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.GetGameObjectsList().Add(this.gameObject.transform.parent.gameObject));
            }
        }

    }

    void OnDestroy()
    {
        ExecuteEvents.Execute<IDronePointInterface>(
                    target: m_gPointObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.DelItem());
    }
}
