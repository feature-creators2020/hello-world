using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{

    //public float speed = 3.0f;
    //public float gravity = 9.81f;

    //public Vector3 moveDirection = Vector3.zero;

    //CharacterController controller;


    //// Start is called before the first frame update
    //void Start()
    //{
    //    controller = GetComponent<CharacterController>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (controller.isGrounded)
    //    {
    //        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    //        moveDirection = transform.TransformDirection(moveDirection);
    //        moveDirection *= speed;
    //    }

    //    moveDirection.y -= gravity * Time.deltaTime;
    //    controller.Move(moveDirection * Time.deltaTime);
    //}



    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;

    float moveSpeed = 3f;

    void Start()
    {
        
    }

    void Update()
    {

    }
}
