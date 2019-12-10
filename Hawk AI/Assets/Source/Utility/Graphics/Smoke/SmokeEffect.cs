using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISmokeEffect : IEventSystemHandler
{
    void CallFadeIn();
    void CallFadeOut();

}

public class SmokeEffect : PostEffect,ISmokeEffect
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CallFadeIn()
    {

    }

    public void CallFadeOut()
    {


    }

    protected override void UpdateMaterial()
    {


    }

}
