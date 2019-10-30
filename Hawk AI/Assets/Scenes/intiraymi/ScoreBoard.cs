using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/****シングルトン化****/
public class ScoreBoard : SingletonMonoBehaviour<ScoreBoard>
{
    [SerializeField]
    private List<GameObject> CheeseIcon;
    private int RemainingCheese = 4;

    public void GetCheese()
    {
        RemainingCheese -= 1;
        if (RemainingCheese < 0) RemainingCheese = 4;
        //現状はアイコンの色を変えている、実際はテクスチャを変える
        CheeseIcon[RemainingCheese].GetComponent<Image>().color = new Color(0, 0, 0);
    }
}
