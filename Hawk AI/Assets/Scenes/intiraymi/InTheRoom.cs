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

    [SerializeField]
    private int RoomIndex;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            //Debug.Log("Enter" + RoomIndex);
            RoomManager.Instance.HumanEnter(RoomIndex, other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Exit" + RoomIndex);
            RoomManager.Instance.HumanExit(RoomIndex, other.gameObject);
        }
    }
}
