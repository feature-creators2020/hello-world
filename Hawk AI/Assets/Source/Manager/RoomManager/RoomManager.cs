﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : GeneralManager
{
    //インスタンス
    public static RoomManager Instance;
    //部屋の番号が入るリスト
    private List<int> List_Human = new List<int>();
    private List<int> List_Mouse = new List<int>();
    private List<int> List_Drone = new List<int>();

    void Awake()
    {
        Instance = this;
    }

    public override void GeneralInit()
    {
        base.GeneralInit();
    }

    public override void GeneralUpdate()
    {
        base.GeneralUpdate();
    }

    //人間が入ってきたときに部屋番号をリストに追加
    public void HumanEnter(int index)
    {
        List_Human.Add(index);
    }

    //人間が出て行ったときに部屋番号をリストから（1つのみ）除外
    public void HumanExit(int index)
    {
        List_Human.Remove(index);
    }

    //オブジェリストから人がいない部屋にあるものだけ格納用リストに追加
    public void FarOffHuman(List<int> RespIndex, List<int> RespList)
    {
        for(int i = 0; i < RespIndex.Count; i++)
        {
            //指定した数値がリストになければ入れ込み
            if (!List_Human.Contains(RespIndex[i]))
            {
                RespList.Add(RespIndex[i]);
            }
        }
    }

    //ネズミが入ってきたときに部屋番号をリストに追加
    public void MouseEnter(int index)
    {
        List_Mouse.Add(index);
    }

    //ネズミが出て行ったときに部屋番号をリストから（1つのみ）除外
    public void MouseExit(int index)
    {
        List_Mouse.Remove(index);
    }

    //
    //public void FarOffMouse(List<GameObject> RespObj, List<GameObject> RespList)
    //{
    //    //オブジェリストにある分だけ
    //    for (int i = 0; i < RespObj.Count; i++)
    //    {
    //        //指定した数値がリストになければ入れ込み
    //        if (!List_InHuman.Contains(i))
    //        {
    //            RespList.Add(RespObj[i]);
    //        }
    //    }
    //}

    //ホークドローンが入ってきたときに部屋番号をリストに追加
    public void DroneEnter(int index)
    {
        List_Drone.Add(index);
    }

    //ホークドローンが出て行ったときに部屋番号をリストから（1つのみ）除外
    public void DroneExit(int index)
    {
        List_Drone.Remove(index);
    }

    //ホークドローンがいる部屋番号を読み取り
    public int GetDroneIn()
    {
        return List_Drone[0];
    }
}
