using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IFanToStarColInterface : IEventSystemHandler
{
    void OnRemoveObject(GameObject obj);
}

public class FanToStarCollision : MonoBehaviour, IFanToStarColInterface
{

    private int m_nFanHitDuration = 0;                                      // 何人のファンとゲームオーバーになっているか
    //private const int m_nGameOverCount = 300;                             // ゲームオーバーになる秒数
    private bool m_bGameOverFlag = false;                                   // ゲームおーばかどうか
    private List<FanListCount> m_lFanListCount = new List<FanListCount>();  // ファン判定用情報を持ったクラスのリスト
    private Star m_cStar;                                                   // スターですよ

    // リスト管理用情報を持ったクラス
    class FanListCount
    {
        public Collider FanCollision;
        public int m_nTimeCount;

        public void Initialize(Collider col)
        {
            FanCollision = col;
            m_nTimeCount = 0;
        }

        public void Plus()
        {
            m_nTimeCount++;
        }

        //public bool CatchPlayer(int gameovertime)
        //{
        //    if (m_nTimeCount >= gameovertime)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }

    void Start()
    {
        m_nFanHitDuration = 0;
        m_bGameOverFlag = false;
        m_cStar = transform.root.gameObject.GetComponent<Star>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!global::PauseManager.IsPause)
        {
            for (var i = 0; i < m_lFanListCount.Count; i++)
            {
                if (m_cStar.StarOrdinaly)
                {
                    m_lFanListCount[i].Plus();
                    //if (m_lFanListCount[i].CatchPlayer(m_cStar.gameOverTime * 60))
                    //{
                    //    m_nFanHitDuration++;
                    //}
                    m_nFanHitDuration += m_lFanListCount[i].m_nTimeCount;
                }
            }
            //if (m_nFanHitDuration >= m_cStar.gameOverTime * 60)
            //{
            //    m_bGameOverFlag = true;
            //    Debug.Log("GameOver");
            //}
            m_nFanHitDuration = 0;
        }
    }

    // ゲームオーバーかどうかゲッター
    public bool GameOver
    {
        get
        {
            return m_bGameOverFlag;
        }
    }

    // ファンに当たったらリストに追加、初期化
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Fan") && col.tag == "FanMain")
        {
            FanListCount fan = new FanListCount();
            fan.Initialize(col);
            m_lFanListCount.Add(fan);
        }
    }

    // 離れたらリストから削除
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Fan") && col.tag == "FanMain")
        {

            for (var i = 0; i < m_lFanListCount.Count; i++)
            {
                if (m_lFanListCount[i].FanCollision != null)
                {
                    if (m_lFanListCount[i].FanCollision.gameObject == col.gameObject)
                    {
                        m_lFanListCount.RemoveAt(i);
                        return;
                    }
                }
                else
                {
                    m_lFanListCount.RemoveAt(i);
                }
            }
        }
    }

    // インターフェイス、離れた、削除されたファンを強制的に削除
    public void OnRemoveObject(GameObject obj)
    {
        for (var i = 0; i < m_lFanListCount.Count; i++)
        {
            if (m_lFanListCount[i].FanCollision.gameObject == obj)
            {
                m_lFanListCount.RemoveAt(i);
            }
        }
    }
    
    // ゲージのためのゲームオーバーの進行度
    public float GaugeProgress()
    {
        float progress = 0.0f;
        float meterprogress = 0.0f;

        if (m_lFanListCount.Count != 0)
        {
            m_lFanListCount.Sort((a, b) => b.m_nTimeCount - a.m_nTimeCount);

            for (var i = 0; i < m_lFanListCount.Count; i++)
            {
                progress += m_lFanListCount[i].m_nTimeCount;
            }
        }

       // meterprogress = 1 - (progress / (float)(m_cStar.gameOverTime * 60));

        if (m_cStar.StarOrdinaly)
        {
            return meterprogress;
        }
        else
        {
            return 1.0f;
        }
    }
}
