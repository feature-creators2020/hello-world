using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISerchLight : IEventSystemHandler
{
    void TargetObject(GameObject _TargetFromObject,GameObject _TargetToObject);

    void TargetObjectLost();
}

public enum ESpotLightChild
{
    eSpotLight,
    eSprite,
}

public class SerchLight : MonoBehaviour, ISerchLight
{
    private List<GameObject> m_cSpotLight = new List<GameObject>();
    private Light m_cLight = new Light();
    private SpriteRenderer m_cSprite = new SpriteRenderer();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < gameObject.transform.childCount;i++)
        {
            m_cSpotLight.Add(gameObject.transform.GetChild(i).gameObject);
            m_cSpotLight[i].SetActive(false);
        }


        //m_cLight = m_cSpotLight[(int)ESpotLightChild.eSpotLight].GetComponent<Light>();
        //m_cSprite = m_cSpotLight[(int)ESpotLightChild.eSpotLight].GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TargetObject(GameObject _TargetFromObject, GameObject _TargetToObject)
    {
        for (int i = 0; i < m_cSpotLight.Count; i++)
        {
            m_cSpotLight[i].SetActive(true);
        }

        //var target = new Vector3(
        //    _TargetToObject.gameObject.transform.position.x,
        //    _TargetToObject.gameObject.transform.position.y,
        //    _TargetToObject.gameObject.transform.position.z)
        //    ;
        //var distance = Vector3.Distance(
        //    _TargetToObject.gameObject.transform.position,
        //    _TargetFromObject.gameObject.transform.position);

        //if (distance > 0.01f)
        //{
        //    _TargetFromObject.gameObject.transform.rotation = 
        //        Quaternion.LookRotation(target - _TargetFromObject.transform.position);
        //}

        //_TargetFromObject.transform.position = 
        //    Vector3.Lerp(_TargetFromObject.transform.position, target, 0.1f);

    }


    public void TargetObjectLost()
    {
        for (int i = 0; i < m_cSpotLight.Count; i++)
        {
            m_cSpotLight[i].SetActive(false);
        }

        //m_cSpotLight[(int)ESpotLightChild.eSpotLight].GetComponent<Light>().S = null;
        //m_cSpotLight[(int)ESpotLightChild.eSpotLight].GetComponent<SpriteRenderer>();

    }

}
