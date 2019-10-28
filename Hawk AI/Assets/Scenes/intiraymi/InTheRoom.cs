using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTheRoom : MonoBehaviour
{
    [SerializeField]
    private int RoomIndex;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            Debug.Log("Enter" + RoomIndex);
            RoomManager.Instance.HumanEnter(RoomIndex);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            RoomManager.Instance.HumanExit(RoomIndex);
        }
    }
}
