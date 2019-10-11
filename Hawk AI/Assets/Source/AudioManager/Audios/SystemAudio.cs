using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HACK : サウンドの追加ごとに列挙型を追加するので、あまりよくない
public enum SystemAudioType
{
    OpeningPause,
    Decide,
    Cancel,
    Select,
    GameClear,
    GameOver, 
}

public class SystemAudio : AudioManager
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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
