using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRespawn : MonoBehaviour
{
    /*  unityでのキャラクターの復活             */
    /*  https://teratail.com/questions/38941    */

    //private float time = 0.0f;

    ////対象のオブジェクト
    //private bool isRespon = false;
    //private Vector3 initPos;

    //// Use this for initialization 
    //void Start()
    //{
    //    //初期値設定 
    //    this.initPos = this.gameObject.transform.position;
    //}
    //// Update is called once per frame 
    //void Update()
    //{
    //    if (isRespon)
    //    {
    //        time += Time.deltaTime;
    //        if (time >= 3.0f)
    //        {
    //            time = 0.0f;
    //            isRespon = false;
    //            this.gameObject.SetActive(true);
    //            this.gameObject.transform.position = initPos;
    //        }
    //    }
    //}
    //void OnCollisionEnter2D(Collision2D coll)
    //{
    //    if (coll.gameObject.name == "Cube")
    //    {
    //        this.desObj.SetActive(false);
    //        isRespon = true;
    //        //または
    //        //coll.gameObject.SetActive(false);
    //    }
    //}
    void Update()
    {
        if (Input.GetKey("up"))
        {
            this.transform.position += Vector3.forward * 0.1f;
        }
        if (Input.GetKey("down"))
        {
            this.transform.position += Vector3.back * 0.1f;
        }
        if (Input.GetKey("right"))
        {
            this.transform.position += Vector3.right * 0.1f;
        }
        if (Input.GetKey("left"))
        {
            this.transform.position += Vector3.left * 0.1f;
        }
    }


    /*  ゴールするか倒されるかでRespawn関数をシングルトンで呼び出し*/
    void OnTriggerEnter(Collider other)
    {
        //現状名前判定、タグ判定の方がいいかな？
        //ゴール時、死亡時の違いはまだつけてない
        //呼ぶのを遅らせるか関数内で何秒後にリスポーンするかをいれる
        if (other.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            ScoreManager.Instance.GoalMouse();
            RespawnPoint.Instance.Respawn(this.gameObject);
        }
        if(other.gameObject.name == "Dead")
        {
            ScoreManager.Instance.DeadMouse();
            RespawnPoint.Instance.Respawn(this.gameObject);
        }
    }

}
