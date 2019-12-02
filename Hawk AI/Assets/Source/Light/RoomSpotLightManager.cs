using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IRoomSpotLightManager : IEventSystemHandler
{
    void LightOn();

    void LightOff();

    bool IsLightOn();
}

public enum ERoomSpotLightState
{
    eLightOn,
    eLightOff
}

public class RoomSpotLightManager : GeneralManager, IRoomSpotLightManager
{
    ERoomSpotLightState m_eRoomLightState = ERoomSpotLightState.eLightOff;

    public void LightOn()
    {
        m_eRoomLightState = ERoomSpotLightState.eLightOn;

        foreach (var obj in m_cGameObjects)
        {
            obj.SetActive(true);
        }
    }

    public void LightOff()
    {
        m_eRoomLightState = ERoomSpotLightState.eLightOff;

        foreach (var obj in m_cGameObjects)
        {
            obj.SetActive(false);
        }
    }

    public bool IsLightOn()
    {
        if(m_eRoomLightState == ERoomSpotLightState.eLightOn)
        return true;

        return false;
    }


    public override void GeneralInit()
    {
        m_eRoomLightState = ERoomSpotLightState.eLightOff;
        base.GeneralInit();
        this.LightOff();

    }

    public override void GeneralUpdate()
    {
        base.GeneralUpdate();
    }

    public override void GeneralRelease()
    {
        base.GeneralRelease();
    }

    public override void DebugUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            this.LightOn();
        }
        //base.DebugUpdate();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override GameObject GetGameObject(int _ID)
    {
        return base.GetGameObject(_ID);
    }

    public override GameObject GetGameObject(string _Str)
    {
        return base.GetGameObject(_Str);
    }

    public override List<GameObject> GetGameObjectsList()
    {
        return base.GetGameObjectsList();
    }

}
