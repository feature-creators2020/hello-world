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
    [SerializeField]
    private Texture texture;

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
    protected override void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        UpdateMaterial(src);
        Graphics.Blit(src, dest, m_cMaterial);
    }

    protected override void UpdateMaterial()
    {
        m_cMaterial.SetTexture("_MainTex", texture);
        m_cMaterial.SetColor("_Color", new Color(0,0,0,0));
    }

    private void UpdateMaterial(RenderTexture src)
    {
        m_cMaterial.SetTexture("_MainTex", texture);
        m_cMaterial.SetTexture("_SrcTex", src);

       // m_cMaterial.SetVector("_SrcTextureUV", src.);
    }


}
