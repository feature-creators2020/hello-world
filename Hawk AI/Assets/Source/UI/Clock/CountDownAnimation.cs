using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownAnimation : SingletonMonoBehaviour<CountDownAnimation>
{
    public void SetCount3(bool TorF)
    {
        GetComponent<Animator>().SetBool("CountDown3", TorF);
    }
    public void SetCount10(bool TorF)
    {
        GetComponent<Animator>().SetBool("CountDown10", TorF);
    }
    public void SetFinish(bool TorF)
    {
        GetComponent<Animator>().SetBool("Finish", TorF);
    }
}
