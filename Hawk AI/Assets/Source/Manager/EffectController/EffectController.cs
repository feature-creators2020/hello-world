using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//HOWTO : 必要に応じて増やす
public interface IEffectControllerInterface : IEventSystemHandler
{
    void Play();

    void Play(int _ID);

    void Play(Vector2 _pos);

    void Play(Vector3 _pos);

    void Stop();
    void Stop(int _ID);

    void Pause();
    void Pause(int _ID);

}

/// <summary>
/// エフェクトコントローラ基底クラス
/// </summary>

public abstract class EffectController : MonoBehaviour,IEffectControllerInterface
{
    protected ParticleSystem particle;
    protected List<ParticleSystem> particles;
    protected Vector3 PopPosition;

    // Start is called before the first frame update
    public virtual void Start()
    {
        particle = this.gameObject.GetComponent<ParticleSystem>();
        particle.Stop();
    }


    public virtual void Update()
    {
        //HACK : ポーズで止めたい場合は要検討
        //if (global::PauseManager.IsPause == false)
        //{
        //    Play();
        //}
        //else
        //{
        //    Pause();
        //}

    }

    public virtual void Play()
    {
        if (particle.isPlaying == false)
        {
            particle.Play();
        }
    }

    public virtual void Play(int _ID)
    {
        if (particles[_ID].isPlaying == false)
        {
            particles[_ID].Play();
        }
    }

    public virtual void Play(Vector2 _pos)
    {
        if (particle.isPlaying == false)
        {
            particle.gameObject.transform.position = _pos;
            PopPosition = _pos;
            particle.Play();
        }
    }

    public virtual void Play(Vector3 _pos)
    {
        if (particle.isPlaying == false)
        {
            particle.gameObject.transform.position = _pos;
            PopPosition = _pos;
            particle.Play();
        }
    }

    public virtual void Stop()
    {
        if (particle.isPlaying == true)
        {
            particle.Stop();
        }
    }

    public virtual void Stop(int _ID)
    {
        if (particles[_ID].isPlaying == true)
        {
            particles[_ID].Stop();
        }
    }
    public virtual void Pause()
    {
        if (particle.isPlaying == true)
        {
            particle.Pause();
        }
    }

    public virtual void Pause(int _ID)
    {
        if (particles[_ID].isPlaying == true)
        {
            particles[_ID].Pause();
        }
    }
}
