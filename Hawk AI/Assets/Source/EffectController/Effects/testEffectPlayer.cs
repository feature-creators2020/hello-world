using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// エフェクトの再生テストクラス
/// </summary>

public class testEffectPlayer : MonoBehaviour
{

    [SerializeField]
    private GameObject AttatchedObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        #region Debug
        if (Input.GetKey(KeyCode.Alpha0))
        {
            ExecuteEvents.Execute<IEffectControllerInterface>(
                target: AttatchedObj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Play());

        }

        if (Input.GetKey(KeyCode.Alpha9))
        {
            ExecuteEvents.Execute<IEffectControllerInterface>(
                target: AttatchedObj,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Stop());

        }
        #endregion //region
    }
}
