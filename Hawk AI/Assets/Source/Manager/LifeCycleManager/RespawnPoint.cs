﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****シングルトン化****/
public class RespawnPoint : SingletonMonoBehaviour<RespawnPoint>
{
    [SerializeField]
    private List<GameObject> RespObj;

    //Respawn関数(出現させるオブジェクト)
    public void Respawn(GameObject Obj)
    {
        //人間がいない空間にあるリスポーン地の配列を作成
        List<GameObject> RespList = new List<GameObject>();
        RoomManager.Instance.FarOffHuman(RespObj ,RespList);
        //ランダムで生成場所を決定
        int number;
        Vector3 pos;
        //もしすべての部屋に人間がいる状況が生まれた場合はすべてのリスポーン地からランダム
        if (RespList.Count > 0)
        {
            number = Random.Range(0, RespList.Count);
            //設定位置に移動
            pos = RespList[number].transform.position;
        }
        else
        {
            number = Random.Range(0, RespObj.Count);
            //設定位置に移動
            pos = RespObj[number].transform.position;
        }
        Obj.transform.position = new Vector3(pos.x, 0.5f, pos.z);
    }
}
