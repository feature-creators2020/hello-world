using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class HumanMove : MonoBehaviour
{

    float inputHorizontal;
    float inputVertical;
    //Rigidbody rb;
    public Camera targetCamera;
    public GamePad.Index GamePadIndex;

    float moveSpeed = 3f;


    // Start is called before the first frame update
    void Start()
    {
        //targetCamera = transform.Find("Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        // ゲームパッドの情報取得
        var playerNo = GamePadIndex;
        var keyState = GamePad.GetState(playerNo, false);

        // ゲームパッドの入力情報取得
        inputHorizontal = 0f;
        inputVertical = 0f;

        inputHorizontal = keyState.LeftStickAxis.x;
        inputVertical = keyState.LeftStickAxis.y;

        if (Input.GetKey(KeyCode.F))
        {
            inputHorizontal = -1;
        }
        if (Input.GetKey(KeyCode.H))
        {
            inputHorizontal = 1;
        }
        if (Input.GetKey(KeyCode.G))
        {
            inputVertical = -1;
        }
        if (Input.GetKey(KeyCode.T))
        {
            inputVertical = 1;
        }

        //inputHorizontal = Input.GetAxisRaw("Horizontal");
        //inputVertical = Input.GetAxisRaw("Vertical");

    }


    // 移動処理
    void FixedUpdate()
    {
        // カメラの方向から、x-z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(targetCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 移動量
        Vector3 moveForward = cameraForward * inputVertical + targetCamera.transform.right * inputHorizontal;

        this.transform.position += moveForward * moveSpeed * Time.deltaTime;

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(moveForward);
        }


    }

}
