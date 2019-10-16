using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : SingletonMonoBehaviour<RespawnPoint>
{
    //対象のオブジェクト
    private Vector3 initPos;

    void Start()
    {
        //初期値設定 
        this.initPos = this.gameObject.transform.position;
    }
    
    public void Respawn(GameObject Obj)
    {
        Debug.Log("singleton");
        Obj.SetActive(true);
        Obj.transform.position = initPos;
    }
}
