using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private GameObject CameraObj;

    void Update()
    {
        Vector3 p = CameraObj.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }
}
