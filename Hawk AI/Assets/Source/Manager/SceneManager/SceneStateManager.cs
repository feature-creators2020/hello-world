using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum ESceneState
{
    testSceneA,
    testSceneB,
    testSceneC,
    Title,
    Tutorial,
    StageSelect,
    GameMain,
    Result,
}

public interface ISceneInterfase : IEventSystemHandler
{
    void ChangeStete(ESceneState eSceneState);
}

public class SceneStateManager : GeneralManager, ISceneInterfase
{
    [SerializeField]
    List<SceneObject> sceneObjects = new List<SceneObject>();

    // Start is called before the first frame update
    public override void GeneralInit()
    {
        base.GeneralInit();
    }

    public override void GeneralUpdate()
    {

        base.GeneralUpdate();
    }

    public override void GeneralRelease()
    {
        base.GeneralRelease();
    }

    public override void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var obj in m_cGameObjects)
            {
                Debug.Log(obj.name);
            }
        }
    }
    public override void OnDestroy()
    {
        GeneralRelease();

    }

    public override GameObject GetGameObject(int _ID)
    {
        return m_cGameObjects[_ID];
    }

    public virtual void ChangeStete(ESceneState eSceneState)
    {
        Debug.Log(sceneObjects[(int)eSceneState].GetSceneName());
        SceneManager.LoadScene(sceneObjects[(int)eSceneState].GetSceneName());

    }
}
