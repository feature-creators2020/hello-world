using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別設定用のアウトラインシェーダー
/// </summary>
public class IndividualOutlineController : MonoBehaviour
{
    [SerializeField] private Color OutlineColor;        //アウトラインサイズ
    [SerializeField] private float OutlineSize;        //アウトラインサイズ
    [SerializeField] private Material EleMaterial;    //元々のシェーダーマテリアル
    [SerializeField] private Texture2D Texture;        //テクスチャ有の場合設定
    [SerializeField] private bool isNumMaterials;
    private GameObject m_cLightObj;

    void Start()
    {
        m_cLightObj = GameObject.Find("Directional Light");

        if(m_cLightObj == null)
        {
            Debug.LogError("m_cLightObj is null.");
        }

        if (isNumMaterials == false)
        {
            OneToMaterial();
        }
        else
        {
            NumToMaterials();
        }

    }

    void OneToMaterial()
    {
        Material material = null;

        // HACK : switchに直した方がよい
        if (this.gameObject.GetComponent<SkinnedMeshRenderer>() == true)
        {
            material = this.gameObject.GetComponent<SkinnedMeshRenderer>().material;
        }
        else if (this.gameObject.GetComponent<MeshRenderer>() == true)
        {
            material = this.gameObject.GetComponent<MeshRenderer>().material;
        }
        else
        {//Error Log
            Debug.LogError("Not Having Mesh Component");
        }

        //デフォルトカラー
        Color col = Color.cyan;

        if (material != null)
        {//オブジェクトにマテリアルがセットされている場合

            //マテリアルに元々設定されている色を取得
            if (material.HasProperty("_Color") == true)
            {
                col = material.GetColor("_Color");
            }
            else
            {//error log
                Debug.LogError("material.HasProperty is Not Material Color !");
            }

            //後に描画したいシェーダーに変更
            material.shader = EleMaterial.shader;

            //シェーダーに渡す変数
            Vector4 vector4 =
                new Vector4(m_cLightObj.gameObject.transform.position.x,
                m_cLightObj.gameObject.transform.position.y,
                m_cLightObj.gameObject.transform.position.z,
                0f);

            material.SetColor("_OutlineColor", OutlineColor);
            material.SetColor("_Albedo", col);
            material.SetFloat("_OutlineSize", OutlineSize);
            material.SetVector("_DirectionalLight", vector4);

            if (Texture == true)
            {
                material.SetTexture("Texture", Texture);
            }
        }


    }


    void NumToMaterials()
    {
        Material[] materials = null;

        // HACK : switchに直した方がよい
        if (this.gameObject.GetComponent<SkinnedMeshRenderer>() == true)
        {
            materials = this.gameObject.GetComponent<SkinnedMeshRenderer>().materials;
        }
        else if (this.gameObject.GetComponent<MeshRenderer>() == true)
        {
            materials = this.gameObject.GetComponent<MeshRenderer>().materials;
        }
        else
        {//Error Log
            Debug.LogError("Not Having Mesh Component");
        }

        //デフォルトカラー
        Color col = Color.cyan;

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials != null)
            {//オブジェクトにマテリアルがセットされている場合

                //マテリアルに元々設定されている色を取得
                if (materials[i].HasProperty("_Color") == true)
                {
                    col = materials[i].GetColor("_Color");
                }
                else
                {//error log
                    Debug.LogError("material.HasProperty is Not Material Color !");
                }

                //後に描画したいシェーダーに変更
                materials[i].shader = EleMaterial.shader;

                //シェーダーに渡す変数
                materials[i].SetColor("_OutlineColor", OutlineColor);
                materials[i].SetColor("_Albedo", col);
                materials[i].SetFloat("_OutlineSize", OutlineSize);
                materials[i].SetVector("_DirectionalLight", m_cLightObj.gameObject.transform.position);

                if (Texture == true)
                {
                    materials[i].SetTexture("Texture", Texture);
                }
            }
        }
    }

}
