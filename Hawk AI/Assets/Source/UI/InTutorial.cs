using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InTutorial : SingletonMonoBehaviour<InTutorial>
{
    [SerializeField]
    private Text TUTORIAL;
    [Range(0,0.1f)][SerializeField]
    private float ChangeHue;
    private float Hue;

    // Start is called before the first frame update
    void Start()
    {
        Hue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TUTORIAL.color = Color.HSVToRGB(Hue, 1, 1);
        Hue += ChangeHue;
        if(Hue > 1)
        {
            Hue = 0;
        }
    }
}
