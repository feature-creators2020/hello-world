using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    // カーソル
    [SerializeField]
    GameObject CheeseCursor01;
    [SerializeField]
    GameObject CheeseCursor02;
    [SerializeField]
    GameObject ItemCursor;

    void Update()
    {
        List<Transform> Target = ShiftOtherGoal.Instance.GetGoalTransform();
        CheeseCursor01.transform.LookAt(Target[0]);
        if(Target.Count > 1)
        {
            CheeseCursor02.SetActive(true);
            CheeseCursor02.transform.LookAt(Target[1]);
        }
        else
        {
            CheeseCursor02.SetActive(false);
        }
    }
}
