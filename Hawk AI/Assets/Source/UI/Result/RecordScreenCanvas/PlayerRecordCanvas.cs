using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public interface IActivatePlayerRecordImage : IEventSystemHandler
{
    void SetActive(Sprite Icon,int numObj,bool isVal);
}

public class PlayerRecordCanvas : MonoBehaviour, IActivatePlayerRecordImage
{
    private List<GameObject> ImageObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetActive(Sprite Icon, int numObj, bool isVal)
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            ImageObj.Add(this.gameObject.transform.GetChild(i).gameObject);
            ImageObj[i].SetActive(false);
        }

        for (int i = 0; i < numObj; i++)
        {
            ImageObj[i].SetActive(isVal);
            ImageObj[i].GetComponent<Image>().sprite = Icon;
        }

    }
}
