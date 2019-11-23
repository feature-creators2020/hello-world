using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ELoseSide
{
    eLeft,
    eRight
}


public enum ELoseEffectsType
{
    eWind,
    eGaan
}


public enum ELoseEffectsComponents
{
    eLerpObjects,
    eSpriteObjects,
    eParticleObjects,
}

public interface ILoseEffectInterface : IEventSystemHandler
{
    void GetListObject(ELoseSide _Side);

    void PlayEffects(ELoseSide _Side, ELoseEffectsType _Type);
}

public class LoseEffect : MonoBehaviour, ILoseEffectInterface
{
    private List<GameObject> m_cLerpObj = new List<GameObject>();
    private List<GameObject> m_cGaanSprtieObjects = new List<GameObject>();
    private GameObject m_cEffectsObj;
    private bool m_bEffectsPlay = false;
    private int m_nLerpCount = 0;
    private float m_fTimeCnt = 0f;

    [SerializeField]
    private float LerpTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (m_bEffectsPlay == true)
        //{
        //    if ((m_nLerpCount + 1) < m_cLerpObj.Count)
        //    {
        //        Vector3 StartPos = m_cLerpObj[m_nLerpCount].transform.position - this.gameObject.transform.position;
        //        Vector3 EndPos = m_cLerpObj[m_nLerpCount + 1].transform.position - m_cLerpObj[m_nLerpCount].transform.position;
        //        float MaxAngle = Vector3.Dot(StartPos, EndPos);
        //        float CalcAngle = Mathf.LerpAngle(this.transform.rotation.z, this.transform.rotation.z + MaxAngle, m_fTimeCnt);
        //        Debug.Log("CalcAngle :" + CalcAngle);
        //        m_cEffectsObj.transform.GetChild((int)EWindEffectsComponents.eSpriteObjects).transform.position = Vector3.Slerp(m_cLerpObj[m_nLerpCount].transform.position, m_cLerpObj[m_nLerpCount + 1].transform.position, m_fTimeCnt);

        //        //Vector3 StartRot = this.transform.rotation;
        //        //Vector3 EndRot = m_cLerpObj[m_nLerpCount + 1].transform.rotation;
        //        Vector3 vector3 = m_cEffectsObj.transform.GetChild((int)EWindEffectsComponents.eSpriteObjects).transform.eulerAngles;
        //        m_cEffectsObj.transform.GetChild((int)EWindEffectsComponents.eSpriteObjects).transform.eulerAngles = new Vector3(vector3.x, vector3.y, vector3.z + CalcAngle); 

        //        m_fTimeCnt += Time.deltaTime / LerpTime;


        //        if (m_fTimeCnt >= 1f)
        //        {
        //            m_fTimeCnt = 0f;
        //            m_nLerpCount++;
        //        }

        //    }
        //    else
        //    {
        //        m_nLerpCount = 0;
        //    }

        //}
    }

    public void GetListObject(ELoseSide _Side)
    {
        m_bEffectsPlay = true;
        GameObject obj = new GameObject();
        Debug.Log("Call GetListObject");

        switch (_Side)
        {
            case ELoseSide.eLeft:

                m_cEffectsObj = gameObject.transform.GetChild(0).gameObject.transform.GetChild((int)ELoseSide.eLeft).gameObject;

                obj = m_cEffectsObj.transform.GetChild((int)ELoseEffectsComponents.eLerpObjects).gameObject;

                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    m_cLerpObj.Add(obj.transform.GetChild(i).gameObject);
                }

                break;

            case ELoseSide.eRight:

                m_cEffectsObj = gameObject.transform.GetChild(0).gameObject.transform.GetChild((int)ELoseSide.eRight).gameObject;

                obj = m_cEffectsObj.transform.GetChild((int)ELoseEffectsComponents.eLerpObjects).gameObject;

                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    m_cLerpObj.Add(obj.transform.GetChild(i).gameObject);
                }

                break;
        }
    }

    public void PlayEffects(ELoseSide _Side, ELoseEffectsType _Type)
    {
        GameObject obj = new GameObject();
        GameObject sprobj = new GameObject();

        switch (_Type)
        {
            case ELoseEffectsType.eWind:

                m_cEffectsObj = gameObject.transform.GetChild((int)_Type).gameObject.transform.GetChild((int)_Side).gameObject;

                obj = m_cEffectsObj.transform.GetChild((int)ELoseEffectsComponents.eParticleObjects).gameObject;

                if (obj.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying == false)
                    obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

                break;

            case ELoseEffectsType.eGaan:

                m_cEffectsObj = gameObject.transform.GetChild((int)_Type).gameObject.transform.GetChild((int)_Side).gameObject;

                obj = m_cEffectsObj.transform.GetChild((int)ELoseEffectsComponents.eParticleObjects).gameObject;

                if (obj.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying == false)
                    obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

                obj = this.gameObject.transform.GetChild((int)ELoseEffectsType.eGaan).
                gameObject.transform.GetChild((int)ELoseSide.eLeft).gameObject;
                m_cGaanSprtieObjects.Add(obj.transform.GetChild((int)ELoseEffectsComponents.eSpriteObjects).gameObject);

                obj = this.gameObject.transform.GetChild((int)ELoseEffectsType.eGaan).
                    gameObject.transform.GetChild((int)ELoseSide.eRight).gameObject;
                m_cGaanSprtieObjects.Add(obj.transform.GetChild((int)ELoseEffectsComponents.eSpriteObjects).gameObject);


                foreach (var val in m_cGaanSprtieObjects)
                {
                    val.SetActive(false);
                }

                m_cGaanSprtieObjects[(int)_Side].SetActive(true);
                //sprobj = m_cEffectsObj.transform.GetChild((int)ELoseEffectsComponents.eSpriteObjects).gameObject;



                obj = m_cEffectsObj.transform.GetChild((int)ELoseEffectsComponents.eLerpObjects).gameObject;

                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    m_cLerpObj.Add(obj.transform.GetChild(i).gameObject);
                }

                StartCoroutine(LerpCoroutine(m_cGaanSprtieObjects[(int)_Side].transform.GetChild(0).gameObject));

                break;
        }
    }

    IEnumerator LerpCoroutine(GameObject sprobj)
    {
        float lerpVal = 0f;

        while ((m_nLerpCount + 1) < m_cLerpObj.Count)
        {
            Vector3 StartPos = m_cLerpObj[m_nLerpCount].transform.localPosition;
            Vector3 EndPos = m_cLerpObj[m_nLerpCount + 1].transform.localPosition;

            while (lerpVal <= 1f)
            {

                sprobj.transform.localPosition = Vector3.Lerp(StartPos, EndPos, lerpVal);
                lerpVal += Time.deltaTime / LerpTime;


                yield return null;
            }

            if (lerpVal >= 1f)
            {
                lerpVal = 0f;
                m_nLerpCount++;
            }
        }
    }
}
