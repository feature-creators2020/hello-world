using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSelectScenePuller : GeneralObject
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

            //TODO : Stage Select

            ExecuteEvents.Execute<ISceneInterfase>(
               target: gameObject,
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.ChangeStete(ESceneState.GameMain));
        }
    }

    public override void GeneralRelease()
    {
        base.GeneralRelease();
    }

}
