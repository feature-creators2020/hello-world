using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HACK : サウンドの追加ごとに列挙型を追加するので、あまりよくない
public enum SEAudioType
{
    eSE_OK,
    eSE_HumanRunning,
    eSE_MouseRunning,
    eSE_Pipe,
    eSE_GetTrap,
    eSE_SetTrap,
    eSE_MouseCatching,
    eSE_GetCheese,
    eSE_Jump,
    eSE_DoorOpen,
    eSE_DoorClose,
    eSE_Discovery,
    eSE_FallOutItem,
    eSE_Debuff
}

public class SEAudio : AudioManager
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

    public override void MultiplePlay(int _id)
    {
        base.MultiplePlay(_id);
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
