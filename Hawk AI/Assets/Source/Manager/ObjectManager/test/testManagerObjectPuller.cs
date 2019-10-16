using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testManagerObjectPuller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject DebugObj = null;
            GameObject gameObject
             = ManagerObjectManager.Instance.GetGameObject((int)EManagerObject.eOBJECT);

            ExecuteEvents.Execute<IGeneralInterface>(
                target: gameObject,
                eventData: null,
                functor: (recieveTarget, y) => DebugObj = recieveTarget.GetGameObject(1));

            Debug.Log(DebugObj.name);
        }

    }
}
