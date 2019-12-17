﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCursor : SingletonMonoBehaviour<RadarCursor>
{
    [SerializeField]
    private List<GameObject> Mouse01Cursor;
    [SerializeField]
    private List<GameObject> Mouse02Cursor;
    [SerializeField]
    private List<GameObject> MouseObj;
    [SerializeField]
    private List<GameObject> HumanObj;
    [SerializeField]
    private List<GameObject> CameraObj;
    [SerializeField]
    private float CursorRadius;
    [SerializeField]
    private float DetectionRange;

    public void CheckInCursor(GameObject Target)
    {
        if (Target == MouseObj[0])
        {
            for(int i = 0; i < 2; i++)
            {
                CameraObj[i].transform.Find("RadarCamera").GetComponent<Camera>().cullingMask |= 1 << 22;
                Mouse01Cursor[i].SetActive(false);
            }
        }
        else if (Target == MouseObj[1])
        {
            for (int i = 0; i < 2; i++)
            {
                CameraObj[i].transform.Find("RadarCamera").GetComponent<Camera>().cullingMask |= 1 << 23;
                Mouse02Cursor[i].SetActive(false);
            }
        }
    }
    
    //ドローンがターゲティング中実行
    public void CheckCursor(GameObject Target)
    {
        if(Target == MouseObj[0])
        {
            Vector2 Range01 = new Vector2(MouseObj[0].transform.position.x - HumanObj[0].transform.position.x,
                                        MouseObj[0].transform.position.z - HumanObj[0].transform.position.z);
            Vector2 Range02 = new Vector2(MouseObj[0].transform.position.x - HumanObj[1].transform.position.x,
                                        MouseObj[0].transform.position.z - HumanObj[1].transform.position.z);
            if (Range01.magnitude >= 10.0f)
            {
                Mouse01Cursor[0].SetActive(true);
                Vector2 Camera_forward = new Vector2(CameraObj[0].transform.forward.x, CameraObj[0].transform.forward.z);
                var rad = Mathf.Atan2(Range01.x, Range01.y) - Mathf.Atan2(Camera_forward.x, Camera_forward.y);
                Mouse01Cursor[0].transform.localPosition = new Vector3(Mathf.Sin(rad) * 45, Mathf.Cos(rad) * 45);
                Mouse01Cursor[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -rad * Mathf.Rad2Deg);
            }
            else
            {
                Mouse01Cursor[0].SetActive(false);
                CameraObj[0].transform.Find("RadarCamera").GetComponent<Camera>().cullingMask |= 1 << 22;
            }
            if (Range02.magnitude >= 10.0f)
            {
                Mouse01Cursor[1].SetActive(true);
                Vector2 Camera_forward = new Vector2(CameraObj[1].transform.forward.x, CameraObj[1].transform.forward.z);
                var rad = Mathf.Atan2(Range02.x, Range02.y) - Mathf.Atan2(Camera_forward.x, Camera_forward.y);
                Mouse01Cursor[1].transform.localPosition = new Vector3(Mathf.Sin(rad) * 45, Mathf.Cos(rad) * 45);
                Mouse01Cursor[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -rad * Mathf.Rad2Deg);
            }
            else
            {
                Mouse01Cursor[1].SetActive(false);
                CameraObj[1].transform.Find("RadarCamera").GetComponent<Camera>().cullingMask |= 1 << 22;
            }
        }
        else if(Target == MouseObj[1])
        {
            Vector2 Range01 = new Vector2(MouseObj[1].transform.position.x - HumanObj[0].transform.position.x,
                                        MouseObj[1].transform.position.z - HumanObj[0].transform.position.z);
            Vector2 Range02 = new Vector2(MouseObj[1].transform.position.x - HumanObj[1].transform.position.x,
                                        MouseObj[1].transform.position.z - HumanObj[1].transform.position.z);
            if (Range01.magnitude >= 10.0f)
            {
                Mouse02Cursor[0].SetActive(true);
                Vector2 Camera_forward = new Vector2(CameraObj[0].transform.forward.x, CameraObj[0].transform.forward.z);
                var rad = Mathf.Atan2(Range01.x, Range01.y) - Mathf.Atan2(Camera_forward.x, Camera_forward.y);
                Mouse02Cursor[0].transform.localPosition = new Vector3(Mathf.Sin(rad) * 45, Mathf.Cos(rad) * 45);
                Mouse02Cursor[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -rad * Mathf.Rad2Deg);
            }
            else
            {
                Mouse02Cursor[0].SetActive(false);
                CameraObj[0].transform.Find("RadarCamera").GetComponent<Camera>().cullingMask |= 1 << 23;
            }
            if (Range02.magnitude >= 10.0f)
            {
                Mouse02Cursor[1].SetActive(true);
                Vector2 Camera_forward = new Vector2(CameraObj[1].transform.forward.x, CameraObj[1].transform.forward.z);
                var rad = Mathf.Atan2(Range02.x, Range02.y) - Mathf.Atan2(Camera_forward.x, Camera_forward.y);
                Mouse02Cursor[1].transform.localPosition = new Vector3(Mathf.Sin(rad) * 45, Mathf.Cos(rad) * 45);
                Mouse02Cursor[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -rad * Mathf.Rad2Deg);
            }
            else
            {
                Mouse02Cursor[1].SetActive(false);
                CameraObj[1].transform.Find("RadarCamera").GetComponent<Camera>().cullingMask |= 1 << 23;
            }
        }
    }

    //ターゲットが外れたら見えなくする
    public void CheckOutCursor()
    {
        for(int i = 0; i < 2; i++)
        {
            CameraObj[i].transform.Find("RadarCamera").GetComponent<Camera>().cullingMask &= ~(1 << 22);
            CameraObj[i].transform.Find("RadarCamera").GetComponent<Camera>().cullingMask &= ~(1 << 23);
            Mouse01Cursor[i].SetActive(false);
            Mouse02Cursor[i].SetActive(false);
        }
    }

    //private void Update()
    //{
    //    Vector2 Range01 = new Vector2(MouseObj[0].transform.position.x - HumanObj[0].transform.position.x,
    //                                    MouseObj[0].transform.position.z - HumanObj[0].transform.position.z);
    //    Vector2 Range02 = new Vector2(MouseObj[0].transform.position.x - HumanObj[1].transform.position.x,
    //                                MouseObj[0].transform.position.z - HumanObj[1].transform.position.z);
    //    if (Range01.magnitude >= 5.0f)
    //    {
    //        Mouse01Cursor[0].SetActive(true);
    //        Vector2 Camera_forward = new Vector2(CameraObj[0].transform.forward.x, CameraObj[0].transform.forward.z);
    //        var rad = Mathf.Atan2(Range01.x, Range01.y) - Mathf.Atan2(Camera_forward.x, Camera_forward.y);
    //        Mouse01Cursor[0].transform.localPosition = new Vector3(Mathf.Sin(rad) * 80, Mathf.Cos(rad) * 80);
    //        Mouse01Cursor[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0,0, -rad*Mathf.Rad2Deg);
    //    }
    //    else
    //    {
    //        Mouse01Cursor[0].SetActive(false);
    //    }
    //    if (Range02.magnitude >= 5.0f)
    //    {
    //        Mouse01Cursor[1].SetActive(true);
    //        Vector2 Camera_forward = new Vector2(CameraObj[1].transform.forward.x, CameraObj[1].transform.forward.z);
    //        var rad = Mathf.Atan2(Range02.x, Range02.y) - Mathf.Atan2(Camera_forward.x, Camera_forward.y);
    //        Mouse01Cursor[1].transform.localPosition = new Vector3(Mathf.Sin(rad) * 80, Mathf.Cos(rad) * 80);
    //        Mouse01Cursor[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -rad * Mathf.Rad2Deg);
    //    }
    //    else
    //    {
    //        Mouse01Cursor[1].SetActive(false);
    //    }
    //    Vector2 Range03 = new Vector2(MouseObj[1].transform.position.x - HumanObj[0].transform.position.x,
    //                                    MouseObj[1].transform.position.z - HumanObj[0].transform.position.z);
    //    Vector2 Range04 = new Vector2(MouseObj[1].transform.position.x - HumanObj[1].transform.position.x,
    //                                MouseObj[1].transform.position.z - HumanObj[1].transform.position.z);
    //    if (Range03.magnitude >= 5.0f)
    //    {
    //        Mouse02Cursor[0].SetActive(true);
    //        Vector2 Camera_forward = new Vector2(CameraObj[0].transform.forward.x, CameraObj[0].transform.forward.z);
    //        var rad = Mathf.Atan2(Range03.x, Range03.y) - Mathf.Atan2(Camera_forward.x, Camera_forward.y);
    //        Mouse02Cursor[0].transform.localPosition = new Vector3(Mathf.Sin(rad) * 80, Mathf.Cos(rad) * 80);
    //        Mouse02Cursor[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -rad * Mathf.Rad2Deg);
    //    }
    //    else
    //    {
    //        Mouse02Cursor[0].SetActive(false);
    //    }
    //    if (Range04.magnitude >= 5.0f)
    //    {
    //        Mouse02Cursor[1].SetActive(true);
    //        Vector2 Camera_forward = new Vector2(CameraObj[1].transform.forward.x, CameraObj[1].transform.forward.z);
    //        var rad = Mathf.Atan2(Range04.x, Range04.y) - Mathf.Atan2(Camera_forward.x, Camera_forward.y);
    //        Mouse02Cursor[1].transform.localPosition = new Vector3(Mathf.Sin(rad) * 80, Mathf.Cos(rad) * 80);
    //        Mouse02Cursor[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -rad * Mathf.Rad2Deg);
    //    }
    //    else
    //    {
    //        Mouse02Cursor[1].SetActive(false);
    //    }
    //}
}
