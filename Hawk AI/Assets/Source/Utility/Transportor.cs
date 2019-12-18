using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//public interface ITransportor : IEventSystemHandler
//{
//    void 
//}




public class Transportor : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> LerpObjList = new List<GameObject>();

    [SerializeField]
    private List<Image> LerpImageList = new List<Image>();

    [SerializeField]
    private List<Vector3> ObjStartPos = new List<Vector3>();

    [SerializeField]
    private List<Vector3> ObjEndPos = new List<Vector3>();

    [SerializeField]
    private List<Vector3> ImageStartPos = new List<Vector3>();

    [SerializeField]
    private List<Vector3> ImageEndPos = new List<Vector3>();

    [SerializeField]
    private float lerpTime;

    private bool m_bStart = true;
    private bool m_bDirectionLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bStart)
        {
            m_bStart = false;

            if (m_bDirectionLeft)
            {// Right To Left
                //Obj
                for (int i = 0; i < LerpObjList.Count; i++)
                {
                    StartCoroutine(LerpObj(LerpObjList[i], ObjStartPos[i], ObjEndPos[i]));
                }

                //Image
                for (int i = 0; i < LerpImageList.Count; i++)
                {
                    StartCoroutine(LerpImage(LerpImageList[i], ImageStartPos[i], ImageEndPos[i]));
                }

                m_bDirectionLeft = false;
            }
            else
            {// Left To Right

                //obj
                for (int i = 0, j = LerpObjList.Count -1;
                        i < LerpObjList.Count;
                        i++,j--)
                {
                    StartCoroutine(LerpObj(LerpObjList[j], ObjEndPos[i], ObjStartPos[i]));
                }

                //Image
                for (int i = 0 ,j = LerpImageList.Count - 1;
                     i < LerpImageList.Count;
                    i++,j--)
                {
                    StartCoroutine(LerpImage(LerpImageList[j], ImageEndPos[i], ImageStartPos[i]));
                }

                m_bDirectionLeft = true;
            }
        }
    }

    IEnumerator LerpImage(Image _Img, Vector3 StartPos, Vector3 EndPos)
    {
        float lerpVal = 0f;

        while (lerpVal <= 1f)
        {
            lerpVal += (Time.deltaTime / lerpTime);
            _Img.rectTransform.localPosition = Vector3.Lerp(
                StartPos,
                EndPos,
                lerpVal
                );
            yield return null;
        }


        if (StartPos != EndPos)
        {
            _Img.rectTransform.localPosition = EndPos;
        }
    }


    IEnumerator LerpObj(GameObject _obj,Vector3 StartPos, Vector3 EndPos)
    {
        int LerpImageCount = LerpImageList.Count;
        float lerpVal = 0f;

        while(lerpVal <= 1f)
        {
            lerpVal += (Time.deltaTime / lerpTime);
            _obj.transform.position = Vector3.Lerp(
                StartPos,
                EndPos,
                lerpVal
                );
            yield return null;
        }

        if(StartPos != EndPos)
        {
            _obj.transform.position = EndPos;
        }

        _obj.transform.rotation = Quaternion.AngleAxis(_obj.transform.eulerAngles.y + 180, Vector3.up);

        if (LerpObjList[LerpImageCount] == _obj)
        {
            m_bStart = true;
        }
    }
}
