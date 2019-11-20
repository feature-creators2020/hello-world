using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アウトラインをInspector上で編集できるようにするクラス
/// </summary>

public class OutlineController : MonoBehaviour
{
    [SerializeField]
    float OutlineSize;		//アウトラインサイズ
    [SerializeField]
    Material EleMaterial;	//セットするシェーダーマテリアル

    // Start is called before the first frame update
    void Start()
    {
        ////親オブジェクトにアタッチして一括管理するようにする
        //var SkinnedMeshRend = GetComponentsInChildren<SkinnedMeshRenderer>();

        ////総スキンメッシュ数
        //Debug.LogFormat("materials : {0}", SkinnedMeshRend.Length);

        // 子Transformのコンポーネントをすべて取得し、ループで回す
        var SkinnedMeshRend = GameObject.FindGameObjectWithTag("fbxWithOutlineParts").GetComponentsInChildren<SkinnedMeshRenderer>();
        //マテリアルをアウトライン付きのマテリアルに変更
        foreach (var child in SkinnedMeshRend)
        {
            Material material = child.material;
            Color col = Color.cyan;

            if (material != null)
            {//オブジェクトにマテリアルがセットされている場合

                //マテリアルの元々設定されている色を取得
                if (material.HasProperty("_Color") == true)
                {
                    col = material.GetColor("_Color");
                }
                else
                {//error log
                    Debug.LogError("material.HasProperty is Not Material Color !");
                }
                //マテリアルのシェーダー変更
                material.shader = EleMaterial.shader;

                //シェーダーに渡す変数
                material.SetColor("_Albedo", col);
                material.SetFloat("_OutlineSize", OutlineSize);
            }

        }

        //マテリアルをアウトライン付きのマテリアルに変更
        //for (int i = 0; i < SkinnedMeshRend.Length; i++)
        //{
        //    Material material = SkinnedMeshRend[i].material;
        //    Color col = Color.cyan;

        //    if (material != null)
        //    {//オブジェクトにマテリアルがセットされている場合

        //        //マテリアルの元々設定されている色を取得
        //        if (material.HasProperty("_Color") == true)
        //        {
        //            col = material.GetColor("_Color");
        //        }
        //        else
        //        {//error log
        //            Debug.LogError("material.HasProperty is Not Material Color !");
        //        }

        //        //FIXED : 特定のタグのオブジェクトを除外するようにする
        //        //マテリアルのシェーダー変更
        //        material.shader = EleMaterial.shader;

        //        //シェーダーに渡す変数
        //        material.SetColor("_Albedo", col);
        //        material.SetFloat("_OutlineSize", OutlineSize);
        //    }
        //}
    }

}
