using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseGetTrap : GeneralObject
{

    GameObject MouseObject; // 上に乗ったネズミの情報

    float m_fLifeTime = 5f;
    float m_fRotTime = 2f;
    float m_fMaxRotTime = 2f;

    MeshRenderer m_Mesh;    // 自身のメッシュ
    [SerializeField]
    GameObject m_gMesh;     // メッシュ全体
    [SerializeField]
    GameObject m_gTrap;     // 回転させる部分

    Quaternion StartRot;    // 初期姿勢
    Quaternion EndRot;      // 最終姿勢

    public override void GeneralInit()
    {
        // 生成時には呼ばれない
    }

    void OnEnable()
    {
        m_fLifeTime = 1f;
        // 実際にあるメッシュのコンポーネントを取得
        m_Mesh = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();
        //m_Mesh.enabled = false;
        //m_gMesh = this.gameObject.transform.GetChild(0).gameObject;
        m_gMesh.SetActive(false);
        //m_gTrap = this.gameObject.transform.GetChild(1).gameObject;
        m_gTrap.SetActive(false);
        m_fRotTime = m_fMaxRotTime;
        StartRot = m_gTrap.transform.rotation;
        EndRot = new Quaternion(0, 0, 20f, StartRot.w);
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

            if (m_gTrap.activeSelf)
            {
                ExecuteEvents.Execute<IMouseInterface>(
                target: MouseObject,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.SetCollapse());

                float speed = -(m_fMaxRotTime - m_fRotTime) * EndRot.z;
                if(speed >= EndRot.z)
                {
                    speed = EndRot.z;
                    ExecuteEvents.Execute<IMouseInterface>(
                    target: MouseObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.SetDefaultSize());
                }
                Quaternion rot = Quaternion.AngleAxis(speed, transform.right);

                // 元の回転値と合成して上書き
                m_gTrap.transform.localRotation = StartRot * rot;
                //m_gTrap.transform.rotation = Quaternion.Slerp(StartRot, EndRot, m_fMaxRotTime - m_fRotTime);
                m_fRotTime -= Time.deltaTime;
                if(m_fRotTime < 0f)
                {
                    m_fRotTime = 0f;
                }
            }
            else
            {
                // 初期姿勢に戻す
                m_gTrap.transform.rotation = StartRot;
                m_fRotTime = m_fMaxRotTime;
            }
        }
    }

    public override void GeneralRelease()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        //m_Mesh.enabled = true;
        m_gMesh.SetActive(true);
        if (other.tag == "Mouse")
        {
            m_gTrap.SetActive(true);
            MouseObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //m_Mesh.enabled = false;
        m_gMesh.SetActive(false);
        if (other.tag == "Mouse")
        {
            m_gTrap.SetActive(false);
            MouseObject = null;
        }
    }

}
