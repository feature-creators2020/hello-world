using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum EImagerPlayerModelChild
{
    eLoser,
    eWinner,

}


public class ImagerPlayerModel : MonoBehaviour,IResultManagerInterfase
{
    [SerializeField]
    private Texture HumanTexture;

    [SerializeField]
    private Texture MouseTexture;

    [SerializeField]
    private Texture EatCheeseTexture;

    [SerializeField]
    private Texture KillMouseTexture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HawkAIWin()
    {
        var winner =
        this.gameObject.transform.GetChild((int)EImagerPlayerModelChild.eWinner).gameObject;

        var loser =
        this.gameObject.transform.GetChild((int)EImagerPlayerModelChild.eLoser).gameObject;

        winner.GetComponent<RawImage>().texture = HumanTexture;
        loser.GetComponent<RawImage>().texture = MouseTexture;
    }

    public void MouseWin()
    {
        var winner =
          this.gameObject.transform.GetChild((int)EImagerPlayerModelChild.eWinner).gameObject;

        var loser =
           this.gameObject.transform.GetChild((int)EImagerPlayerModelChild.eLoser).gameObject;

        winner.GetComponent<RawImage>().texture = MouseTexture;
        loser.GetComponent<RawImage>().texture = HumanTexture;

    }

    public void SetIsOk(int id, bool isVal)
    {

    }
}
