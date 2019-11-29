using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : GeneralManager
{
    //インスタンス
    public static RoomManager Instance;
    //部屋の番号が入るリスト
    private List<int> List_InHuman = new List<int>();
    private List<int> List_InMouse = new List<int>();
    private List<int> List_InDrone = new List<int>();

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
        List_InHuman.Add(index);
    }

    //人間が出て行ったときに部屋番号をリストから（1つのみ）除外
    public void HumanExit(int index)
    {
        List_InHuman.Remove(index);
    }

    //オブジェリストから人がいない部屋にあるものだけ格納用リストに追加
    public void FarOffHuman(List<GameObject> RespObj, List<GameObject> RespList)
    {
        //オブジェリストにある分だけ
        for (int i = 0; i < RespObj.Count ; i++)
        {
            //指定した数値がリストになければ入れ込み
            if (!List_InHuman.Contains(i))
            {
                RespList.Add(RespObj[i]);
            }
        }
    }

    //ネズミが入ってきたときに部屋番号をリストに追加
    public void MouseEnter(int index)
    {
        List_InMouse.Add(index);
    }

    //ネズミが出て行ったときに部屋番号をリストから（1つのみ）除外
    public void MouseExit(int index)
    {
        List_InMouse.Remove(index);
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
    public void DrawnEnter(int index)
    {
        List_InDrone.Add(index);
    }

    //ホークドローンが出て行ったときに部屋番号をリストから（1つのみ）除外
    public void DrawnExit(int index)
    {
        List_InDrone.Remove(index);
    }
}
