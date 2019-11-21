using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderManager : SingletonMonoBehaviour<ItemHolderManager>
{
    [SerializeField]
    private List<GameObject> Item;

    public void HoldItem(string Data_tag)
    {
        //Debug.Log(Data);
        if(Data_tag == "Mousetrap")
        {
            Item[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("MouseTrap");
        }
        Item[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void ReleaseItem()
    {
        Item[0].GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
}
