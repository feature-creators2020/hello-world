using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDronePointInterface : IEventSystemHandler
{
    GameObject GetGameObject();
    bool IsDrop();
    void SetItem(GameObject _item);
    void DelItem();
}

public class DronePoint : MonoBehaviour, IDronePointInterface
{
    // 保持しているアイテム情報
    GameObject m_gItemObject;


    // Start is called before the first frame update
    void Start()
    {
        m_gItemObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.gameObject.name + ".ItemInfo : " + m_gItemObject);
    }

    public GameObject GetGameObject()
    {
        return m_gItemObject;
    }

    public bool IsDrop()
    {
        // すでに地点にアイテムが存在しているか
        if (ReferenceEquals(m_gItemObject, null))
        {
            // 存在していないため落とせる
            return true;
        }
        // 存在しているので落とせない
        return false;
    }

    public void SetItem(GameObject _item)
    {
        m_gItemObject = _item;
    }

    public void DelItem()
    {
        // アイテムの情報を消す
        m_gItemObject = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            SetItem(other.gameObject);
        }
    }

    void OnTriggerEnd(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            if (m_gItemObject == other.gameObject)
            {
                DelItem();
            }
        }
    }
}
