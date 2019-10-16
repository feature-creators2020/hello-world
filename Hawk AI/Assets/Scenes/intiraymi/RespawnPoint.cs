using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****シングルトン化****/
public class RespawnPoint : SingletonMonoBehaviour<RespawnPoint>
{
    public GameObject[] RespObj;
    
    //Respawn関数(出現させるオブジェクト)
    public void Respawn(GameObject Obj)
    {
        //ランダムで生成場所を決定
        int number = Random.Range(0, RespObj.Length);
        //Debug.Log("singleton");
        //ゴール時、死亡時非アクティブ化する想定のコード
        Obj.SetActive(true);
        //設定位置に移動
        Obj.transform.position = RespObj[number].transform.position;

    }
}
