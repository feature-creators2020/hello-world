using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//HACK : サウンドの追加ごとに列挙型を追加するので、あまりよくない
//public enum BGMAudioType
//{
//    AREA_1,         //エリア１
//    AREA_2,         //エリア２
//    AREA_3,         //エリア３
//    StageSelect,    //ステージセレクト
//    Title,          //タイトル
//}


/// <summary>
/// BGMのオーディオコンポーネントをまとめたクラス
/// </summary>
public class BGMAudio : AudioManager
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        base.Play(0);
    }

    public override void Play(int _id)
    {
        base.Play(_id);
    }

    public override void Stop(int _id)
    {
        base.Stop(_id);
    }

    public override void Pause(int _id)
    {
        base.Pause(_id);
    }

    public override bool isPlaying(int _id)
    {
        return audioSources[_id].isPlaying;
    }

}

