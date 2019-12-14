using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IValsanEffect : IEventSystemHandler
{
    void Play();

    void Stop();
}

public enum ERoomArea
{
    eRoom1,
    eRoom2,
    eRoom3,
    eRoom4,
}



public class ValsanEffectsController : MonoBehaviour, IValsanEffect
{
    [SerializeField]
    private List<GameObject> ParticleList = new List<GameObject>();

    [SerializeField]
    private List<RawImage> SmokeEffectsCanvas = new List<RawImage>();

    [SerializeField]
    private List<Image> SmokeColorCanvas = new List<Image>();

    private List<Color> SmokeEffects = new List<Color>();
    private List<Color> SmokeColor = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SmokeEffectsCanvas.Count; i++)
        {
            SmokeEffects.Add(SmokeEffectsCanvas[i].color);

            SmokeEffectsCanvas[i].color = new Color(
                SmokeEffectsCanvas[i].color.r,
                SmokeEffectsCanvas[i].color.g,
                SmokeEffectsCanvas[i].color.b,
                0);
        }

        for (int i = 0; i < SmokeColorCanvas.Count; i++)
        {
            SmokeColor.Add(SmokeColorCanvas[i].color);

            SmokeColorCanvas[i].color = new Color(
                SmokeColorCanvas[i].color.r,
                SmokeColorCanvas[i].color.g,
                SmokeColorCanvas[i].color.b,
                0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Play();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Stop();
        }


        MaskingPlayerCamera();
    }

    private void MaskingPlayerCamera()
    {
        //Hack : Playerがどこのエリアにいるか

        for (int i = 0; i < SmokeEffectsCanvas.Count; i++)
        {
            // TODO : 自分と同じエリアであれば
            //if ()
            //{
            SmokeEffectsCanvas[i].color = SmokeEffects[i];
            //}
        }

        for (int i = 0; i < SmokeColorCanvas.Count; i++)
        {
            // TODO : 自分と同じエリアであれば
            //if ()
            //{
                SmokeColorCanvas[i].color = SmokeColor[i];
            //}
        }

    }



    public void Play()
    {
        foreach(var val in ParticleList)
        {
            val.GetComponent<ParticleSystem>().Play();
        }
    }

    public void Stop()
    {
        foreach (var val in ParticleList)
        {
            val.GetComponent<ParticleSystem>().Stop();
        }
    }

}
