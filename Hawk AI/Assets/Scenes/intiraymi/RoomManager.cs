using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : GeneralManager
{
    public static RoomManager Instance;

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

    public void HumanEnter(int index)
    {
        m_bHumanInTheRoom[index] = true;
    }

    public void HumanExit(int index)
    {
        m_bHumanInTheRoom[index] = false;
    }

    public void FarOffHuman(List<GameObject> RespObj, List<GameObject> RespList)
    {
        for(int i = 0; i < m_bHumanInTheRoom.Length; i++)
        {
            if (!m_bHumanInTheRoom[i])  RespList.Add(RespObj[i]);
        }
    }
}
