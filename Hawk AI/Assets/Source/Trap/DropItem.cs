using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : GeneralObject
{

    public GameObject m_gItemInfo;

    RaycastHit hit;
    Ray ray;

    public override void GeneralInit()
    {

    }

    void OnEnable()
    {
        var dropitemmanager = ManagerObjectManager.Instance.GetGameObject("DropItemManager");
        dropitemmanager.GetComponent<DropItemManager>().GetGameObjectsList().Add(this.gameObject);
    }

    public override void GeneralUpdate()
    {
        Debug.Log("GeneralUpdate");
        ray = new Ray(this.transform.position, -this.transform.up);
        Debug.DrawLine(this.transform.position, this.transform.position - this.transform.up * 0.5f, Color.red);
        if (Physics.Raycast(ray, out hit, 0.5f))
        {
            var LayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            var TagName = hit.collider.gameObject.tag;

            if (TagName != "Room")
            {
                Debug.Log("otherobject : " + hit.collider.gameObject.name);
                if (m_gItemInfo != null)
                {
                    var col = this.gameObject.GetComponent<BoxCollider>();
                    col.enabled = false;
                    Debug.Log("Instantiate : " + m_gItemInfo.name);
                    Instantiate(m_gItemInfo, this.gameObject.transform.position, this.transform.rotation);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    //void Update()
    //{
    //    ray = new Ray(this.transform.position, -this.transform.up);
    //    Debug.DrawLine(this.transform.position, this.transform.position - this.transform.up * 0.5f, Color.red);
    //    if (Physics.Raycast(ray, out hit, 0.5f))
    //    {
    //        var LayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
    //        var TagName = hit.collider.gameObject.tag;

    //        if (TagName != "Room")
    //        {
    //            Debug.Log("otherobject : " + hit.collider.gameObject.name);
    //            if (m_gItemInfo != null)
    //            {
    //                var col = this.gameObject.GetComponent<BoxCollider>();
    //                col.enabled = false;
    //                Instantiate(m_gItemInfo, this.gameObject.transform);
    //                Destroy(this.gameObject);
    //            }
    //        }
    //    }
    //}

    public void SetItem(GameObject item)
    {
        m_gItemInfo = item;
    }

    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag != "Room")
    //    {
    //        Debug.Log("otherobject : " + other.gameObject.name);
    //        if (m_gItemInfo != null)
    //        {
    //            var col = this.gameObject.GetComponent<BoxCollider>();
    //            col.enabled = false;
    //            Instantiate(m_gItemInfo, this.gameObject.transform);
    //            Destroy(this.gameObject);
    //        }
    //    }
    //}

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag != "Room")
    //    {
    //        Debug.Log("otherobject : " + other.gameObject.name);
    //        if (m_gItemInfo != null)
    //        {
    //            Destroy(this.gameObject);
    //            Instantiate(m_gItemInfo, this.transform);
    //        }
    //    }
    //}

}
