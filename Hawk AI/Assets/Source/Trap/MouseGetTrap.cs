using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseGetTrap : GeneralObject
{

    GameObject MouseObject; // 上に乗ったネズミの情報

    float m_fLifeTime = 5f;

    MeshRenderer m_Mesh;    // 自身のメッシュ

    public override void GeneralInit()
    {
        // 生成時には呼ばれない
    }

    void OnEnable()
    {
        m_fLifeTime = 1f;
        // 実際にあるメッシュのコンポーネントを取得
        m_Mesh = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();
        m_Mesh.enabled = false;
    }
    public override void GeneralUpdate()
    {

        // ネズミの情報を取得しているとき
        if (!ReferenceEquals(MouseObject, null))
        {
            // ネズミ情報取得
            var mouse = MouseObject.GetComponent<MouseStateManager>();
            // 速度低下倍率が0のとき
            if (mouse.m_fSlowDownRate <= 0f)
            {
                m_fLifeTime -= Time.deltaTime;
                if (m_fLifeTime <= 0f)
                {
                    ExecuteEvents.Execute<IMouseInterface>(
                        target: MouseObject,
                        eventData: null,
                        functor: (recieveTarget, y) => recieveTarget.Catched());

                    Destroy(this.gameObject);
                }
            }
        }
    }

    public override void GeneralRelease()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mouse")
        {
            m_Mesh.enabled = true;
            MouseObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mouse")
        {
            m_Mesh.enabled = false;
            MouseObject = null;
        }
    }

}
