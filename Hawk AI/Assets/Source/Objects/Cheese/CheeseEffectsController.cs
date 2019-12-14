using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface ICheeseEffect : IEventSystemHandler
{
    void Play();

    void Stop();
}

public class CheeseEffectsController : MonoBehaviour, ICheeseEffect
{
    [SerializeField]
    private GameObject TimeZoneObj;

    private List<Vector3> m_cInitialScales = new List<Vector3>();

    [SerializeField]
    private List<GameObject> ParticleList = new List<GameObject>();

    [SerializeField]
    private List<Image> CheeseColorCanvas = new List<Image>();

    private List<Color> CheeseColor = new List<Color>();

    private bool m_bStartFlg = false;

    [SerializeField]
    private int ExpandEffectPhaseAtMorning;
    [SerializeField]
    private int ExpandEffectPhaseAtEvening;

    private int m_nPhaseCount = 0;

    [SerializeField]
    private float ExpandEffectRate;
    [SerializeField]
    private float ExpandEffectTime;

    private float m_fTimeCounter = 0f;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ParticleList.Count; i++)
        {
            m_cInitialScales.Add(ParticleList[i].transform.localScale);
        }

        for (int i = 0; i < CheeseColorCanvas.Count; i++)
        {
            CheeseColor.Add(CheeseColorCanvas[i].color);

            CheeseColorCanvas[i].color = new Color(
                CheeseColorCanvas[i].color.r,
                CheeseColorCanvas[i].color.g,
                CheeseColorCanvas[i].color.b,
                0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Play();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Stop();
        }

        ExpandingEffects();

        if(m_bStartFlg == true)
        MaskingPlayerCamera();
    }

    private void ExpandingEffects()
    {
        if (m_bStartFlg == true)
        {
            ETimeZone eTimeZone = ETimeZone.eMorning;

            ExecuteEvents.Execute<IETimeZone>(
            target: TimeZoneObj,
            eventData: null,
            functor: (recieveTarget, y) => eTimeZone = recieveTarget.TimeZoneStatus);

            if (eTimeZone == ETimeZone.eMorning)
            {
                if (m_nPhaseCount >= ExpandEffectPhaseAtMorning)
                {
                    return;
                }
            }
            else
            {
                if (m_nPhaseCount >= ExpandEffectPhaseAtEvening)
                {
                    return;
                }

            }

            m_fTimeCounter += Time.deltaTime;

            if (m_fTimeCounter >= ExpandEffectTime)
            {//デカくする処理
                ParticleList[0].transform.localScale = new Vector3
                (ParticleList[0].transform.localScale.x + ExpandEffectRate,
                ParticleList[0].transform.localScale.y + ExpandEffectRate,
                ParticleList[0].transform.localScale.z + ExpandEffectRate);

                m_nPhaseCount++;
                m_fTimeCounter = 0f;
            }
        }
    }

    private void MaskingPlayerCamera()
    {
        //Hack : Playerがどこのエリアにいるか

        for (int i = 0; i < CheeseColorCanvas.Count; i++)
        {
            // TODO : 自分と同じエリアであれば
            //if ()
            //{
            CheeseColorCanvas[i].color = CheeseColor[i];
            //}
        }

    }



    public void Play()
    {
        foreach (var val in ParticleList)
        {
            if(val.GetComponent<ParticleSystem>().isPlaying == false)
            val.GetComponent<ParticleSystem>().Play();
        }
        m_bStartFlg = true;

    }

    public void Stop()
    {
        foreach (var val in ParticleList)
        {
            val.gameObject.transform.localScale = m_cInitialScales[0];
            val.GetComponent<ParticleSystem>().Stop();
        }

        m_nPhaseCount = 0;
        m_fTimeCounter = 0f;

        m_bStartFlg = false;
    }


}
