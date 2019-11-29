using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : SingletonMonoBehaviour<CursorManager>
{
    [SerializeField]
    private List<GameObject> Cursor;

    public void SetCheeseActive()
    {
        for(int i = 0; i < Cursor.Count; i++)
        {
            Cursor[i].GetComponent<LookAtCursor>().SetCheeseActive();
        }
    }

    public void SetItem(GameObject ItemObj)
    {
        for (int i = 0; i < Cursor.Count; i++)
        {
            Cursor[i].GetComponent<LookAtCursor>().SetItem(ItemObj);
        }
    }

    public void GetItem(GameObject ItemObj)
    {
        for (int i = 0; i < Cursor.Count; i++)
        {
            Cursor[i].GetComponent<LookAtCursor>().GetItem(ItemObj);
        }
    }
}
