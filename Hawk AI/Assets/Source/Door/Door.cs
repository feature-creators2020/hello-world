using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDoorInterface : IEventSystemHandler
{
    void Open();
    void Close();

}

/// <summary>
/// Doorの開閉スクリプト
/// </summary>

public class Door : MonoBehaviour, IDoorInterface
{
    [SerializeField]
    protected float OpenSpeed;                //扉の開閉速度
    [SerializeField]
    protected float MaxOpenRadian;             //扉の開閉最大角度
    protected Vector3 StartAngle;
    public bool isOpening {get;set; }        //扉の開フラグ
    public bool isClosing { get; set; }       //扉の閉フラグ
    public bool isPopEffect { get; set; }     //エフェクト出現フラグ


    // Start is called before the first frame update
    public virtual void Start()
    {
        isOpening = false;
        isClosing = false;
        isPopEffect = false;
        StartAngle = this.gameObject.transform.rotation.eulerAngles;
    }

    public virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Close();
        }
    }


    //開ける
    public virtual void Open()
    {
        if (isOpening == false)
        {
            isOpening = true;
            StartCoroutine(OpenDoorCoroutine(StartAngle, new Vector3(0f, MaxOpenRadian, 0f)));
        }
    }

    //閉める
    public virtual void Close()
    {
        if (isClosing == false)
        {
            isClosing = true;
            StartCoroutine(CloseDoorCoroutine(new Vector3(0f, MaxOpenRadian, 0f), StartAngle));
        }
    }



    public virtual IEnumerator OpenDoorCoroutine(Vector3 _StartAngle, Vector3 _EndAngle)
    {
        isClosing = false;
        float lerpVal = 0f;

        while (lerpVal <= 1f)
        {//開ける時間補間
            this.gameObject.transform.rotation = Quaternion.Euler(
                Vector3.Lerp(_StartAngle, _EndAngle, lerpVal));
            lerpVal += Time.deltaTime / OpenSpeed;
            yield return null;
        }

        if(_StartAngle != _EndAngle)
        {//強硬手段
            this.gameObject.transform.rotation = Quaternion.Euler(_EndAngle);
        }

        //isOpening = false;
    }


    public virtual IEnumerator CloseDoorCoroutine(Vector3 _StartAngle, Vector3 _EndAngle)
    {
        isOpening = false;
        float lerpVal = 0f;

        while (lerpVal <= 1f)
        {//閉まる時間補間
            this.gameObject.transform.rotation = Quaternion.Euler(
                Vector3.Lerp(_StartAngle, _EndAngle, lerpVal));
            lerpVal += Time.deltaTime / OpenSpeed;
            yield return null;
        }

        if (_StartAngle != _EndAngle)
        {//強硬手段
            this.gameObject.transform.rotation = Quaternion.Euler(_EndAngle);
        }


        //isClosing = false;
    }

}
