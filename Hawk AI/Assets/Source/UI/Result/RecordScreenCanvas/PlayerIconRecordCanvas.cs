using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerIconRecordCanvas : MonoBehaviour, IActivatePlayerRecordImage
{

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < this.gameObject.transform.childCount; i++)
        //{
        //    ImageObj.Add(this.gameObject.transform.GetChild(i).gameObject);
        //    ImageObj[i].SetActive(false);
        //}
    }
    public void SetActive(Sprite Icon, int numObj,bool isVal)
    {

       this.gameObject.transform.GetChild(numObj).gameObject.SetActive(false);
       this.gameObject.transform.GetChild(numObj).gameObject.GetComponent<Image>().sprite = Icon;

    }
}
