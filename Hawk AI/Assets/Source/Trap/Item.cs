using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    bool isLimitScale = false;
    Vector3 keepScale;

    // Start is called before the first frame update
    void Start()
    {
        keepScale = this.transform.localScale;
        this.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        isLimitScale = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0f, 1f, 0f);
        if (!isLimitScale)
        {
            ScaleUp();
        }
    }

    void ScaleUp()
    {
        this.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        if (this.transform.localScale.x >= keepScale.x)
        {
            isLimitScale = true;
        }
    }
}
