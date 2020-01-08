using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IMouseGetTrapCollider : IEventSystemHandler
{
    void SetAnotherHitFlg(bool _isVal);
}


public class MouseGetTrapCollider : MonoBehaviour, IMouseGetTrapCollider
{
    [SerializeField]
    private GameObject MouseGetTrapObj;

    [SerializeField]
    private GameObject AnotherMouseGetTrapCollider;

    private GameObject MouseObject = null;

    private bool isAnotherSideHit = false;

    // Start is called before the first frame update
    void Start()
    {
        MouseObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (MouseObject == null)
        {
            if (other.tag == "Mouse")
            {
                if (isAnotherSideHit == false)
                {
                    //もう片方のコライダーに入らないようにする
                    isAnotherSideHit = true;
                    ExecuteEvents.Execute<IMouseGetTrapCollider>(
                    target: AnotherMouseGetTrapCollider,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.SetAnotherHitFlg(true));


                    MouseObject = other.gameObject;

                    ExecuteEvents.Execute<IMouseGetTrap>(
                    target: MouseGetTrapObj,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.TrapActive(true, other.gameObject));

                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (MouseObject == other.gameObject)
        {
            if (other.tag == "Mouse")
            {
                //if (isAnotherSideHit == false)
                {
                    MouseObject = null;

                    ExecuteEvents.Execute<IMouseGetTrap>(
                    target: MouseGetTrapObj,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.TrapActive(false, other.gameObject));

                    isAnotherSideHit = false;
                }
            }
        }
    }


    public void SetAnotherHitFlg(bool _isVal)
    {
        isAnotherSideHit = _isVal;
    }

}
