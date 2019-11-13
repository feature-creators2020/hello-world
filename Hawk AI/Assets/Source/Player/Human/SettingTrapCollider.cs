using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISettingTrapCollider : IEventSystemHandler
{
    bool GetHitFlg();

}

public class SettingTrapCollider : MonoBehaviour,ISettingTrapCollider
{
    private GameObject m_cPlayerObj;
    private BoxCollider m_cCollider;
    private bool bHitFlg = new bool();
    //public bool bHitFlg
    //{
    //    get { return bHitFlg; }
    //    set { bHitFlg = value; }
    //}

    // Start is called before the first frame update
    void Start()
    {
        m_cPlayerObj = this.gameObject.transform.parent.parent.gameObject;
        m_cCollider = this.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //SetMyPosition(m_cPlayerObj.transform.position);
    }

    void SetMyPosition(Vector3 pos)
    {
        this.gameObject.transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("MapColliderBox"))
        {
            Debug.Log("OnTriggerEnter : " + other.gameObject.name);
            bHitFlg = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("MapColliderBox"))
        {
            Debug.Log("OnTriggerExit : " + other.gameObject.name);

            bHitFlg = false;
        }
    }

    public bool GetHitFlg()
    {
        return bHitFlg;
    }

}
