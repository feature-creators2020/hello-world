using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : PostEffect,IFadeInterfase
{
    EFadeState m_eFadeState = EFadeState.FadeStay;

    [SerializeField]
    private float m_fSpeed;

    private Vector2 m_cTopUV = new Vector2();
    private Vector2 m_cUnderUV = new Vector2();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        m_cTopUV[1] = 2f;
        m_cUnderUV[1] = 1f;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    CallFadeOut();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    CallFadeIn();
        //}
    }

    protected override void UpdateMaterial()
    {
        switch(m_eFadeState)
        {
            case EFadeState.FadeIn:

                m_cTopUV[1] -= m_fSpeed;
                m_cMaterial.SetVector("_TopUV", m_cTopUV);
                m_cUnderUV[1] -= m_fSpeed;
                m_cMaterial.SetVector("_UnderUV", m_cUnderUV);

                if (m_cTopUV[1] <= 0f)
                {
                    Init();
                    CallFadeStay();
                }

                break;
            case EFadeState.FadeOut:

                m_cTopUV[1] -= m_fSpeed;
                m_cMaterial.SetVector("_TopUV", m_cTopUV);
                m_cUnderUV[1] -= m_fSpeed;
                m_cMaterial.SetVector("_UnderUV", m_cUnderUV);

                if (m_cUnderUV[1] <= 0f)
                {
                    CallFadeStay();
                }


                break;
            case EFadeState.FadeStay:
                break;
        }


    }

    public void CallFadeIn()
    {
        m_eFadeState = EFadeState.FadeIn;
    }
    public void CallFadeOut()
    {
        m_eFadeState = EFadeState.FadeOut;

    }

    public void CallFadeStay()
    {
        m_eFadeState = EFadeState.FadeStay;
    }


}
