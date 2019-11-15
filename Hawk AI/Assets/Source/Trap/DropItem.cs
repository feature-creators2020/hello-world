using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : GeneralObject
{

    public GameObject m_gItemInfo;

    public override void GeneralInit()
    {

    }

    public void SetItem(GameObject item)
    {
        m_gItemInfo = item;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Room")
        {
            Debug.Log("otherobject : " + other.gameObject.name);
            if (m_gItemInfo != null)
            {
                var col = this.gameObject.GetComponent<BoxCollider>();
                col.enabled = false;
                Instantiate(m_gItemInfo);
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Room")
        {
            Debug.Log("otherobject : " + other.gameObject.name);
            if (m_gItemInfo != null)
            {
                Destroy(this.gameObject);
                Instantiate(m_gItemInfo, this.transform);
            }
        }
    }

}
