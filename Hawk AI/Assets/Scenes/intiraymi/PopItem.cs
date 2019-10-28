using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopItem : MonoBehaviour
{
    //出現させるアイテムオブジェクト
    public GameObject[] originItemObj;
    //床のオブジェクト
    public GameObject GroundObj;
    //private float m_fNowItemCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int number = Random.Range(0, originItemObj.Length);
            //作成できる範囲にアイテムを生成する(生成するオブジェクト,生成する場所,クォータニオン)
            Instantiate(originItemObj[number], randomItemDropPos(Vector3.zero, 5f, 5f, GroundObj), Quaternion.identity);
        }
    }

    //障害物にぶつからずアイテムを作成できる場所を返すメソッド

    //pos = 指定する範囲の中心
    //width = 範囲の幅
    //height = 範囲の高さ
    //ground = 地面のオブジェクト
    Vector3 randomItemDropPos(Vector3 pos, float width, float height, GameObject ground)
    {

        while (true)
        {
            Vector3 itemPos = new Vector3(Random.Range(-width, width), 0, Random.Range(-height, height)) + pos; //ランダムに取得したItemの座標
            float topSpace = 10f;
            Vector3 p1 = itemPos + new Vector3(0, topSpace, 0); //上空から
            float itemSize = 0.5f; //このサイズの球体とぶつからなければ何も障害物はないとしてアイテムを置く
            RaycastHit hit; //衝突した場合このオブジェクトに衝突した物体の情報が入る
                            //上空からアイテムを置く候補の場所まで球体を飛ばして衝突したらtrue
            if (Physics.SphereCast(p1, itemSize, Vector3.down, out hit, topSpace))
            {
                //衝突したものが地面ならアイテムを置ける
                //それ以外とぶつかれば座標をランダムに取得する所からやりなおし。
                if (hit.collider.gameObject == ground)
                {
                    return itemPos;
                }
            }
        }
    }
}
