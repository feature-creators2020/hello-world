using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCatchZone : MonoBehaviour
{
    public bool isCatch;                // 捕まえられるか
    public GameObject TargetObject;     // 捕まえる対象のオブジェクトを保持

    // Start is called before the first frame update
    void Start()
    {
        isCatch = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("OnTriggerStay");
        if(other.tag == "Mouse")
        {
            isCatch = true;
            this.transform.Find("Push○BottonMark").gameObject.SetActive(true);
            TargetObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mouse")
        {
            //Debug.Log("OnTriggerExit");
            isCatch = false;
            this.transform.Find("Push○BottonMark").gameObject.SetActive(false);
            TargetObject = null;
        }
    }

}
