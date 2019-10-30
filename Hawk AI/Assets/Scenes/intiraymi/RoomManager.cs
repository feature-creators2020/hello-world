using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;

public class RoomManager : GeneralManager
{
    //public class Data
    //{
    //    public GameObject Obj;
    //    public int Index;

    //    public Data(GameObject Obj, int Index)
    //    {
    //        this.Obj = Obj;
    //        this.Index = Index;
    //    }
    //}
    //インスタンス
    public static RoomManager Instance;
    //人間がいる部屋の番号が入るリスト
    private List<int> List_Index = new List<int>();
    //private List<Data> List_Data = new List<Data>();
    //private bool[] m_bHumanInTheRoom = { false, false, false, false };

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
        //リスト管理が理想
        List_Index.Add(index);
        
        //Data data = new Data(Obj, index);
        //List_Data.Add(data);
        //Debug.Log(List_Data[0].Index);
        
        //for (int i = 0; i < List_Index.Count; i++)
        //{
        //    Debug.Log("Enter " + i + List_Index[i]);
        //}
        //m_bHumanInTheRoom[index] = true;
    }

    //人間が出て行ったときに部屋番号をリストから（1つのみ）除外
    public void HumanExit(int index)
    {
        List_Index.Remove(index);

        //Data data = new Data(Obj, index);
        //IEnumerable<Data> query = List_Data.Where(s => s.Obj == Obj );
        //foreach (var player in query)
        //{
        //    //Debug.Log(player.Index);
        //    List_Data.Remove(player);
        //}
        //for (int i = 0; i < List_Index.Count; i++)
        //{
        //    Debug.Log("Exit " + i + List_Index[i]);
        //}
        //m_bHumanInTheRoom[index] = false;
    }

    //オブジェリストから人がいない部屋にあるものだけ格納用リストに追加
    public void FarOffHuman(List<GameObject> RespObj, List<GameObject> RespList)
    {
        //オブジェリストにある分だけ
        for (int i = 0; i < RespObj.Count ; i++)
        {
            //指定した数値がリストになければ入れ込み
            if (!List_Index.Contains(i))
            {
                RespList.Add(RespObj[i]);
                //Debug.Log(RespObj[i]);
            }
        }
        //for(int i = 0; i < m_bHumanInTheRoom.Length; i++)
        //{
        //    if (!m_bHumanInTheRoom[i])  RespList.Add(RespObj[i]);
        //    //IEnumerable<Data> query = List_Data.Where(s => s.Obj.tag == "Player");
        //    //foreach(var player in query)
        //    //{
        //    //    Debug.Log(player.Index);
        //    //}
        //    //Debug.Log(List_Index[0]);
        //}
    }
}
