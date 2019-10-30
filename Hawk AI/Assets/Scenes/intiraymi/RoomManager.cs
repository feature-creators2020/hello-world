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
    
    public static RoomManager Instance;
    private List<int> List_Index = new List<int>();
    //private List<Data> List_Data = new List<Data>();

    void Awake()
    {
        Instance = this;
    }

    private bool[] m_bHumanInTheRoom = { false, false, false, false };

    public override void GeneralInit()
    {
        base.GeneralInit();
    }

    public override void GeneralUpdate()
    {
        base.GeneralUpdate();
    }

    public void HumanEnter(int index, GameObject Obj)
    {
        //リスト管理が理想
        //Data data = new Data(Obj, index);
        //List_Data.Add(data);
        //Debug.Log(List_Data[0].Index);
        List_Index.Add(index);
        //for (int i = 0; i < List_Index.Count; i++)
        //{
        //    Debug.Log("Enter " + i + List_Index[i]);
        //}

        m_bHumanInTheRoom[index] = true;
    }

    public void HumanExit(int index, GameObject Obj)
    {
        //Data data = new Data(Obj, index);
        //IEnumerable<Data> query = List_Data.Where(s => s.Obj == Obj );
        //foreach (var player in query)
        //{
        //    //Debug.Log(player.Index);
        //    List_Data.Remove(player);
        //}
        List_Index.Remove(index);
        //for (int i = 0; i < List_Index.Count; i++)
        //{
        //    Debug.Log("Exit " + i + List_Index[i]);
        //}

        m_bHumanInTheRoom[index] = false;
    }

    public void FarOffHuman(List<GameObject> RespObj, List<GameObject> RespList)
    {
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
        for(int i = 0; i < RespObj.Count ; i++)
        {
            if (!List_Index.Contains(i))
            {
                RespList.Add(RespObj[i]);
                //Debug.Log(RespObj[i]);
            }
        }
    }
}
