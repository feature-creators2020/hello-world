using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : PostEffect,IFadeInterfase
{
    EFadeState m_eFadeState = EFadeState.FadeStay;

    [SerializeField]
    private float m_fSpeed;

    public bool IsCompleteFlg
    {
        get { return m_bCompleteFlg; }
        set { m_bCompleteFlg = value;}
    }

    private bool m_bCompleteFlg = false;

    private Vector2 m_cTopUV = new Vector2();
    private Vector2 m_cUnderUV = new Vector2();

    private void Start()
    {
        m_cTopUV[1] = 2f;
        m_cUnderUV[1] = 1f;

        m_cMaterial.SetVector("_TopUV", m_cTopUV);
        m_cMaterial.SetVector("_UnderUV", m_cUnderUV);


    }

    private void InitFadeInPos()
    {
        m_cTopUV[1] = 1f;
        m_cUnderUV[1] = 0f;
    }

    private void InitFadeOutPos()
    {
        m_cTopUV[1] = 2f;
        m_cUnderUV[1] = 1f;
    }

    private void InitFadeStayPos()
    {
        m_cTopUV[1] = 1f;
        m_cUnderUV[1] = 0f;
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
        InitFadeInPos();
        m_bCompleteFlg = false;
        m_eFadeState = EFadeState.FadeIn;
    }
    public void CallFadeOut()
    {
        InitFadeOutPos();
        m_bCompleteFlg = false;
        m_eFadeState = EFadeState.FadeOut;

    }

    public void CallFadeStay()
    {
        InitFadeStayPos();
        m_bCompleteFlg = true;
        m_eFadeState = EFadeState.FadeStay;
    }


}
