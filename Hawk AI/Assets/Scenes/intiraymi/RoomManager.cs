using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;

public class RoomManager : GeneralManager
{
    //インスタンス
    public static RoomManager Instance;
    //人間がいる部屋の番号が入るリスト
    private List<int> List_Human = new List<int>();
    private List<int> List_Mouse = new List<int>();

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
    public void FarOffHuman(List<GameObject> RespObj, List<GameObject> RespList)
    {
        //オブジェリストにある分だけ
        for (int i = 0; i < RespObj.Count ; i++)
        {
            //指定した数値がリストになければ入れ込み
            if (!List_Human.Contains(i))
            {
                RespList.Add(RespObj[i]);
            }
        }
    }

    //人間が入ってきたときに部屋番号をリストに追加
    public void MouseEnter(int index)
    {
        List_Mouse.Add(index);
    }

    //人間が出て行ったときに部屋番号をリストから（1つのみ）除外
    public void MouseExit(int index)
    {
        List_Mouse.Remove(index);
    }

    //
    public void FarOffMouse(List<GameObject> RespObj, List<GameObject> RespList)
    {
        //オブジェリストにある分だけ
        for (int i = 0; i < RespObj.Count; i++)
        {
            //指定した数値がリストになければ入れ込み
            if (!List_Human.Contains(i))
            {
                RespList.Add(RespObj[i]);
            }
        }
    }
}
