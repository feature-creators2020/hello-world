using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class ControllerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var controllerNames = Input.GetJoystickNames();

        var debugIndex = GamePad.Index.Two;
        Debug.Log(debugIndex.GetType().ToString());

        //Debug.Log(controllerNames.Length);
        //Debug.Log(Input.GetJoystickNames()[0]);

        /*
        var debuginput = GamePad.GetState(GamePad.Index.Two, false);
        Debug.Log("A:" + debuginput.A);
        Debug.Log("B:" + debuginput.B);
        Debug.Log("Back:" + debuginput.Back);
        Debug.Log("Down:" + debuginput.Down);
        Debug.Log("dPadAxis:" + debuginput.dPadAxis);
        Debug.Log("Left:" + debuginput.Left);
        Debug.Log("LeftShoulder:" + debuginput.LeftShoulder);
        Debug.Log("LeftStick:" + debuginput.LeftStick);
        Debug.Log("LeftStickAxis:" + debuginput.LeftStickAxis);
        Debug.Log("LeftTrigger:" + debuginput.LeftTrigger);
        Debug.Log("Right:" + debuginput.Right);
        Debug.Log("RightShoulder:" + debuginput.RightShoulder);
        Debug.Log("RightStick:" + debuginput.RightStick);
        Debug.Log("rightStickAxis:" + debuginput.rightStickAxis);
        Debug.Log("RightTrigger:" + debuginput.RightTrigger);
        Debug.Log("Start:" + debuginput.Start);
        Debug.Log("Up:" + debuginput.Up);
        Debug.Log("X:" + debuginput.X);
        Debug.Log("Y:" + debuginput.Y);
        */

        //controllerNames.GetValue(0);
    }
}
