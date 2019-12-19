﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IVarsanTrapInterface : IEventSystemHandler
{
    void SetRoom(int _room);

}

public class VarsanTrap : GeneralObject, IVarsanTrapInterface, IMouseTrap
{

    float m_fLifeTime;                  // トラップの生存時間
    float m_fMaxLifeTime;               // トラップの最大生存時間
    bool m_isActive = false;            // カウントを始める
    int m_nRoomNum;                     // 存在しているルームの番号

    GameObject m_gHavePlayer;   // 所有者


    // Start is called before the first frame update
    void OnEnable()
    {

    }

    // Update is called once per frame
    public override void GeneralUpdate()
    {
        if (m_isActive)
        {
            if(m_fLifeTime >= m_fMaxLifeTime)
            {
                // 生存時間終了
                Destroy(this.gameObject);
                return;
            }
            m_fLifeTime += Time.deltaTime;  // 生存時間カウント

            // 全プレイヤーにバルサン影響の判定をする
            UpdateVarsanImpact();

        }
    }

    void UpdateVarsanImpact()
    {
        var manager = ManagerObjectManager.Instance;
        var playermanager = manager.GetGameObject("PlayerManager").GetComponent<PlayerManager>();
        var MouseList = playermanager.GetGameObjectsList("Mouse");
        var HumanList = playermanager.GetGameObjectsList("Human");
        var RespObj = manager.GetGameObject("LifeCycleManager").transform.Find("Respawn").gameObject;
        GameObject _gameObject;
        int i;
        int _roomID = 99;

        for (i = 0; i < MouseList.Count; i++)
        {
            _gameObject = playermanager.GetGameObject(i, "Mouse");
            ExecuteEvents.Execute<IMouseInterface>(
                    target: _gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => _roomID = recieveTarget.GetRoomID());
            if (CheckRoomMatch(_roomID))
            {
                ExecuteEvents.Execute<IMouseInterface>(
                    target: _gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.StartVarsan());
            }
            else
            {
                ExecuteEvents.Execute<IMouseInterface>(
                    target: _gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.StopVarsan());
            }
        }

       
        for (i = 0; i < HumanList.Count; i++)
        {
            _gameObject = playermanager.GetGameObject(i, "Human");
            ExecuteEvents.Execute<IHumanInterface>(
                    target: _gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => _roomID = recieveTarget.GetRoomID());
            if (CheckRoomMatch(_roomID))
            {
                ExecuteEvents.Execute<IHumanInterface>(
                    target: _gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.StartVarsan());
            }
            else
            {
                ExecuteEvents.Execute<IHumanInterface>(
                    target: _gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.StopVarsan());
            }
        }

        ExecuteEvents.Execute<IRespawnInterface>(
                    target: RespObj,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.SetVarsanRoomID(m_nRoomNum));

    }

    public override void OnDestroy()
    {
        var manager = ManagerObjectManager.Instance;
        var playermanager = manager.GetGameObject("PlayerManager").GetComponent<PlayerManager>();
        var MouseList = playermanager.GetGameObjectsList("Mouse");
        var HumanList = playermanager.GetGameObjectsList("Human");
        var RespObj = manager.GetGameObject("LifeCycleManager").transform.Find("Respawn").gameObject;
        GameObject _gameObject;
        int i;

        for (i = 0; i < MouseList.Count; i++)
        {
            _gameObject = playermanager.GetGameObject(i, "Mouse");
            ExecuteEvents.Execute<IMouseInterface>(
                target: _gameObject,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.EndVarsan());
        }

        for (i = 0; i < HumanList.Count; i++)
        {
            _gameObject = playermanager.GetGameObject(i, "Human");
            ExecuteEvents.Execute<IHumanInterface>(
                target: _gameObject,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.EndVarsan());
        }

        ExecuteEvents.Execute<IRespawnInterface>(
                    target: RespObj,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.RemoveVarsanRoomId());

        var varsanitemspawn = manager.GetGameObject("VarsanItemSpawnManager").GetComponent<VarsanItemSpawnManager>().GetGameObject("VarsanItemSpawn");
        ExecuteEvents.Execute<IVarsanItemSpawnInterfase>(
               target: varsanitemspawn,
               eventData: null,
               functor: (recieveTarget, y) => recieveTarget.SetDefault());
    }

    bool CheckRoomMatch(int _roomID)
    {
        return (m_nRoomNum == _roomID);
    }

    public void SetRoom(int _room)
    {
        m_nRoomNum = _room;
        m_fLifeTime = 0f;
        m_fMaxLifeTime = 20f;
        m_isActive = true;
        Debug.Log("SetVarsan");
    }

    public void SetPlayer(GameObject _Human)
    {
        m_gHavePlayer = _Human;
    }
}
