using System.Collections;
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

    public void CheckCursor(GameObject Target)
    {
        if(Target == MouseObj[0])
        {
            Vector2 Range01 = new Vector2(MouseObj[0].transform.position.x - HumanObj[0].transform.position.x, 
                                        MouseObj[0].transform.position.z - HumanObj[0].transform.position.z);
            Vector2 Range02 = new Vector2(MouseObj[0].transform.position.x - HumanObj[1].transform.position.x,
                                        MouseObj[0].transform.position.z - HumanObj[1].transform.position.z);
            if (Range01.magnitude >= 25.0f)
            {
                Mouse01Cursor[0].SetActive(true);
                float rad = Mathf.Atan2(Range01.x, Range01.y);
                Mouse01Cursor[0].transform.position = new Vector3(Mathf.Sin(rad) * 80, Mathf.Cos(rad) * 80);
            }
            else
            {
                Mouse01Cursor[0].SetActive(false);
            }
        }
    }

    private void Update()
    {
        Vector2 Range01 = new Vector2(MouseObj[0].transform.position.x - HumanObj[0].transform.position.x,
                                        MouseObj[0].transform.position.z - HumanObj[0].transform.position.z);
        Vector2 Range02 = new Vector2(MouseObj[0].transform.position.x - HumanObj[1].transform.position.x,
                                    MouseObj[0].transform.position.z - HumanObj[1].transform.position.z);
        if (Range01.magnitude >= 5.0f)
        {
            Mouse01Cursor[0].SetActive(true);
            float rad = Mathf.Atan2(Range01.x, Range01.y);
            Mouse01Cursor[0].transform.localPosition = new Vector3(Mathf.Sin(rad) * 80, Mathf.Cos(rad) * 80);
        }
        else
        {
            Mouse01Cursor[0].SetActive(false);
        }
    }
}
