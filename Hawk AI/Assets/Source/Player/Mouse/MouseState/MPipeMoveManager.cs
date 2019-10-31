using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;

public class MPipeMoveManager : CStateBase<MouseStateManager>
{
    public MPipeMoveManager(MouseStateManager _cOwner) : base(_cOwner) { }

    private const int AT_FIRST_POSELEMENT = 1;

    private int m_cPipeCount= AT_FIRST_POSELEMENT;
    private float lerpVal = 0f;

    public override void Enter()
    {
        //TODO : Target Object Pulling is Change
        ExecuteEvents.Execute<ISetMaterial>(
        target: GameObject.Find("testPermeableModel").gameObject,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.SetMaterial(this.m_cOwner.gameObject));

        GravityOff();

    }

    public override void Execute()
    {
        Debug.Log("State:Pipe");

        var playerNo = m_cOwner.GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);


        // ゲームパッドの入力情報取得
        m_cOwner.inputHorizontal = 0f;
        m_cOwner.inputVertical = 0f;

        //m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        //m_cOwner.inputVertical = keyState.LeftStickAxis.y;
        m_cOwner.inputHorizontal = 0.0f;
        m_cOwner.inputVertical = 2.0f;

        MoveLerp();
    }

    public override void Exit()
    {
        //TODO : Target Object Pulling is Change
        ExecuteEvents.Execute<ISetMaterial>(
        target: GameObject.Find("testPermeableModel").gameObject,
        eventData: null,
        functor: (recieveTarget, y) => recieveTarget.RevertMaterial(this.m_cOwner.gameObject));

        GravityOn();

    }

    private void MoveLerp()
    {
        float speed = 3f;
        var StartPos = this.m_cOwner.m_cPipeTransPosObj[m_cPipeCount].transform.position;
        var EndPos = this.m_cOwner.m_cPipeTransPosObj[1 + m_cPipeCount].transform.position;

        if (lerpVal <= 1f)
        {
            this.m_cOwner.transform.position =
            Vector3.Lerp(StartPos, EndPos, lerpVal);
            lerpVal += Time.deltaTime / speed; ;


            // キャラクターの向きを進行方向に
            Vector3 moveForward = EndPos - StartPos;
            m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward);

        }
        else
        {
            lerpVal = 0f;
            m_cPipeCount++;
        }


        if ((1 + m_cPipeCount) == this.m_cOwner.m_cPipeTransPosObj.Count)
        {
            m_cPipeCount = AT_FIRST_POSELEMENT;
            this.m_cOwner.m_cPipeTransPosObj = null;
            m_cOwner.ChangeState(0, EMouseState.Normal);

            return;
        }
    }

    private void GravityOff()
    {
        this.m_cOwner.GetComponent<Rigidbody>().useGravity = false;
        this.m_cOwner.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void GravityOn()
    {
        this.m_cOwner.GetComponent<Rigidbody>().useGravity = true;
        this.m_cOwner.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        this.m_cOwner.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        this.m_cOwner.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
    }

}
