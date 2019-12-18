using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IVarsanItemSpawnInterfase : IEventSystemHandler
{
    void SetDefault();

    void AddTime();
    void AddTime(float _time);

    void DebugSpawn();
}

public class VarsanItemSpawn : GeneralObject, IVarsanItemSpawnInterfase
{
    [SerializeField]
    GameObject m_gSpawnObject;              // スポーンさせるアイテムオブジェクト

    public float m_fCastTime;               // スポーンさせる時間経過
    private float m_fMaxCastTime = 60f;           // スポーンさせる間隔

    bool m_isSpawned = false;               // スポーン済み

    private GameObject m_gItemObject;     // 生成されているオブジェクト

    public override void GeneralInit()
    {
        base.GeneralInit();
    }

    public override void GeneralUpdate()
    {
        if (!m_isSpawned)
        {
            if (m_fCastTime >= m_fMaxCastTime)
            {
                // アイテムを生成する
                SpawnItem();
            }
            else
            {
                m_fCastTime += Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DebugSpawn();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddTime();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            DebugReset();
        }
    }

    void SpawnItem()
    {
        m_gItemObject = Instantiate(m_gSpawnObject, this.transform.position, this.transform.rotation);
        m_isSpawned = true;
    }

    public void SetDefault()
    {
        //Debug.Log("SetDefault");
        m_isSpawned = false;
        m_fCastTime = 0f;
    }

    // 確実に5秒増やす
    public void AddTime()
    {
        m_fCastTime += 5f;
    }
    // 増やす時間を設定する
    public void AddTime(float _time)
    {
        m_fCastTime += _time;
    }

    public void DebugSpawn()
    {
        if (!m_isSpawned)
        {
            m_fCastTime = m_fMaxCastTime;
            SpawnItem();
        }
    }

    public void DebugReset()
    {
        SetDefault();
    }
}
