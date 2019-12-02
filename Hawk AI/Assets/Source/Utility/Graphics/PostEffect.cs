using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public abstract class PostEffect : MonoBehaviour
{
    [SerializeField]
    protected Material m_cMaterial;

    protected virtual void Awake()
    {
    }

    protected virtual void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        UpdateMaterial();
        Graphics.Blit(src, dest, m_cMaterial);
    }

    protected abstract void UpdateMaterial();
}
