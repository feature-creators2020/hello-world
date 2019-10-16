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
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Goal")
        {
            RespawnPoint.Instance.Respawn(this.gameObject);
        }
        if(other.gameObject.name == "Dead")
        {
            RespawnPoint.Instance.Respawn(this.gameObject);
        }
    }

}
