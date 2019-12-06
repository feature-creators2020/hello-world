using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAreaCollision : MonoBehaviour
{

    private List<GameObject> HumanObject_List = new List<GameObject>();

    private bool m_bCanAction;           // ドアのアクションができるか
    private GameObject m_gTargetHuman;   // 対象の人

    // Start is called before the first frame update
    void Start()
    {
        m_bCanAction = false;
    }

    // Update is called once per frame
    void Update()
    {
        // フラグ初期化
        m_bCanAction = false;
        m_gTargetHuman = null;

        foreach (var val in HumanObject_List)
        {
            if (!m_bCanAction)
            {
                // 人が入っているか探す
                if (val.tag == "Human")
                {
                    m_bCanAction = true;
                    m_gTargetHuman = val;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("DoorEnter : " + other.gameObject.name);
        // 人の時処理をする
        if (other.tag == "Human")
        {
            //foreach (var val in HumanObject_List)
            //{
            //    if (val == other.gameObject)
            //    {
            //        return;
            //    }
            //}
            // 人の情報を入れる
            HumanObject_List.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("DoorExit : " + other.gameObject.name);
        // 人の時処理をする
        if (other.tag == "Human")
        {
            // 人の情報を消す
            //HumanObject_List.Remove(other.gameObject);

            //foreach (var val in HumanObject_List)
            //{
            //    if (val == other.gameObject)
            //    {
            //        // 人の情報を消す
                    HumanObject_List.Remove(other.gameObject);
            //        return;
            //    }
            //}
        }
    }

    public bool CanAction()
    {
        return m_bCanAction;
    }

    public GameObject GetTargetHuman()
    {
        return m_gTargetHuman;
    }
}
