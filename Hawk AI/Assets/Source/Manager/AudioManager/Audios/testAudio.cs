using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class testAudio : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            //ExecuteEvents.Execute<IAudioInterface>(
            //       target: GameObject.Find("BGMAudio"),
            //       eventData: null,
            //       functor: (recieveTarget, y) => recieveTarget.Play((int)BGMAudioType.AREA_1));

            //ExecuteEvents.Execute<IAudioInterface>(
            //     target: GameObject.Find("StarAudio"),
            //     eventData: null,
            //     functor: (recieveTarget, y) => recieveTarget.Play((int)StarAudioType.Running));

            //ExecuteEvents.Execute<IAudioInterface>(
            //     target: GameObject.Find("FanAudio"),
            //     eventData: null,
            //     functor: (recieveTarget, y) => recieveTarget.Play((int)FanAudioType.Running));


        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            //ExecuteEvents.Execute<IAudioInterface>(
            //     target: GameObject.Find("FanAudio"),
            //     eventData: null,
            //     functor: (recieveTarget, y) => recieveTarget.Play((int)FanAudioType.Landing));

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            //ExecuteEvents.Execute<IAudioInterface>(
            //     target: GameObject.Find("FanAudio"),
            //     eventData: null,
            //     functor: (recieveTarget, y) => recieveTarget.Play((int)FanAudioType.Hullabaloo));

        }
    }
}
