using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EResultFontEffect
{
    eLeft,
    eRight
}

public class ResultFontEffect : EffectController
{
    // Start is called before the first frame update
    public virtual void CallStart()
    {
        particles = new List<ParticleSystem>();

        //base.Start();

        for(int i = 0; i < this.gameObject.transform.childCount;i++)
        {
            if (this.gameObject.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>() != null)
            {
                particles.Add(this.gameObject.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>());
            }
        }

        foreach(var val in particles)
        {
            val.Stop();
        }

    }

    public override void Start()
    {


    }
    public override void Update()
    {


    }

    public override void Play(int _ID)
    {
        base.Play(_ID);
    }
    public override void Stop(int _ID)
    {
        base.Stop(_ID);
    }

    public override void Pause(int _ID)
    {
        base.Pause(_ID);
    }
}
