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
    //Rigidbody rb;
    public Camera targetCamera;

    float moveSpeed = 3f;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // カメラの方向から、x-z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 移動量
        Vector3 moveForward = cameraForward * inputVertical + targetCamera.transform.right * inputHorizontal;

        // 移動方向にスピードを掛ける。
        //rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);

        transform.position += moveForward * moveSpeed * Time.deltaTime;

        // キャラクターの向きを進行方向に
        if(moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }

    }
}
