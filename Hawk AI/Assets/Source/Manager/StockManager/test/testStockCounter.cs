using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// TODO : 動作確認
/// </summary>

public class testStockCounter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("KeyCode.RightArrow");
            //StockManager.Instance.AddPlayerStockCount(4);
            

            GameObject obj = GameObject.FindWithTag("testObject");

            ExecuteEvents.Execute<IStockManager>(
                 target: obj,
                 eventData: null,
                 functor: (recieveTarget, y) => recieveTarget.AddPlayerStockCount(4));


        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("KeyCode.DownArrow");
            //StockManager.Instance.AddPlayerStockCount(4);

            GameObject obj = GameObject.FindWithTag("testObject");

            ExecuteEvents.Execute<IStockManager>(
                 target: obj,
                 eventData: null,
                 functor: (recieveTarget, y) => recieveTarget.PlayerStockMinus(2));


        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("KeyCode.UpArrow");
            //StockManager.Instance.AddPlayerStockCount(4);

            GameObject obj = GameObject.FindWithTag("testObject");

            ExecuteEvents.Execute<IStockManager>(
                 target: obj,
                 eventData: null,
                 functor: (recieveTarget, y) => recieveTarget.PlayerStockPlus(3));


        }
    }


}
