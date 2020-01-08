using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class MUpManager : CStateBase<MouseStateManager>
{
    float Distance;    // よじ登りで段々速度を下げる
    Vector3 StartPos;   // 上る最初の地点
    Vector3 EndPos;     // 最終地点

    float speed = 5.0f;
    float timer = 0f;

    bool m_isUp;        // 上に上った

    public MUpManager(MouseStateManager _cOwner) : base(_cOwner) { }

    public override void Enter()
    {
        timer = 0f;
        StartPos = m_cOwner.transform.position;
        var TopPos = m_cOwner.m_GTargetBoxObject.transform.position + new Vector3(0f, m_cOwner.m_GTargetBoxObject.transform.localScale.y / 2f, 0f);
        var SubPos = TopPos - StartPos;
        var UpPos = StartPos + new Vector3(0f, SubPos.y, 0f);
        //EndPos = UpPos - m_cOwner.m_TargetBoxNomal * 0.5f;
        EndPos = UpPos;
        Distance = Vector3.Distance(StartPos, EndPos);
        //Debug.Log(m_cOwner.m_GTargetBoxObject.name + ".lossyScale : " + m_cOwner.m_GTargetBoxObject.transform.localScale);
        //Debug.Log("StartPos : " + StartPos);
        //Debug.Log("EndPos : " + EndPos);
        m_cOwner.GravityOff();
        m_cOwner.m_SEAudio.Play((int)SEAudioType.eSE_Jump);   // ジャンプSE
        m_cOwner.PlayAnimation(EMouseAnimation.Run);
        m_isUp = false;
    }

    public override void Execute()
    {
        //Debug.Log("State:Up");

        //var playerNo = m_cOwner.GamePadIndex;
        //var keyState = GamePad.GetState(playerNo, false);

        //// ゲームパッドの入力情報取得
        //m_cOwner.inputHorizontal = 0f;
        //m_cOwner.inputVertical = 0f;

        //m_cOwner.inputHorizontal = keyState.LeftStickAxis.x;
        //m_cOwner.inputVertical = keyState.LeftStickAxis.y;

        if (Distance <= 0f)
        {

            Debug.DrawLine(m_cOwner.transform.position, m_cOwner.transform.position + m_cOwner.transform.forward * 0.5f, Color.red);
            Ray ray = new Ray(m_cOwner.transform.position, m_cOwner.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.5f))
            {
                var LayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                var TagName = hit.collider.gameObject.tag;
                if (LayerName == "Box" || LayerName == "Rail")
                {
                    if (TagName == "CanClimbing")
                    {
                        m_cOwner.transform.position = new Vector3(
                            m_cOwner.transform.position.x,
                            m_cOwner.transform.position.y + 0.5f,
                            m_cOwner.transform.position.z);

                        m_cOwner.ChangeState(0, m_cOwner.EOldState);
                        return;
                    }
                }
            }
        }

        float presentLocation = (timer * speed) / Distance;

        m_cOwner.transform.position = Vector3.Slerp(StartPos, EndPos, presentLocation);

        LookAtPoint();

        timer += Time.deltaTime;

        if (presentLocation >= 1.0f)
        {
            if (m_isUp)
            {
                m_cOwner.ChangeState(0, m_cOwner.EOldState);
            }
            else
            {
                m_isUp = true;
                // 位置を更新する（前に少し進めるため）
                StartPos = EndPos;
                EndPos = EndPos - m_cOwner.m_TargetBoxNomal * 0.5f;
                timer = 0f;
                Distance = Vector3.Distance(StartPos, EndPos);
            }
        }

    }

    public override void Exit()
    {
        //m_cOwner.EOldState = EMouseState.Normal;
        m_cOwner.GravityOn();
        var fowardVec = Vector3.Scale(m_cOwner.transform.forward, new Vector3(1, 0, 1)).normalized;
        //m_cOwner.transform.rotation = Quaternion.LookRotation(fowardVec);
        m_cOwner.m_MouseMesh.transform.rotation = Quaternion.LookRotation(fowardVec);
    }

    private void LookAtPoint()
    {
        // キャラクターの向きを進行方向に
        Vector3 moveForward = EndPos - StartPos;
        //m_cOwner.transform.rotation = Quaternion.LookRotation(moveForward, m_cOwner.m_TargetBoxNomal);
        m_cOwner.m_MouseMesh.transform.rotation = Quaternion.LookRotation(moveForward, m_cOwner.m_TargetBoxNomal);
    }

}
