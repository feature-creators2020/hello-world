using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//HACK : サウンドの追加ごとに列挙型を追加するので、あまりよくない
public enum BGMAudioType
{
    Title,
    Main,
    Result
}


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

