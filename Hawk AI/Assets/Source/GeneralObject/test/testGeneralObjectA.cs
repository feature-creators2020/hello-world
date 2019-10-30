using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGeneralObjectA : GeneralObject
{

    public override void GeneralInit()
    {

    }

    public override void GeneralUpdate()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A");
        }

    }

    public override void GeneralRelease()
    {
    }

}
