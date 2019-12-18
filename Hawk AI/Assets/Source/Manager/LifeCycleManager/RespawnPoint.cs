using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IRespawnInterface : IEventSystemHandler
{
    void SetVarsanRoomID(int _id);

    void RemoveVarsanRoomId();
}


public class RespawnPoint : SingletonMonoBehaviour<RespawnPoint>, IRespawnInterface
{
    [SerializeField]
    private List<GameObject> List_RespObj;
    [SerializeField]
    private List<int> List_RoomIndex;
    private int m_nVarsanRoom = -1;

    //Respawn関数(出現させるオブジェクト)
    public void Respawn(GameObject Obj)
    {
        //人間がいない空間にあるリスポーン地の配列を作成
        List<int> RespList = new List<int>();
        RoomManager.Instance.FarOffHuman(List_RoomIndex, RespList);
        //ランダムで生成場所を決定
        int number;
        Vector3 pos;
        if(m_nVarsanRoom >= 0)
        {
            RespList.Remove(m_nVarsanRoom);
        }
        //もしすべての部屋に人間がいる状況が生まれた場合はすべてのリスポーン地からランダム
        if (RespList.Count <= 0)
        {
            RespList.Clear();
            for (int i = 0; i < List_RespObj.Count; i++)
            {
                RespList.Add(i);
            }
            if (m_nVarsanRoom >= 0)
            {
                RespList.Remove(m_nVarsanRoom);
            }
        }
        number = Random.Range(0, RespList.Count);
        //設定位置に移動
        pos = List_RespObj[RespList[number]].transform.position;
        Obj.transform.position = pos;
    }

    public void SetVarsanRoomID(int _id)
    {
        m_nVarsanRoom = _id;
    }

    public void RemoveVarsanRoomId()
    {
        m_nVarsanRoom = -1;
    }

    //public void RespInit(GameObject Obj,int RoomIndex)
    //{
    //    for(int i = 0;i < List_RespObj.Count; i++)
    //    {
    //        if(List_RespObj[i] == Obj)
    //        {
    //            List_RoomIndex.RemoveAt(i);
    //            List_RoomIndex.Insert(i, RoomIndex);
    //            break;
    //        }
    //    }
    //}
}
