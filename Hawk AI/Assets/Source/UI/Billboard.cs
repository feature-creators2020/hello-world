using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        this.transform.rotation = new Quaternion(0,0,0,1);
    }
}
