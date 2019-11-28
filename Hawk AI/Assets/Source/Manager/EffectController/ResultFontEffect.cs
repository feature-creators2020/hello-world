using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EResultFontEffect
{
    eLeft,
    eRight
}

public class ResultFontEffect : EffectController
{
    private List<GameObject> m_cRawImages = new List<GameObject>();

    // Start is called before the first frame update
    public virtual void CallStart()
    {
        particles = new List<ParticleSystem>();

        //base.Start();

        for(int i = 0; i < this.gameObject.transform.childCount;i++)
        {
            //Particles
            if (this.gameObject.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>() != null)
            {
                particles.Add(this.gameObject.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>());
            }

            //RawImages
            m_cRawImages.Add(this.gameObject.transform.GetChild(i).gameObject);
            //m_cRawImages[i].SetActive(false);

        }

        foreach (var val in particles)
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
        if (particles.Count != 0)
        {
            base.Play(_ID);
        }
        else
        {
            m_cRawImages[_ID].SetActive(true);
        }
    }
    public override void Stop(int _ID)
    {
        if (particles.Count != 0)
        {
            base.Stop(_ID);
        }
        else
        {
            m_cRawImages[_ID].SetActive(false);
        }
    }

    public override void Pause(int _ID)
    {
        if (particles.Count != 0)
        {
            base.Pause(_ID);
        }
        else
        {
            m_cRawImages[_ID].SetActive(false);
        }
    }
}
