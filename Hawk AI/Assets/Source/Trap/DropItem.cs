using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : GeneralObject
{

    public GameObject m_gItemInfo;

    RaycastHit hit;
    Ray ray;
    bool isGround = false;      // 地面に接地している
    bool isInstanced = false;   // アイテムを生成したか
    Rigidbody m_crigidbody;
    GameObject m_gPointObject;
    GameObject m_gDroneObject;  // このオブジェクトが消えるタイミングでドローンのステートを切り替えるため

    public override void GeneralInit()
    {

    }

    void OnEnable()
    {
        var dropitemmanager = ManagerObjectManager.Instance.GetGameObject("DropItemManager");
        dropitemmanager.GetComponent<DropItemManager>().GetGameObjectsList().Add(this.gameObject);
        m_crigidbody = this.gameObject.GetComponent<Rigidbody>();
        //Debug.Log(m_crigidbody);
    }

    public override void GeneralUpdate()
    {
        //Debug.Log("GeneralUpdate");

        if (isInstanced)
        {
            Destroy(this.gameObject);
        }
        else
        {
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

                }
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
        Debug.DrawLine(this.transform.position, this.transform.position - this.transform.up , Color.red);
        if (Physics.Raycast(ray, out hit, 1.0f))
        {
            var LayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            var TagName = hit.collider.gameObject.tag;

            if (TagName != "Room")
            {
                //Debug.Log("otherobject : " + hit.collider.gameObject.name);
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
        //Debug.Log("Instantiate : " + m_gItemInfo.name);
        var item = Instantiate(m_gItemInfo, this.gameObject.transform.position, this.transform.rotation);
        ExecuteEvents.Execute<IItemObjectInterface>(
            target: item.transform.Find("ItemImage").gameObject,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.SetPoint(m_gPointObject));
        ExecuteEvents.Execute<IDroneInterface>(
            target: m_gDroneObject,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.ChangeMoveState());
        isInstanced = true;
        //CursorManager.Instance.SetItem(item);
    }

    public void SetPoint(GameObject _point)
    {
        m_gPointObject = _point;
    }

    public void SetDroneObject(GameObject _drone)
    {
        m_gDroneObject = _drone;
    }
}
