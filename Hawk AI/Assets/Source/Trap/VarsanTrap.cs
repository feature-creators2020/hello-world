using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IVarsanTrap : IEventSystemHandler
{
    void SetRoom(int _room);

}

public class VarsanTrap : GeneralObject, IVarsanTrap
{

    float m_fLifeTime;                  // トラップの生存時間
    float m_fMaxLifeTime;               // トラップの最大生存時間
    bool m_isActive = false;            // カウントを始める
    int m_nRoomNum;                     // 存在しているルームの番号

    // Start is called before the first frame update
    void OnEnable()
    {
        m_fLifeTime = 0f;
        m_fMaxLifeTime = 20f;
        m_isActive = true;
    }

    // Update is called once per frame
    public override void GeneralUpdate()
    {
        if (m_isActive)
        {
            if(m_fLifeTime >= m_fMaxLifeTime)
            {
                // 生存時間終了
                return;
            }
            m_fLifeTime += Time.deltaTime;  // 生存時間カウント
        }
    }

    public void SetRoom(int _room)
    {

    }
}
