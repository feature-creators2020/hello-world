using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemHolderManager : SingletonMonoBehaviour<ItemHolderManager>
{
    [SerializeField]
    private List<GameObject> ItemList;
    private int index = 0;

    public void HoldItem(GameObject ItemObj)
    {
        float TDistance = 0;
        PlayerManager c_PlayerManager = ManagerObjectManager.Instance.GetGameObject("PlayerManager").GetComponent<PlayerManager>();
        var HumanList = c_PlayerManager.GetGameObjectsList("Human");
        for (int i = 0; i < HumanList.Count; i++)
        {
            var targetObj = c_PlayerManager.GetGameObject(i, "Human");
            float nDis = Vector3.Distance(targetObj.transform.position, ItemObj.transform.position);
            if (TDistance == 0)
            {
                TDistance = nDis;
                index = i;
            }
            if (nDis <= TDistance)
            {
                TDistance = nDis;
                index = i;
            }
        }

        if (ItemObj.tag == "Mousetrap")
        {
            ItemList[index].GetComponent<Image>().sprite = Resources.Load<Sprite>("MouseTrap");
        }
        ItemList[index].GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void ReleaseItem(Vector3 ItemPos)
    {
        //Debug.Log(ItemPos);
        float TDistance = 0;
        PlayerManager c_PlayerManager = ManagerObjectManager.Instance.GetGameObject("PlayerManager").GetComponent<PlayerManager>();
        var HumanList = c_PlayerManager.GetGameObjectsList("Human");
        for (int i = 0; i < HumanList.Count; i++)
        {
            var targetObj = c_PlayerManager.GetGameObject(i, "Human");
            float nDis = Vector3.Distance(targetObj.transform.position, ItemPos);
            if (TDistance == 0)
            {
                TDistance = nDis;
                index = i;
            }
            if (nDis <= TDistance)
            {
                TDistance = nDis;
                index = i;
            }
        }

        ItemList[index].GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
}
