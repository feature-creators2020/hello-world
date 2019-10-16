using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    public GameObject score_object = null; // Textオブジェクト
    public GameObject result_object1 = null;
    public GameObject result_object2 = null;
    private int score_num1, score_num2;
    private Text score_text;
    private Text result_text1;
    private Text result_text2;

    // 初期化
    void Start()
    {
        score_num1 = score_num2 = 0;
        // オブジェクトからTextコンポーネントを取得
        score_text = score_object.GetComponent<Text>();
        result_text1 = result_object1.GetComponent<Text>();
        result_text2 = result_object2.GetComponent<Text>();
    }

    void Update()
    {
        if(score_num1+score_num2 == 4)
        {
            if (score_num1 < score_num2)
            {
                // テキストの表示を入れ替える
                result_text1.text = "LOSE";
                result_text2.text = "WIN";
            }
            else if (score_num1 > score_num2) 
            {
                // テキストの表示を入れ替える
                result_text1.text = "WIN";
                result_text2.text = "LOSE";
            }
            else
            {
                // テキストの表示を入れ替える
                result_text1.text = result_text2.text = "DROW";
            }
        }
    }

    //  ネズミ側死亡時カウント
    public void DeadMouse()
    {
        score_num1++;
        // テキストの表示を入れ替える
        score_text.text = score_num1 + "::" + score_num2;
    }

    //  ネズミ側ゴール時カウント
    public void GoalMouse()
    {
        score_num2++;
        // テキストの表示を入れ替える
        score_text.text = score_num1 + "::" + score_num2;
    }
}