using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    //インスタンス
    public static LookAtCursor Instance;
    // カーソル
    [SerializeField]
    private List<GameObject> List_CheeseCursor;
    [SerializeField]
    private List<GameObject> List_ItemCursor;
    [SerializeField]
    private GameObject DroneCursor;
    private List<GameObject> List_Cheese;
    private List<GameObject> List_Item;
    private GameObject Drone;
    private float CursorHeight;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CursorHeight = this.transform.position.y;
        List_Cheese = new List<GameObject>();
        List_Item = new List<GameObject>();
        Drone = GameObject.Find("Drone");
    }

    void Update()
    {
        for(int i = 0; i < List_Cheese.Count; i++)
        {
            List_CheeseCursor[i].transform.LookAt(List_Cheese[i].transform);
        }
        for (int i = 0; i < List_Item.Count; i++)
        {
            List_ItemCursor[i].transform.LookAt(List_Item[i].transform);
        }
        DroneCursor.transform.LookAt(Drone.transform);
    }

    public void SetCheeseActive()
    {
        List_Cheese = ShiftOtherGoal.Instance.GetGoalObj();
        for(int i = 0; i < List_Cheese.Count; i++)
        {
            List_CheeseCursor[i].SetActive(true);
        }
        for(int i = List_Cheese.Count; i < List_CheeseCursor.Count; i++)
        {
            List_CheeseCursor[i].SetActive(false);
        }
    }

    public void SetItem(GameObject ItemObj)
    {
        List_Item.Add(ItemObj);
        for (int i = 0; i < List_Item.Count; i++)
        {
            List_ItemCursor[i].SetActive(true);
        }
        for (int i = List_Item.Count; i < List_ItemCursor.Count; i++)
        {
            List_ItemCursor[i].SetActive(false);
        }
    }

    public void GetItem(GameObject ItemObj)
    {
        List_Item.Remove(ItemObj);
        for (int i = 0; i < List_Item.Count; i++)
        {
            List_ItemCursor[i].SetActive(true);
        }
        for (int i = List_Item.Count; i < List_ItemCursor.Count; i++)
        {
            List_ItemCursor[i].SetActive(false);
        }
    }
}
