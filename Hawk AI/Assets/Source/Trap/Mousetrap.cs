using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IMouseTrap : IEventSystemHandler
{
    void SetMapPosition(Vector2Int[] _mappos);

}


public class Mousetrap : GeneralObject, IMouseTrap
{
    private const int m_cTrapWidth = 2;
    private Vector2Int[] m_cTrapPos = new Vector2Int[m_cTrapWidth];

    public override void GeneralInit()
    {
        
    }

    public override void GeneralRelease()
    {
        // Map から消す
        for (int i = 0; i < m_cTrapWidth; i++)
        {
            if(MapManager.Instance != null)
            MapManager.Instance.MapData[m_cTrapPos[i].y][m_cTrapPos[i].x]
                = (int)ObjectNo.NONE;
                
        }

        //var itemmanager = ManagerObjectManager.Instance.GetGameObject("ItemManager");
        //var mousetrapmanager = itemmanager.GetComponent<ItemManager>().GetGameObject("MousetrapManager");
        //mousetrapmanager.GetComponent<MousetrapManager>().Destroy(this.gameObject);
    }

    //public override void OnDestroy()
    //{
    //    base.OnDestroy();
    //}

    public void SetMapPosition(Vector2Int[] _mappos)
    {
        for (int i = 0; i < m_cTrapWidth; i++)
        {
            m_cTrapPos[i] = _mappos[i];
        }
    }

}
