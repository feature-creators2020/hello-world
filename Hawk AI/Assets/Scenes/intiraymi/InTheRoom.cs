using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTheRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("InTheRoom.cs Start");
    }
    //部屋番号
    [SerializeField]
    private int RoomIndex;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        //Humanタグ持ちであれば(今はPlayer)
        if (other.tag == "Player")
        {
            //Debug.Log("Enter" + RoomIndex);
            RoomManager.Instance.HumanEnter(RoomIndex);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Humanタグ持ちであれば(今はPlayer)
        if (other.tag == "Player")
        {
            //Debug.Log("Exit" + RoomIndex);
            RoomManager.Instance.HumanExit(RoomIndex);
        }
    }
}
