using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMapHundller : MonoBehaviour
{
    private GameObject m_cPlayerObj;
    private Vector2Int m_cPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        m_cPlayerObj = this.gameObject.transform.parent.GetChild(0).gameObject;
        m_cPlayerPos = new Vector2Int(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = this.gameObject.transform.parent.GetChild(0).transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "MapColliderBox")
        {
            m_cPlayerPos.x = (int)other.gameObject.transform.position.x;
            m_cPlayerPos.y = (int)other.gameObject.transform.position.z;
            MapManager.Instance.MapData[m_cPlayerPos.y][m_cPlayerPos.x] = (int)ObjectNo.PLAYER;
            Debug.Log("csv[" + m_cPlayerPos.y + "][" + m_cPlayerPos.x + "] = " + MapManager.Instance.MapData[m_cPlayerPos.y][m_cPlayerPos.x]);


            ExecuteEvents.Execute<IPlayerInterfase>(
            target: m_cPlayerObj,
            eventData: null,
            functor: (recieveTarget, y) => recieveTarget.SetMapPos(m_cPlayerPos));

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "MapColliderBox")
        {
            m_cPlayerPos.x = (int)other.gameObject.transform.position.x;
            m_cPlayerPos.y = (int)other.gameObject.transform.position.z;
            MapManager.Instance.MapData[m_cPlayerPos.y][m_cPlayerPos.x] = (int)ObjectNo.NONE;
            //Debug.Log("csv[" + m_cPlayerPos.y + "][" + m_cPlayerPos.x + "] = " + MapManager.Instance.MapData[m_cPlayerPos.y][m_cPlayerPos.x]);

            //ExecuteEvents.Execute<IPlayerInterfase>(
            //target: m_cPlayerObj,
            //eventData: null,
            //functor: (recieveTarget, y) => recieveTarget.MapPos = new Vector2Int(horizon, vertical));

        }
    }
}