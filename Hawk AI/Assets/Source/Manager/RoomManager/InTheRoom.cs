using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTheRoom : MonoBehaviour
{
    //部屋番号
    [SerializeField]
    private int RoomIndex;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        //Humanタグ持ちであれば(今はPlayer)
        if (other.tag == "Human")
        {
            RoomManager.Instance.HumanEnter(RoomIndex);
        }
        if(other.tag == "Mouse")
        {
            RoomManager.Instance.MouseEnter(RoomIndex);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Humanタグ持ちであれば(今はPlayer)
        if (other.tag == "Human")
        {
            RoomManager.Instance.HumanExit(RoomIndex);
        }
        if (other.tag == "Mouse")
        {
            RoomManager.Instance.MouseExit(RoomIndex);
        }
    }
}
