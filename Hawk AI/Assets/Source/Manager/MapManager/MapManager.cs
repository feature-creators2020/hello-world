using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;



public enum ObjectNo
{
    NONE,
    PLAYER,
    SEHLF_HORIZON,
    SEHLF_VERTICAL,
    CORRUGATED_BOARD_1,
    CORRUGATED_BOARD_2,
    UNCLIMB_OBJECT,
    MAP_COLLIDERBOX,
    MOUSE_TRAP_LOW,
    RAIL_STRAIGHT_HORIZON,
    RAIL_STRAIGHT_VERTICAL, //10
    RAIL_END,
    RAIL_BEND,
    LEVER_SWITCH,
    DOOR,
    RAIL_TESTBOX,
    RAIL_ARM,
    RAIL_TUNNEL,

    NUM_OBJECT_TYPE,
}

public enum PipeObjectNo
{
    NONE,
    PIPE_HENOJI_̚,
    PIPE_HENOJI_I̚,
    PIPE_HENOJI_IL,
    PIPE_HENOJI_L,
    PIPE_LONG_VERTICAL,
    PIPE_LONG_HORIZONTAL,
    PIPE,
    PIPE_SHORT_VERTICAL,
    PIPE_SHORT_HORIZONTAL,
    PIPE_SLOPE,
}

public enum EObjectDataTable
{
    Spase,
    Width,
    Height,
};

public partial class MapManager : SingletonMonoBehaviour<MapManager>
{
    public TextAsset CSVMap;
    public TextAsset CSVPipeMap;
    public GameObject[] ObjectType; // マップチップのNoどおりにゲームオブジェクトをセット
    public GameObject[] PipeObjectType; // マップチップのNoどおりにゲームオブジェクトをセット
    Vector3 Initpos = Vector3.zero; // 配置場所
    private List<string[]> stringData = new List<string[]>();
    public List<int[]> InitMapData = new List<int[]>();
    public List<int[]> MapData = new List<int[]>();
    public List<int[]> MapPipeData = new List<int[]>();

    protected override void Awake()
    {
        base.Awake();
        // read CSV file
        InitMapData = CsvRead(CSVMap);
        MapPipeData = CsvRead(CSVPipeMap);

        // Instantiate objects
        MapCreate(InitMapData);
        PipeCreate(MapPipeData);

        // setting use map
        MapData = InitMapData;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (var i = 0; i < InitMapData.Count; i++)
            {
                for (var j = 0; j < InitMapData[i].Length; j++)
                {
                    //if(MapData[i][j] == (int)ObjectNo.PLAYER)
                    Debug.Log("InitMapData[" + i + "][" + j + "] = " + InitMapData[i][j]);
                }
            }
        }

    }

    /// <summary>
    ///  CSVファイル読み込み用
    /// </summary>
    /// <param name="csv">CSVファイル</param>
    /// <returns></returns>
    private List<int[]> CsvRead(TextAsset csv)
    {
        List<int[]> mapData = new List<int[]>();
        StringReader reader = new StringReader(csv.text);
        stringData = new List<string[]>();

        // CSV読み込み
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            stringData.Add(line.Split(','));
        }

        // stringをintに変換
        for (var i = 0; i < stringData.Count; i++)
        {
            int[] mapline = new int[stringData[i].Length];
            for (var j = 0; j < stringData[i].Length; j++)
            {
                mapline[j] = int.Parse(stringData[i][j]);
            }
            mapData.Add(mapline);
        }

        return mapData;
    }

    /// <summary>
    /// 使用配列リストの初期化
    /// </summary>
    /// <param name="maplist">初期化したいリスト</param>
    private void ListInit(ref List<int[]> maplist)
    {
        for (var i = 0; i < stringData.Count; i++)
        {
            int[] mapline = new int[stringData[i].Length];
            for (var j = 0; j < stringData[i].Length; j++)
            {
                mapline[j] = 0;
            }
            maplist.Add(mapline);
        }
    }

    /// <summary>
    /// マップをCSVデータから生成
    /// </summary>
    private void MapCreate(List<int[]> maplist)
    {
        //MapDataをもとにマップを生成 and 仕分け
        for (var i = 0; i < maplist.Count; i++)
        {
            for (var j = 0; j < maplist[i].Length; j++)
            {
                //Debug.Log("csv[" + i + "][" + j + "] = " + MapData[i][j]);

                Initpos = new Vector3(j, 0, maplist.Count - 1 - i);

                if (maplist[i][j] != (int)ObjectNo.NONE)
                {    
                    Instantiate(ObjectType[maplist[i][j]], Initpos, ObjectType[maplist[i][j]].transform.rotation);
                }
                else
                {//当たり判定用のボックス生成
                    Instantiate(ObjectType[(int)ObjectNo.MAP_COLLIDERBOX], Initpos, ObjectType[(int)ObjectNo.MAP_COLLIDERBOX].transform.rotation);
                }
            }
        }


    }

    /// <summary>
    /// PipeをCSVデータから生成
    /// </summary>
    private void PipeCreate(List<int[]> maplist)
    {
        for (var i = 0; i < maplist.Count; i++)
        {
            for (var j = 0; j < maplist[i].Length; j++)
            {

                if (maplist[i][j] != (int)PipeObjectNo.NONE)
                {
                    Initpos = new Vector3(j, PipeObjectType[maplist[i][j]].gameObject.transform.position.y, maplist.Count - 1 - i);
                    Instantiate(PipeObjectType[maplist[i][j]], Initpos, PipeObjectType[maplist[i][j]].transform.rotation);
                }
                //else
                //{//当たり判定用のボックス生成
                //    Instantiate(ObjectType[(int)ObjectNo.MAP_COLLIDERBOX], Initpos, ObjectType[(int)ObjectNo.MAP_COLLIDERBOX].transform.rotation);
                //}
            }
        }


    }

    /// <summary>
    /// 追加でオブジェクトを作る用
    /// 配列の位置とオブジェクトのObjectNoを投げると作ってくれます
    /// </summary>
    /// <param name="horizontal">縦</param>
    /// <param name="vertical">横</param>
    /// <param name="objtype">オブジェクトの種類</param>
    public void CreateObject(int vertical, int horizontal, ObjectNo objtype)
    {
        Vector3 Initpos = new Vector3(-(InitMapData[0].Length / 2) + horizontal, 0.0f , (InitMapData.Count / 2) - vertical);
        Instantiate(ObjectType[(int)objtype - 1], Initpos, Quaternion.identity);
    }

    /// <summary>
    /// オブジェクト生成位置決定
    /// </summary>
    /// <param name="_vertical">縦列の番号</param>
    /// <param name="_hotizontal">横列の番号</param>
    /// <param name="_zpos">ｚ座標</param>
    /// <returns></returns>
    public Vector3 CreatePosition(float _horizontal, float _vertical, float _zpos)
    {
        Vector3 InitPos = new Vector3(-(InitMapData[0].Length / 2) + _horizontal, (InitMapData.Count / 2) - _vertical, _zpos);
        return InitPos;
    }

}
