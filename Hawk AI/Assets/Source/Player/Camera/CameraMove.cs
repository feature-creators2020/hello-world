using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class CameraMove : MonoBehaviour
{

    public GameObject targetObj;
    Vector3 targetPos;
    public GamePad.Index GamePadIndex;


    // Start is called before the first frame update
    void Start()
    {
        targetPos = targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // ゲームパッドの情報取得
        var playerNo = GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // ゲームパッドの入力情報取得
        float inputHorizontal = keyState.rightStickAxis.x;

        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        // 方向キー左右で方向転換
        /*
        float inputHorizontal = 0;
        if (Input.GetKey(KeyCode.J))
        {
            inputHorizontal = -1;
        }
        if (Input.GetKey(KeyCode.L))
        {
            inputHorizontal = 1;
        }
        */
        transform.RotateAround(targetPos, Vector3.up, inputHorizontal * Time.deltaTime * 200f);
        //transform.RotateAround(targetPos, transform.right, 200f);
    }
}
