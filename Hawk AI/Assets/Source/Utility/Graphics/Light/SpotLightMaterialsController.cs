using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightMaterialsController : MonoBehaviour
{
    [SerializeField] private Light LightObj;
    [SerializeField] private Material EleMaterial;    //元々のシェーダーマテリアル
    [SerializeField] private Texture2D Texture;        //テクスチャ有の場合設定
    [SerializeField] private bool isNumMaterials;

    void Start()
    {
        if (LightObj == null)
        {
            Debug.LogError("LightObj is null.");
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
                new Vector4(LightObj.gameObject.transform.position.x,
                LightObj.gameObject.transform.position.y,
                LightObj.gameObject.transform.position.z,
                0f);

            material.SetColor("_Color", LightObj.color);
            material.SetColor("_Albedo", col);
            material.SetVector("_LightPos", vector4);

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
                materials[i].SetColor("_LightColor", LightObj.color);
                materials[i].SetColor("_Albedo", col);
                materials[i].SetVector("_DirectionalLight", LightObj.gameObject.transform.position);

                if (Texture == true)
                {
                    materials[i].SetTexture("Texture", Texture);
                }
            }
        }
    }

}
