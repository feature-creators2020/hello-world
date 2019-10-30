using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISetMaterial : IEventSystemHandler
{
    void SetMaterial(GameObject _Object);

    void RevertMaterial(GameObject _Object);
}

public class MaterialController : GeneralManager, ISetMaterial
{
    Dictionary<GameObject, List<Shader>> m_cDicShaders
    = new Dictionary<GameObject, List<Shader>>();

    Dictionary<GameObject, List<Color>> m_cDicColors
    = new Dictionary<GameObject, List<Color>>();

    [SerializeField]
    Material m_cMaterial;   //セットするシェーダーマテリアル

    public void SetMaterial(GameObject _Object)
    {
        List<Shader> bufShaders = new List<Shader>();

        // 子のコンポーネントをすべて取得し、ループで回す
        var SkinnedMeshRend = _Object.GetComponentsInChildren<SkinnedMeshRenderer>();

        //マテリアルに変更
        foreach (var child in SkinnedMeshRend)
        {
            foreach (var material in child.materials)
            {
                Color col = Color.cyan;

                if (material != null)
                {//オブジェクトにマテリアルがセットされている場合

                    //マテリアルの元々設定されている色を取得
                    if (material.HasProperty("_Color") == true)
                    {
                        col = material.GetColor("_Color");
                        //シェーダーに渡す変数
                        material.SetColor("_Albedo", col);
                    }
                    else
                    {//error log
                     // Debug.LogError("material.HasProperty is Not Material Color !");
                    }
                    //マテリアルのシェーダー変更
                    bufShaders.Add(material.shader);
                    material.shader = m_cMaterial.shader;

                }
            }
        }


        m_cDicShaders[_Object] = bufShaders;

    }

    public void RevertMaterial(GameObject _Object)
    {

        // 子のコンポーネントをすべて取得し、ループで回す
        var SkinnedMeshRend = _Object.GetComponentsInChildren<SkinnedMeshRenderer>();

        List<Shader> Shaders = m_cDicShaders[_Object];

        int i = 0;
        foreach (var child in SkinnedMeshRend)
        {
            foreach (var material in child.materials)
            {
                Color col = Color.cyan;

                if (material != null)
                {//オブジェクトにマテリアルがセットされている場合

                    material.shader = Shaders[i];
                    i++;
                }
            }
        }
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    SetMaterial(this.gameObject);
            
            
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    RevertMaterial(this.gameObject);
        //}

    }

    //public override void GeneralInit()
    //{
    //    base.GeneralInit();
    //}

    //// Update is called once per frame
    //public override void GeneralUpdate()
    //{
    //    base.GeneralUpdate();
    //    DebugUpdate();
    //}

    //public override void GeneralRelease()
    //{
    //    base.GeneralRelease();
    //}

    //public override void DebugUpdate()
    //{
    //    base.DebugUpdate();
    //}

    //public override GameObject GetGameObject(int _ID)
    //{
    //    return base.GetGameObject(_ID);
    //}

    //public override List<GameObject> GetGameObjectsList()
    //{
    //    return base.GetGameObjectsList();
    //}

}
