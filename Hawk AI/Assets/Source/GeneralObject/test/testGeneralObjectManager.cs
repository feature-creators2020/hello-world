using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testGeneralObjectManager : MonoBehaviour
{
    private List<GameObject> gameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("testObject"))
        {
            gameObjects.Add(obj);

            ExecuteEvents.Execute<IGeneralInterface>(
                target: obj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.GeneralInit());

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            foreach (var obj in gameObjects)
            {
                ExecuteEvents.Execute<IGeneralInterface>(
                     target: obj,
                     eventData: null,
                     functor: (recieveTarget, y) => recieveTarget.GeneralUpdate());

            }
        }
    }
}
