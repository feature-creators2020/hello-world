using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialScenePuller : GeneralObject
{
    public override void GeneralInit()
    {
        base.GeneralInit();
    }

    public override void GeneralUpdate()
    {
        if (Input.anyKeyDown)
        {
            GameObject gameObject
                 = ManagerObjectManager.Instance.GetGameObject((int)EManagerObject.eSCENE);

            ExecuteEvents.Execute<ISceneInterfase>(
               target: gameObject,
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.StageSelect));
        }
    }

    public override void GeneralRelease()
    {
        base.GeneralRelease();
    }

}
