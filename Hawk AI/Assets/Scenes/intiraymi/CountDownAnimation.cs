using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownAnimation : SingletonMonoBehaviour<CountDownAnimation>
{
    public void SetCount3(float Num)
    {
        GetComponent<Animator>().SetFloat("CountDown3", Num);
    }
    public void SetCount10(float Num)
    {
        GetComponent<Animator>().SetFloat("CountDown10", Num);
    }
}
