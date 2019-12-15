using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public struct ObjectRoomInfo
{
    public GameObject ObjectInfo { get; set; }
    public int RoomInfo { get; set; }
}

public class RoomManager : GeneralManager
{
    //インスタンス
    public static RoomManager Instance;
    //部屋の番号が入るリスト
    private List<int> List_Human = new List<int>();
    private List<int> List_Mouse01 = new List<int>();
    private List<int> List_Mouse02 = new List<int>();
    private List<int> List_Drone = new List<int>();
    private List<int> List_Debug = new List<int>();

    [SerializeField] private List<ObjectRoomInfo> Object_RoomIDs = new List<ObjectRoomInfo>();

    [SerializeField] private GameObject DroneObject;

    void Awake()
    {
        Instance = this;
    }

    public override void GeneralInit()
    {
        base.GeneralInit();
        CheckRoomInfo();
    }

    public override void GeneralUpdate()
    {
        base.GeneralUpdate();
        //CheckRoomInfo();
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
    public void Mouse01Enter(int index)
    {
        List_Mouse01.Add(index);
        Debug.Log(index);
    }

    //ネズミが出て行ったときに部屋番号をリストから（1つのみ）除外
    public void Mouse01Exit(int index)
    {
        List_Mouse01.Remove(index);
    }

    //ネズミが入ってきたときに部屋番号をリストに追加
    public void Mouse02Enter(int index)
    {
        List_Mouse02.Add(index);
    }

    //ネズミが出て行ったときに部屋番号をリストから（1つのみ）除外
    public void Mouse02Exit(int index)
    {
        List_Mouse01.Remove(index);
    }

    //ネズミがいる部屋番号を読み取り
    public int GetMouse01In()
    {
        if(List_Mouse01.Count > 0)
        {
            return List_Mouse01[0];
        }
        return 0;
    }

    //ネズミ2がいる部屋番号を読み取り
    public int GetMouse02In()
    {
        if(List_Mouse02.Count > 0)
        {
            return List_Mouse02[0];
        }
        return 0;
    }

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

    public void CheckRoomInfo()
    {
        int i, j;
        // それぞれのルーム情報を更新する
        var manager = ManagerObjectManager.Instance;
        var playermanager = manager.GetGameObject("PlayerManager").GetComponent<PlayerManager>();
        var MouseList = playermanager.GetGameObjectsList("Mouse");

        ObjectRoomInfo _inInfo = new ObjectRoomInfo();

        for (i = 0; i < MouseList.Count; i++)
        {
            _inInfo.ObjectInfo = playermanager.GetGameObject(i, "Mouse");
            ExecuteEvents.Execute<IMouseInterface>(
                    target: _inInfo.ObjectInfo,
                    eventData: null,
                    functor: (recieveTarget, y) => _inInfo.RoomInfo = recieveTarget.GetRoomID());
            Debug.Log("Mouse : " + _inInfo.ObjectInfo);
            for (j = 0; j < Object_RoomIDs.Count; j++)
            {
                if(Object_RoomIDs[j].ObjectInfo == _inInfo.ObjectInfo)
                {
                    // すでに情報が入っている
                    Object_RoomIDs[j] = _inInfo;    // 構造体の情報ごと変える必要がある
                    break;
                }
            }
            // 情報が入っていなかった場合
            if (j >= Object_RoomIDs.Count)
            {
                Object_RoomIDs.Add(new ObjectRoomInfo());
                Object_RoomIDs[j] = _inInfo;
            }
        }

        var HumanList = playermanager.GetGameObjectsList("Human");
        for (i = 0; i < HumanList.Count; i++)
        {
            _inInfo.ObjectInfo = playermanager.GetGameObject(i, "Human");
            ExecuteEvents.Execute<IHumanInterface>(
                    target: _inInfo.ObjectInfo,
                    eventData: null,
                    functor: (recieveTarget, y) => _inInfo.RoomInfo = recieveTarget.GetRoomID());
            Debug.Log("Human : " + _inInfo.ObjectInfo);
            for (j = 0; j < Object_RoomIDs.Count; j++)
            {
                if (Object_RoomIDs[j].ObjectInfo == _inInfo.ObjectInfo)
                {
                    // すでに情報が入っている
                    Object_RoomIDs[j] = _inInfo;    // 構造体の情報ごと変える必要がある
                    break;
                }
            }
            // 情報が入っていなかった場合
            if (j >= Object_RoomIDs.Count)
            {
                Object_RoomIDs.Add(new ObjectRoomInfo());
                Object_RoomIDs[j] = _inInfo;
            }
        }

        _inInfo.ObjectInfo = DroneObject;
        ExecuteEvents.Execute<IDroneInterface>(
                target: _inInfo.ObjectInfo,
                eventData: null,
                functor: (recieveTarget, y) => _inInfo.RoomInfo = recieveTarget.GetRoomID());
        Debug.Log("Drone : " + _inInfo.ObjectInfo);
        for (j = 0; j < Object_RoomIDs.Count; j++)
        {
            if (Object_RoomIDs[j].ObjectInfo == _inInfo.ObjectInfo)
            {
                // すでに情報が入っている
                Object_RoomIDs[j] = _inInfo;    // 構造体の情報ごと変える必要がある
                break;
            }
        }
        // 情報が入っていなかった場合
        if (j >= Object_RoomIDs.Count)
        {
            Object_RoomIDs.Add(new ObjectRoomInfo());
            Object_RoomIDs[j] = _inInfo;
        }

        // デバッグ用
        Debug.Log("ObjectCount : " + Object_RoomIDs.Count);
        for(int debug_i = 0; debug_i < Object_RoomIDs.Count; debug_i++)
        {
            if(List_Debug.Count < Object_RoomIDs.Count)
            {
                List_Debug.Add(Object_RoomIDs[debug_i].RoomInfo);
            }
            else
            {
                List_Debug[i] = Object_RoomIDs[debug_i].RoomInfo;
            }
        }
    }

    // 指定したオブジェクトのルーム情報を取得する
    public int GetObjectRoomID(GameObject _object)
    {
        int i = 0;
        int RoomNum = 99;   // 初期でどこにも当てはまらない値を入れる
        for(i=0;i< Object_RoomIDs.Count; i++)
        {
            if(Object_RoomIDs[i].ObjectInfo == _object)
            {
                RoomNum = Object_RoomIDs[i].RoomInfo;
                break;
            }
        }

        // 情報がなかった場合
        //if (i >= Object_RoomIDs.Count) return RoomNum;
        Debug.Log(Object_RoomIDs[i].ObjectInfo.name + ".RoomId : " + RoomNum);
        return RoomNum;
    }
}
