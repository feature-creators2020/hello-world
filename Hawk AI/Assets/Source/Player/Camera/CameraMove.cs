using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class CameraMove : MonoBehaviour
{

    public GameObject targetObj;
    Vector3 targetPos;
    public GamePad.Index GamePadIndex;

    Vector3 def;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = targetObj.transform.position;
        //targetPos = this.transform.parent.transform.position;
        def = this.transform.localRotation.eulerAngles;
        offset = this.transform.position - targetPos;
        transform.position = targetPos + offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        // ゲームパッドの情報取得
        var playerNo = GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // ゲームパッドの入力情報取得
        float inputViewHorizontal = keyState.rightStickAxis.x;

        //targetPos = this.transform.parent.transform.position;

        this.transform.RotateAround(targetPos, Vector3.up, inputViewHorizontal * Time.deltaTime * 200f);

    }
}
