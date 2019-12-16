using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EResultCircleShadowChild
{
    eHuman1,
    eHuman2,
    eMouse1,
    eMouse2
}

public class ShadowCanvas : MonoBehaviour, IResultManagerInterfase
{
    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < this.gameObject.transform.childCount; i++)
        //{
        //    this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HawkAIWin()
    {
        this.gameObject.transform.GetChild((int)EResultCircleShadowChild.eHuman1).gameObject.SetActive(true);
        this.gameObject.transform.GetChild((int)EResultCircleShadowChild.eHuman2).gameObject.SetActive(true);
        this.gameObject.transform.GetChild((int)EResultCircleShadowChild.eMouse1).gameObject.SetActive(false);
        this.gameObject.transform.GetChild((int)EResultCircleShadowChild.eMouse2).gameObject.SetActive(false);

    }

    public void MouseWin()
    {
        this.gameObject.transform.GetChild((int)EResultCircleShadowChild.eHuman1).gameObject.SetActive(false);
        this.gameObject.transform.GetChild((int)EResultCircleShadowChild.eHuman2).gameObject.SetActive(false);
        this.gameObject.transform.GetChild((int)EResultCircleShadowChild.eMouse1).gameObject.SetActive(true);
        this.gameObject.transform.GetChild((int)EResultCircleShadowChild.eMouse2).gameObject.SetActive(true);
    }

}
