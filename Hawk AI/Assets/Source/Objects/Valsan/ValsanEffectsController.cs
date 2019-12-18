using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IValsanEffect : IEventSystemHandler
{
    void Play(int _nPlayerRoomNo);

    void Stop(int _nPlayerRoomNo);

    void End();

    void SetSignalFirePos(Vector3 _pos);
}

public enum EPlayerRoom
{
    ePlayerRoom1,
    ePlayerRoom2,
    ePlayerRoom3,
    ePlayerRoom4,
}


public enum EValsanEffectsState
{
    ePlay,
    eStop,
    eEnd
}


public class ValsanEffectsController : MonoBehaviour, IValsanEffect
{
    [SerializeField]
    private List<GameObject> PlayerList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> ParticleList = new List<GameObject>();

    [SerializeField]
    private List<RawImage> SmokeEffectsCanvas = new List<RawImage>();

    [SerializeField]
    private List<Image> SmokeColorCanvas = new List<Image>();

    [SerializeField]
    private float FadeRate;

    private List<Color> SmokeEffects = new List<Color>();
    private List<Color> SmokeColor = new List<Color>();

    private bool m_bStart = false;
    private EValsanEffectsState[] m_eValsanEffectsStates = new EValsanEffectsState[4];

    private float[] m_fValsanEffectsRates = new float[4];
    private float[] m_fValsanEffectsCanvasRates = new float[4];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < m_eValsanEffectsStates.Length;i++)
        {
            m_eValsanEffectsStates[i] = EValsanEffectsState.eEnd;
        }

        for (int i = 0; i < SmokeEffectsCanvas.Count; i++)
        {
            SmokeEffects.Add(SmokeEffectsCanvas[i].color);

            SmokeEffectsCanvas[i].color = new Color(
                SmokeEffectsCanvas[i].color.r,
                SmokeEffectsCanvas[i].color.g,
                SmokeEffectsCanvas[i].color.b,
                0);
        }

        for (int i = 0; i < SmokeColorCanvas.Count; i++)
        {
            SmokeColor.Add(SmokeColorCanvas[i].color);

            SmokeColorCanvas[i].color = new Color(
                SmokeColorCanvas[i].color.r,
                SmokeColorCanvas[i].color.g,
                SmokeColorCanvas[i].color.b,
                0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    Play(EPlayerRoom.ePlayerRoom1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    Stop(EPlayerRoom.ePlayerRoom1);
        //}


        for (int i = 0; i <  m_eValsanEffectsStates.Length;i++)
        {
            switch (m_eValsanEffectsStates[i])
            {
                case EValsanEffectsState.ePlay:
                    PlayEffectsRate(i);
                    break;

                case EValsanEffectsState.eStop:
                    StopEffectsRate(i);
                    break;

                default:
                    break;
            }
        }


        if (m_bStart == true)
        MaskingPlayerCamera();
    }


    private void PlayEffectsRate(int _PlayerNo)
    {
        if (m_fValsanEffectsRates[_PlayerNo] <= SmokeColor[_PlayerNo].a)
        {
            m_fValsanEffectsRates[_PlayerNo] += FadeRate;


            SmokeColorCanvas[_PlayerNo].color = new Color(
                SmokeColor[_PlayerNo].r,
                SmokeColor[_PlayerNo].g,
                SmokeColor[_PlayerNo].b,
                m_fValsanEffectsRates[_PlayerNo]);

            //SmokeEffectsCanvas[_PlayerNo].color = SmokeEffects[_PlayerNo];
            //SmokeColorCanvas[_PlayerNo].color = SmokeColor[_PlayerNo];
        }

        if (m_fValsanEffectsCanvasRates[_PlayerNo] <= SmokeEffects[_PlayerNo].a)
        {
            m_fValsanEffectsCanvasRates[_PlayerNo] += FadeRate;


            SmokeEffectsCanvas[_PlayerNo].color = new Color(
                SmokeEffects[_PlayerNo].r,
                SmokeEffects[_PlayerNo].g,
                SmokeEffects[_PlayerNo].b,
                m_fValsanEffectsCanvasRates[_PlayerNo]);
        }

    }

    private void StopEffectsRate(int _PlayerNo)
    {
        if (m_fValsanEffectsRates[_PlayerNo] >= 0)
        {
            m_fValsanEffectsRates[_PlayerNo] -= FadeRate;

            SmokeColorCanvas[_PlayerNo].color = new Color(
                SmokeColor[_PlayerNo].r,
                SmokeColor[_PlayerNo].g,
                SmokeColor[_PlayerNo].b,
                m_fValsanEffectsRates[_PlayerNo]);
        }

        if (m_fValsanEffectsCanvasRates[_PlayerNo] >= 0)
        {
            m_fValsanEffectsCanvasRates[_PlayerNo] -= FadeRate;


            SmokeEffectsCanvas[_PlayerNo].color = new Color(
                SmokeEffects[_PlayerNo].r,
                SmokeEffects[_PlayerNo].g,
                SmokeEffects[_PlayerNo].b,
                m_fValsanEffectsCanvasRates[_PlayerNo]);
        }

    }

    private void MaskingPlayerCamera()
    {
        //for (int i = 0; i < SmokeEffectsCanvas.Count; i++)
        //{
        //    //自分と同じエリアであれば
        //    if ()
        //    {
        //        SmokeEffectsCanvas[i].color = SmokeEffects[i];
        //    }
        //}

        //for (int i = 0; i < SmokeColorCanvas.Count; i++)
        //{
        //    // TODO : 自分と同じエリアであれば
        //    //if ()
        //    //{
        //        SmokeColorCanvas[i].color = SmokeColor[i];
        //    //}
        //}

    }



    public void Play(int _nPlayerRoomNo)
    {
        int PlayNo = _nPlayerRoomNo - 1;

        Debug.Log("Player" + PlayNo + "EffectPlay");

        if((m_eValsanEffectsStates[PlayNo] == EValsanEffectsState.eStop) ||
            (m_eValsanEffectsStates[PlayNo] == EValsanEffectsState.eEnd))
        {
            m_fValsanEffectsRates[PlayNo] = 0f;
            m_fValsanEffectsCanvasRates[PlayNo] = 0f;
        }

        m_eValsanEffectsStates[PlayNo] = EValsanEffectsState.ePlay;

        foreach (var val in ParticleList)
        {
            val.GetComponent<ParticleSystem>().Play();
        }

        //SmokeEffectsCanvas[playerno].color = SmokeEffects[playerno];
        //SmokeColorCanvas[playerno].color = SmokeColor[playerno];

        m_bStart = true;
    }

    public void Stop(int _nPlayerRoomNo)
    {
        int PlayNo = _nPlayerRoomNo - 1;

        Debug.Log("Player" + PlayNo + "EffectStop");
        m_eValsanEffectsStates[PlayNo] = EValsanEffectsState.eStop;

            //SmokeEffects.Add(SmokeEffectsCanvas[playerno].color);

            //SmokeEffectsCanvas[playerno].color = new Color(
            //    SmokeEffectsCanvas[playerno].color.r,
            //    SmokeEffectsCanvas[playerno].color.g,
            //    SmokeEffectsCanvas[playerno].color.b,
            //    0);
        


            //SmokeColor.Add(SmokeColorCanvas[playerno].color);

            //SmokeColorCanvas[playerno].color = new Color(
            //    SmokeColorCanvas[playerno].color.r,
            //    SmokeColorCanvas[playerno].color.g,
            //    SmokeColorCanvas[playerno].color.b,
            //    0);

        m_bStart = false;
    }

    public void End()
    {
        Debug.Log("PlayerAll EffectEnd");
        for (int i = 0; i < m_eValsanEffectsStates.Length; i++)
        {
            m_eValsanEffectsStates[i] = EValsanEffectsState.eStop;
        }

        //foreach (var val in ParticleList)
        //{
        //    val.GetComponent<ParticleSystem>().Clear(false);
        //}

        foreach (var val in ParticleList)
        {
            val.GetComponent<ParticleSystem>().Stop();
        }

    }

    public void SetSignalFirePos(Vector3 _pos)
    {
        ParticleList[0].transform.position = _pos;
    }
}
