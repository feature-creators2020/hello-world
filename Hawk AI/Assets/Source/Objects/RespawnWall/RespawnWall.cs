using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RespawnWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Mouse")
        {
            ExecuteEvents.Execute<IMouseInterface>(
                    target: other.gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.SetRespawn());
        }
        if(other.gameObject.tag == "Human")
        {
            ExecuteEvents.Execute<IHumanInterface>(
                    target: other.gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.SetRespawn());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mouse")
        {
            ExecuteEvents.Execute<IMouseInterface>(
                    target: other.gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.SetRespawn());
        }
        if (other.gameObject.tag == "Human")
        {
            ExecuteEvents.Execute<IHumanInterface>(
                    target: other.gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.SetRespawn());
        }
    }
}
