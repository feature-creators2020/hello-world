using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public interface IResultMouseInterfase : IEventSystemHandler
{
    void PlayWin();
    void PlayLose();
}


public class ResultMouseAnimation : MonoBehaviour, IResultMouseInterfase
{
    [System.NonSerialized]
    public string[] AnimationString = { "Mouse_Fun", "Mouse_Sad" };          // アニメーション名
    private int m_nAnimationNo;                                      // 再生中アニメーション番号
    private Animation m_cAnimation;                                  // アニメーション      

    // Start is called before the first frame update
    void Awake()
    {
        m_cAnimation = this.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation(EResultAnimation anim)
    {
        Debug.Log("MousePlayAnimation : " + anim);
        m_nAnimationNo = (int)anim;
        m_cAnimation.Play(AnimationString[m_nAnimationNo]);
    }

    public Animation GetAnimation { get { return m_cAnimation; } }

    public void PlayWin()
    {
        PlayAnimation(EResultAnimation.Win);
    }

    public void PlayLose()
    {
        PlayAnimation(EResultAnimation.Lose);
    }

}
