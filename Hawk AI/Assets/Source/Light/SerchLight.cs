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
    private SpriteRenderer m_cSpriteRend = new SpriteRenderer();
    private Sprite m_cSprite;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            m_cSpotLight.Add(gameObject.transform.GetChild(i).gameObject);
        }
        m_cSpotLight[(int)ESpotLightChild.eSpotLight].SetActive(false);


        //m_cLight = m_cSpotLight[(int)ESpotLightChild.eSpotLight].GetComponent<Light>();
        m_cSpriteRend = m_cSpotLight[(int)ESpotLightChild.eSprite].GetComponent<SpriteRenderer>();
        //Material[] materials = m_cSpriteRend.materials;

        //foreach (var material in materials)
        //{
        //    Color col = m_cSpriteRend.color;
        //    material.SetColor("_Color", col);

        //    //if (material != null)
        //    //{//オブジェクトにマテリアルがセットされている場合

        //    //    //マテリアルの元々設定されている色を取得
        //    //    if (material.HasProperty("_Color") == true)
        //    //    {
        //    //        col = material.GetColor("_Color");
        //    //        //シェーダーに渡す変数
        //    //        material.SetColor("_Albedo", col);
        //    //    }
        //    //    else
        //    //    {//error log
        //    //     // Debug.LogError("material.HasProperty is Not Material Color !");
        //    //    }

        //    //}
        //}

        m_cSprite = m_cSpriteRend.sprite;
        m_cSpriteRend.sprite = null;
        

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.rotation
            = Quaternion.identity;
    }

    public void TargetObject(GameObject _TargetFromObject, GameObject _TargetToObject)
    {
        //for (int i = 0; i < m_cSpotLight.Count; i++)
        //{
        //    m_cSpotLight[i].SetActive(true);
        //}

        m_cSpotLight[(int)ESpotLightChild.eSpotLight].SetActive(true);

        m_cSpriteRend.sprite = m_cSprite;

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
        m_cSpotLight[(int)ESpotLightChild.eSpotLight].SetActive(false);
        m_cSpriteRend.sprite = null;

        //for (int i = 0; i < m_cSpotLight.Count; i++)
        //{
        //    m_cSpotLight[i].SetActive(false);
        //}

        //m_cSpotLight[(int)ESpotLightChild.eSpotLight].GetComponent<Light>().S = null;
        //m_cSpotLight[(int)ESpotLightChild.eSpotLight].GetComponent<SpriteRenderer>();

    }

}
