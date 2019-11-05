using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using KeyBoardInput;


public class CameraMove : MonoBehaviour
{

    public GameObject targetObj;
    Vector3 targetPos;
    public GamePad.Index GamePadIndex;
    public KeyBoard.Index KeyboardIndex;

    Vector3 def;
    Vector3 offset;

    bool SwitchInput = false;   // トリガー、スティック操作の切り替え

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
        var playerKeyNo = (KeyBoard.Index)playerNo;
        var keyboardState = KeyBoard.GetState(KeyboardIndex, false);

        // ゲームパッドの入力情報取得
        if (GamePad.GetButtonDown(GamePad.Button.Start, playerNo))
        {
            SwitchInput = !SwitchInput;
            //Debug.Log("Switch Mode " + playerNo);
        }

        float LeftTrigger = keyState.LeftTrigger + 1.0f;
        float RightTrigger = keyState.RightTrigger + 1.0f;
        float inputViewHorizontal = (RightTrigger - LeftTrigger) * 0.5f;
        inputViewHorizontal += keyboardState.rightStickAxis.x;

        if (SwitchInput)
        {
            inputViewHorizontal = keyState.rightStickAxis.x;
        }

        this.transform.RotateAround(targetPos, Vector3.up, inputViewHorizontal * Time.deltaTime * 200f);

    }
}
