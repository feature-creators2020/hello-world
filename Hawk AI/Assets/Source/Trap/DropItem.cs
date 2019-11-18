using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : GeneralObject
{

    public GameObject m_gItemInfo;

    RaycastHit hit;
    Ray ray;
    bool isGround = false;
    Rigidbody m_crigidbody;

    public override void GeneralInit()
    {

    }

    void OnEnable()
    {
        var dropitemmanager = ManagerObjectManager.Instance.GetGameObject("DropItemManager");
        dropitemmanager.GetComponent<DropItemManager>().GetGameObjectsList().Add(this.gameObject);
        m_crigidbody = this.gameObject.GetComponent<Rigidbody>();
        Debug.Log(m_crigidbody);
    }

    public override void GeneralUpdate()
    {
        Debug.Log("GeneralUpdate");

        if (isGround == false)
        {
            HitCheck();
        }
        else
        {

            // 着地時の処理
            this.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            // 一定のサイズより小さくなると消す
            if (this.transform.localScale.x <= 0.1f)
            {
                CreateItem();
                Destroy(this.gameObject);
            }
        }

    }

    public void SetItem(GameObject item)
    {
        m_gItemInfo = item;
    }

    void HitCheck()
    {
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
                    isGround = true;
                    // 当たり判定を消す
                    var col = this.gameObject.GetComponent<BoxCollider>();
                    col.enabled = false;
                    // 重力処理をなくす
                    m_crigidbody.useGravity = false;
                    m_crigidbody.velocity = Vector3.zero;
                    m_crigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    //Destroy(this.gameObject);
                }
            }
        }

    }

    void CreateItem()
    {
        Debug.Log("Instantiate : " + m_gItemInfo.name);
        Instantiate(m_gItemInfo, this.gameObject.transform.position, this.transform.rotation);
    }
}
