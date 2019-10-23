using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Camera))]
public class DepthTexture : MonoBehaviour
{

    [SerializeField]
    private Shader _shader;
    [SerializeField]
    private float _outlineThreshold = 0.01f;
    [SerializeField]
    private Color _outlineColor = Color.white;
    [SerializeField]
    private float _outlineThick = 1.0f;
    [SerializeField]
    private GameObject _target;

    private List<GameObject> m_cGameObjectsList = new List<GameObject>();

    private Material _material;

    private void Awake()
    {
        //Initialize();
    }

    private void Update()
    {
#if UNITY_EDITOR
        SetMaterialProperties();
#endif
    }

    public void Initialize()
    {
        var camera = GetComponent<Camera>();
        camera.depthTextureMode |= DepthTextureMode.Depth;

        // CAUT : Check To Inspector!
        if (camera.allowMSAA || camera.allowHDR)
        {
            return;
        }

        _material = new Material(_shader);

        SetMaterialProperties();

        var commandBuffer = new CommandBuffer();
        int tempTextureIdentifier = Shader.PropertyToID("_PostEffectTempTexture");
        commandBuffer.GetTemporaryRT(tempTextureIdentifier, -1, -1);
        commandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, tempTextureIdentifier);
        commandBuffer.Blit(tempTextureIdentifier, BuiltinRenderTextureType.CurrentActive, _material);
        commandBuffer.ReleaseTemporaryRT(tempTextureIdentifier);
        camera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);

    }

    
    private void OldSetMaterialProperties()
    {

        Material[] mtr = _target.GetComponent<SkinnedMeshRenderer>().materials;


        if (_material != null)
        {
            _material.SetFloat("_OutlineThreshold", _outlineThreshold);
            _material.SetColor("_OutlineColor", _outlineColor);
            _material.SetFloat("_OutlineThick", _outlineThick);
            _material.SetTexture("_MaterialTexture", mtr[0].GetTexture("_MainTex"));
        }

    }

    #region AllSetMaterial
    private void SetMaterialProperties()
    {

        GameObject ManagerObj =
            ManagerObjectManager.Instance.GetGameObject((int)EManagerObject.eOBJECT);

        ExecuteEvents.Execute<IGeneralInterface>(
            target: ManagerObj,
            eventData: null,
            functor: (recieveTarget, y) => m_cGameObjectsList = recieveTarget.GetGameObjectsList());


        foreach (var obj in m_cGameObjectsList)
        {
            foreach (var mtr in obj.GetComponent<SkinnedMeshRenderer>().materials)
            {
                if (_material != null)
                {
                    _material.SetFloat("_OutlineThreshold", _outlineThreshold);
                    _material.SetColor("_OutlineColor", _outlineColor);
                    _material.SetFloat("_OutlineThick", _outlineThick);
                    foreach (var uv in obj.GetComponent<SkinnedMeshRenderer>().sharedMesh.uv)
                    {
                        _material.SetColor("_ObjectUV", new Color(uv.x, uv.y,0,0));
                    }
                    _material.SetTexture("_MaterialTexture", mtr.GetTexture("_MainTex"));
                }
            }
        }
    }
    #endregion
}