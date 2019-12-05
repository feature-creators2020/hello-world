using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IMouseTrap : IEventSystemHandler
{
    void SetMapPosition(Vector2Int[] _mappos);

}


public class Mousetrap : GeneralObject, IMouseTrap
{
    private const int m_cTrapWidth = 2;
    private Vector2Int[] m_cTrapPos = new Vector2Int[m_cTrapWidth];

    GameObject MouseObject; // 上に乗ったネズミの情報

    float m_fLifeTime = 5f;

    MeshRenderer m_Mesh;    // 自身のメッシュ

    public override void GeneralInit()
    {
        // 生成時には呼ばれない
    }
    void OnEnable()
    {
        m_fLifeTime = 5f;
        // 実際にあるメッシュのコンポーネントを取得
        m_Mesh = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();
        m_Mesh.enabled = false;
    }


    public override void GeneralUpdate()
    {
        // 実際にあるメッシュのコンポーネントを取得
        m_Mesh = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();

        // ネズミの情報を取得しているとき
        if(!ReferenceEquals(MouseObject, null))
        {
            // ネズミ情報取得
            var mouse = MouseObject.GetComponent<MouseStateManager>();
            // 速度低下倍率が0のとき
            if(mouse.m_fSlowDownRate <= 0f)
            {
                MeshFlashing();
                m_fLifeTime -= Time.deltaTime;
                if(m_fLifeTime <= 0f)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public override void GeneralRelease()
    {
        // Map から消す
        //for (int i = 0; i < m_cTrapWidth; i++)
        //{
        //    if(MapManager.Instance != null)
        //    MapManager.Instance.MapData[m_cTrapPos[i].y][m_cTrapPos[i].x]
        //        = (int)ObjectNo.NONE;
                
        //}

        //var itemmanager = ManagerObjectManager.Instance.GetGameObject("ItemManager");
        //var mousetrapmanager = itemmanager.GetComponent<ItemManager>().GetGameObject("MousetrapManager");
        //mousetrapmanager.GetComponent<MousetrapManager>().Destroy(this.gameObject);
    }

    //public override void OnDestroy()
    //{
    //    base.OnDestroy();
    //}

    public void SetMapPosition(Vector2Int[] _mappos)
    {
        for (int i = 0; i < m_cTrapWidth; i++)
        {
            m_cTrapPos[i] = _mappos[i];
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Mouse")
        {
            m_Mesh.enabled = true;
            MouseObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Mouse")
        {
            m_Mesh.enabled = false;
            MouseObject = null;
        }
    }

    void MeshFlashing()
    {
        if (m_fLifeTime <= 3f)
        {
            if ((int)m_fLifeTime > 0)
            {
                var flash = m_fLifeTime * 10f;
                if ((int)flash % (int)m_fLifeTime == 0)
                {
                    // 点滅始める
                    m_Mesh.enabled = !m_Mesh.enabled;
                }
            }
            else
            {
                m_Mesh.enabled = false;
            }
        }
    }
}
