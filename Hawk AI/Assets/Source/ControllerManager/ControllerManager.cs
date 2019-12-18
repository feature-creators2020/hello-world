using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KeyBoardInput
{
    public static class KeyBoard
    {
        public enum Button { A, B, Y, X, RightShoulder, LeftShoulder, RightStick, LeftStick, Back, Start }
        public enum Trigger { LeftTrigger, RightTrigger }
        public enum Axis { LeftStick, RightStick, Dpad }
        public enum Index { Any, One, Two, Three, Four }

        public static bool GetButtonDown(Button button, Index controlIndex)
        {
            KeyCode code = GetKeycode(button, controlIndex);
            return Input.GetKeyDown(code);
        }

        public static bool GetButtonUp(Button button, Index controlIndex)
        {
            KeyCode code = GetKeycode(button, controlIndex);
            return Input.GetKeyUp(code);
        }

        public static bool GetButton(Button button, Index controlIndex)
        {
            KeyCode code = GetKeycode(button, controlIndex);
            return Input.GetKey(code);
        }
        public static Vector2 GetAxis(Axis axis, Index controlIndex, bool raw = false)
        {

            string xName = "", yName = "";
            switch (axis)
            {
                case Axis.Dpad:
                    xName = "DKey_XAxis_" + (int)controlIndex;
                    yName = "DKey_YAxis_" + (int)controlIndex;
                    break;
                case Axis.LeftStick:
                    xName = "LKey_XAxis_" + (int)controlIndex;
                    yName = "LKey_YAxis_" + (int)controlIndex;
                    break;
                case Axis.RightStick:
                    xName = "RKey_XAxis_" + (int)controlIndex;
                    yName = "RKey_YAxis_" + (int)controlIndex;
                    break;
            }

            Vector2 axisXY = Vector3.zero;

            try
            {
                if (raw == false)
                {
                    axisXY.x = Input.GetAxis(xName);
                    axisXY.y = Input.GetAxis(yName);
                }
                else
                {
                    axisXY.x = Input.GetAxisRaw(xName);
                    axisXY.y = Input.GetAxisRaw(yName);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the KeyboardInput package. \nWarning: do so will overwrite any existing input");
            }
            return axisXY;
        }

        public static float GetTrigger(Trigger trigger, Index controlIndex, bool raw = false)
        {
            //
            string name = "";
            if (trigger == Trigger.LeftTrigger)
                name = "TriggersL_" + (int)controlIndex;
            else if (trigger == Trigger.RightTrigger)
                name = "TriggersR_" + (int)controlIndex;

            //
            float axis = 0;
            try
            {
                if (raw == false)
                    axis = Input.GetAxis(name);
                else
                    axis = Input.GetAxisRaw(name);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the KeyboardInput package. \nWarning: do so will overwrite any existing input");
            }
            return axis;
        }


        static KeyCode GetKeycode(Button button, Index controlIndex)
        {
            switch (controlIndex)
            {
                case Index.One:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Z;
                        case Button.B: return KeyCode.X;
                        case Button.X: return KeyCode.Joystick1Button0;
                        case Button.Y: return KeyCode.Joystick1Button3;
                        case Button.RightShoulder: return KeyCode.E;
                        case Button.LeftShoulder: return KeyCode.Q;
                        case Button.Back: return KeyCode.Z;
                        case Button.Start: return KeyCode.C;
                        case Button.LeftStick: return KeyCode.Joystick1Button10;
                        case Button.RightStick: return KeyCode.Joystick1Button11;
                    }
                    break;
                case Index.Two:
                    switch (button)
                    {
                        case Button.A: return KeyCode.V;
                        case Button.B: return KeyCode.B;
                        case Button.X: return KeyCode.Joystick2Button0;
                        case Button.Y: return KeyCode.Joystick2Button3;
                        case Button.RightShoulder: return KeyCode.Y;
                        case Button.LeftShoulder: return KeyCode.R;
                        case Button.Back: return KeyCode.V;
                        case Button.Start: return KeyCode.N;
                        case Button.LeftStick: return KeyCode.Joystick2Button10;
                        case Button.RightStick: return KeyCode.Joystick2Button11;
                    }
                    break;
                case Index.Three:
                    switch (button)
                    {
                        case Button.A: return KeyCode.M;
                        case Button.B: return KeyCode.Comma;
                        case Button.X: return KeyCode.Joystick3Button0;
                        case Button.Y: return KeyCode.Joystick3Button3;
                        case Button.RightShoulder: return KeyCode.O;
                        case Button.LeftShoulder: return KeyCode.U;
                        case Button.Back: return KeyCode.M;
                        case Button.Start: return KeyCode.Period;
                        case Button.LeftStick: return KeyCode.Joystick3Button10;
                        case Button.RightStick: return KeyCode.Joystick3Button11;
                    }
                    break;
                case Index.Four:

                    switch (button)
                    {
                        case Button.A: return KeyCode.Slash;
                        case Button.B: return KeyCode.Backslash;
                        case Button.X: return KeyCode.Joystick4Button0;
                        case Button.Y: return KeyCode.Joystick4Button3;
                        case Button.RightShoulder: return KeyCode.LeftBracket;
                        case Button.LeftShoulder: return KeyCode.P;
                        case Button.Back: return KeyCode.Slash;
                        case Button.Start: return KeyCode.RightShift;
                        case Button.LeftStick: return KeyCode.Joystick4Button10;
                        case Button.RightStick: return KeyCode.Joystick4Button11;
                    }

                    break;
                case Index.Any:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Z;
                        case Button.B: return KeyCode.X;
                        case Button.X: return KeyCode.JoystickButton0;
                        case Button.Y: return KeyCode.JoystickButton3;
                        case Button.RightShoulder: return KeyCode.E;
                        case Button.LeftShoulder: return KeyCode.Q;
                        case Button.Back: return KeyCode.Z;
                        case Button.Start: return KeyCode.C;
                        case Button.LeftStick: return KeyCode.JoystickButton10;
                        case Button.RightStick: return KeyCode.JoystickButton11;
                    }
                    break;
            }
            return KeyCode.None;
        }

        public static KeyboardState GetState(Index controlIndex, bool raw = false)
        {
            KeyboardState state = new KeyboardState();

            state.A = GetButton(Button.A, controlIndex);
            state.B = GetButton(Button.B, controlIndex);
            state.Y = GetButton(Button.Y, controlIndex);
            state.X = GetButton(Button.X, controlIndex);

            state.RightShoulder = GetButton(Button.RightShoulder, controlIndex);
            state.LeftShoulder = GetButton(Button.LeftShoulder, controlIndex);
            state.RightStick = GetButton(Button.RightStick, controlIndex);
            state.LeftStick = GetButton(Button.LeftStick, controlIndex);

            state.Start = GetButton(Button.Start, controlIndex);
            state.Back = GetButton(Button.Back, controlIndex);

            state.LeftStickAxis = GetAxis(Axis.LeftStick, controlIndex, raw);
            state.rightStickAxis = GetAxis(Axis.RightStick, controlIndex, raw);
            state.dPadAxis = GetAxis(Axis.Dpad, controlIndex, raw);

            state.Left = (state.dPadAxis.x < 0);
            state.Right = (state.dPadAxis.x > 0);
            state.Up = (state.dPadAxis.y > 0);
            state.Down = (state.dPadAxis.y < 0);

            state.LeftTrigger = GetTrigger(Trigger.LeftTrigger, controlIndex, raw);
            state.RightTrigger = GetTrigger(Trigger.RightTrigger, controlIndex, raw);

            return state;
        }

    }

    public class KeyboardState
    {
        public bool A = false;
        public bool B = false;
        public bool X = false;
        public bool Y = false;
        public bool Start = false;
        public bool Back = false;
        public bool Left = false;
        public bool Right = false;
        public bool Up = false;
        public bool Down = false;
        public bool LeftStick = false;
        public bool RightStick = false;
        public bool RightShoulder = false;
        public bool LeftShoulder = false;

        public Vector2 LeftStickAxis = Vector2.zero;
        public Vector2 rightStickAxis = Vector2.zero;
        public Vector2 dPadAxis = Vector2.zero;

        public float LeftTrigger = 0;
        public float RightTrigger = 0;

    }

}