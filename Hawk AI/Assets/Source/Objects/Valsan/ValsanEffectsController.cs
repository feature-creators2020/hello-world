using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IValsanEffect : IEventSystemHandler
{
    void Play();

    void Stop();
}


public class ValsanEffectsController : MonoBehaviour, IValsanEffect
{
    [SerializeField]
    private List<GameObject> ParticleList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
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
