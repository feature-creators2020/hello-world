using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//オーディオの再生ステート
public enum AudioStatement
{
    Play,
    Stop,
    Pause,
}

/// <summary>
/// オーディオインターフェース
/// </summary>

//HACK : 引数を定数にした方がいい
public interface IAudioInterface : IEventSystemHandler
{
    //再生
    void Play(int _id);
    //多重再生
    void MultiplePlay(int _id);

    //停止
    void Stop(int _id);
    //一時停止
    void Pause(int _id);
    //Hack : 実際ゲッターと変わらないので、変更できるかも
    //再生中かどうか
    bool isPlaying(int _id);
}

/// <summary>
/// @name  : オーディオマネージャ抽象クラス
/// @brief : 取得した_idのAudioSourceを触る
/// </summary>
/// 
public abstract class AudioManager : MonoBehaviour,IAudioInterface
{
    protected AudioSource[] audioSources;

    // Start is called before the first frame update
    public virtual void Start()
    {
       //自身のAudioSouceコンポーネントを全て取得
       audioSources = this.gameObject.GetComponents<AudioSource>();
       //Debug.LogFormat("audioSources : {0}", audioSources.Length);
        foreach(var audio in audioSources)
        {
            audio.Stop();
        }
    }

    //再生
    public virtual void Play(int _id)
    {
        //Debug.LogFormat("Play Audio : {0}", _id);
        //Debug.LogFormat("audioSources : {0}", audioSources[_id].clip);

        if (audioSources[_id].isPlaying == false)
        {
            audioSources[_id].Play();
        }
    }

    //多重再生
    public virtual void MultiplePlay(int _id)
    {
        audioSources[_id].Play();
    }

    //停止
    public virtual void Stop(int _id)
    {
        if (audioSources[_id].isPlaying == true)
        {
            audioSources[_id].Stop();
        }
    }

    //一時停止
    public virtual void Pause(int _id)
    {
        if (audioSources[_id].isPlaying == true)
        {
            audioSources[_id].Pause();
        }
    }


    //再生中かどうか
    public virtual bool isPlaying(int _id)
    {
        return audioSources[_id].isPlaying;
    }
}
