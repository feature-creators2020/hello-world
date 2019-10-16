using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testScenePuller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject gameObject
             = ManagerObjectManager.Instance.GetGameObject((int)EManagerObject.eSCENE);

            ExecuteEvents.Execute<ISceneInterfase>(
               target: gameObject,
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.testSceneA));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject gameObject
             = ManagerObjectManager.Instance.GetGameObject((int)EManagerObject.eSCENE);

            ExecuteEvents.Execute<ISceneInterfase>(
               target: gameObject,
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.testSceneB));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject gameObject
             = ManagerObjectManager.Instance.GetGameObject((int)EManagerObject.eSCENE);

            ExecuteEvents.Execute<ISceneInterfase>(
               target: gameObject,
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.testSceneC));
        }
    }
}
