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
            if (other.transform.parent.name == "Player_Mouse") RoomManager.Instance.Mouse01Enter(RoomIndex);
            else if (other.transform.parent.name == "Player_Mouse2") RoomManager.Instance.Mouse02Enter(RoomIndex);
            //Debug.Log(other.transform.parent.name);
        }
        if(other.tag == "Drone")
        {
            RoomManager.Instance.DroneEnter(RoomIndex);
        }
        //if(other.tag == "Respawn")
        //{
        //    RespawnPoint.Instance.RespInit(other.gameObject, RoomIndex);
        //}
        //Debug.Log(other.name);
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
            if (other.transform.parent.name == "Player_Mouse") RoomManager.Instance.Mouse01Exit(RoomIndex);
            else if (other.transform.parent.name == "Player_Mouse2") RoomManager.Instance.Mouse02Exit(RoomIndex);
        }
        if (other.tag == "Drone")
        {
            RoomManager.Instance.DroneExit(RoomIndex);
        }
    }
}
