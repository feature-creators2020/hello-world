using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GamepadInput;
using KeyBoardInput;

public enum ELeverSwitchState
{
    ActiveCorrect,
    ActiveInverse,
}


public interface ILeverSwitch : IEventSystemHandler
{
    void ChangeState(ELeverSwitchState _eLeverSwitchState);
    ELeverSwitchState GetState();

}


public class LeverSwitch : MonoBehaviour, ILeverSwitch
{
    private GameObject m_cMeshObj;
    private ELeverSwitchState m_eLeverSwitchState;
    private bool m_bActivation;
    [SerializeField]
    private string m_sTag;
    [SerializeField]
    private float m_fBendLeverRadian;
    [SerializeField]
    private float m_fBenddingSpeed;
    [SerializeField]
    private GameObject ExclamationMark;
    [SerializeField]
    private List<GameObject> RailObjects = new List<GameObject>();

    GameObject m_gActorObject;
    int m_nActorControllerNum;

    // Start is called before the first frame update
    void Start()
    {
        m_cMeshObj = this.gameObject.transform.parent.GetChild(1)
            .gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        m_eLeverSwitchState = ELeverSwitchState.ActiveCorrect;
        m_bActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bActivation == true)
        {
            // コントローラー対応
            if ((GamePad.GetButtonDown(GamePad.Button.A, (GamePad.Index)m_nActorControllerNum) || (KeyBoard.GetButtonDown(KeyBoard.Button.A, (KeyBoard.Index)m_nActorControllerNum))))
            {
                if (GetState() == ELeverSwitchState.ActiveCorrect)
                {
                    // Correct To Inverse
                    //var RailManager = ManagerObjectManager.Instance.GetGameObject("RailManager").
                    //    GetComponent<RailManager>().GetGameObjectsList();

                    foreach (var val in RailObjects)
                    {
                        ExecuteEvents.Execute<IRailManager>(
                            target: val,
                            eventData: null,
                            functor: (recieveTarget, y) => recieveTarget.ChangeFloatingDirection());
                    }

                    Vector3 startRot = new Vector3(
                        m_fBendLeverRadian,
                        m_cMeshObj.transform.rotation.y,
                        m_cMeshObj.transform.rotation.z);

                    Vector3 endRot = new Vector3(
                         -m_fBendLeverRadian,
                         m_cMeshObj.transform.rotation.y,
                         m_cMeshObj.transform.rotation.z);

                    StartCoroutine(ActivateSwitching(startRot, endRot));

                    ChangeState(ELeverSwitchState.ActiveInverse);
                }
                else
                {
                    //  Inverse To Correct
                    //var RailManager = ManagerObjectManager.Instance.GetGameObject("RailManager").
                    //    GetComponent<RailManager>().GetGameObjectsList();

                    //foreach (var val in RailManager)
                    //{
                    //    ExecuteEvents.Execute<IRailInterfase>(
                    //        target: val,
                    //        eventData: null,
                    //        functor: (recieveTarget, y) => recieveTarget.ChangeState(ERailState.Correct));
                    //}

                    foreach (var val in RailObjects)
                    {
                        ExecuteEvents.Execute<IRailManager>(
                            target: val,
                            eventData: null,
                            functor: (recieveTarget, y) => recieveTarget.ChangeFloatingDirection());
                    }

                    Vector3 startRot = new Vector3(
                        -m_fBendLeverRadian,
                        m_cMeshObj.transform.rotation.y,
                        m_cMeshObj.transform.rotation.z
                        );

                    Vector3 endRot = new Vector3(
                        m_fBendLeverRadian,
                         m_cMeshObj.transform.rotation.y,
                         m_cMeshObj.transform.rotation.z
                         );

                    StartCoroutine(ActivateSwitching(startRot, endRot));

                    ChangeState(ELeverSwitchState.ActiveCorrect);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == m_sTag)
        {
            m_bActivation = true;
            ExclamationMark.SetActive(true);
            m_gActorObject = other.gameObject;
            ExecuteEvents.Execute<IHumanInterface>(
                            target: m_gActorObject,
                            eventData: null,
                            functor: (recieveTarget, y) => m_nActorControllerNum = recieveTarget.GetPlayerControllerNum());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == m_sTag)
        {
            m_bActivation = false;
            ExclamationMark.SetActive(false);
            m_gActorObject = null;
        }
    }

    public void ChangeState(ELeverSwitchState _eLeverSwitchState)
    {
        m_eLeverSwitchState = _eLeverSwitchState;
    }

    public ELeverSwitchState GetState()
    {
        return m_eLeverSwitchState;
    }

    IEnumerator ActivateSwitching(Vector3 _StartAngle, Vector3 _EndAngle)
    {
        float lerpVal = 0f;

        while (lerpVal <= 1f)
        {//
            m_cMeshObj.transform.rotation = Quaternion.Euler(
                Vector3.Lerp(_StartAngle, _EndAngle, lerpVal));
            lerpVal += Time.deltaTime / m_fBenddingSpeed;
            yield return null;
        }

        if (_StartAngle != _EndAngle)
        {//強硬手段
            m_cMeshObj.transform.rotation = Quaternion.Euler(_EndAngle);
        }
    }

}
