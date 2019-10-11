using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public GameObject targetObj;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        // 方向キー左右で方向転換
        float HorizontalInput = 0;
        if (Input.GetKey(KeyCode.J))
        {
            HorizontalInput = -1;
        }
        if (Input.GetKey(KeyCode.L))
        {
            HorizontalInput = 1;
        }
        transform.RotateAround(targetPos, Vector3.up, HorizontalInput * Time.deltaTime * 200f);
        //transform.RotateAround(targetPos, transform.right, 200f);
    }
}
