using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IStockManager : IEventSystemHandler
{
    void PlayerStockPlus(int _ID);
    void PlayerStockMinus(int _ID);

    int GetPlayerStockCount(int _ID);

    void SetPlayerStockCount(int _ID, int _NumStock);
    void AddPlayerStockCount(int _NumStock);
}

public class StockManager : GeneralManager, IStockManager
{
    private List<int> m_nPlayerStockCount = new List<int>();

    // Start is called before the first frame update
    public override void GeneralInit()
    {

    }

    public override void GeneralUpdate()
    {
        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    Debug.Log("KeyCode.C");

        //    foreach (var val in m_nPlayerStockCount)
        //    {
        //        Debug.Log(val);
        //    }
        //}

    }

    public override void GeneralRelease()
    {
    }

    public void PlayerStockPlus(int _ID)
    {
        m_nPlayerStockCount[_ID]++;
    }


    public void PlayerStockMinus(int _ID)
    {
        if (m_nPlayerStockCount[_ID] > 0)
            m_nPlayerStockCount[_ID]--;
    }

    public int GetPlayerStockCount(int _ID)
    {
        return m_nPlayerStockCount[_ID];
    }

    public void SetPlayerStockCount(int _ID, int _NumStock)
    {
        m_nPlayerStockCount[_ID] = _NumStock;
    }

    public void AddPlayerStockCount(int _NumStock)
    {
        m_nPlayerStockCount.Add(_NumStock);
    }
}
