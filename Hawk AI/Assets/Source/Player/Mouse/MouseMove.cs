using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class MouseMove : MonoBehaviour
{

    float inputHorizontal;              // コントローラーLスティック横軸情報
    float inputVertical;                // コントローラーLスティック縦軸情報
    public Camera targetCamera;         // 対象のカメラ
    public GamePad.Index GamePadIndex;  // 対象のコントローラー

    float moveSpeed = 3f;               // 通常の移動速度
    float RunRate = 1.5f;               // 走る時の速度倍率
    bool isRun;                         // 走る状態切り替え


    void Start()
    {
        //targetCamera = transform.GetComponentInChildren<Camera>();
    }

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

        isRun = keyState.A; 


        // 仮：キーボードの入力
        if (Input.GetKey(KeyCode.A))
        {
            inputHorizontal = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputHorizontal = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVertical = -1;
        }
        if (Input.GetKey(KeyCode.W))
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

        if (isRun)
        {
            moveForward *= RunRate;
        }

        this.transform.position += moveForward * moveSpeed * Time.deltaTime;

        // キャラクターの向きを進行方向に
        if(moveForward != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(moveForward);
        }


    }
}
