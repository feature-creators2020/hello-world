using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGeneralObjectB : GeneralObject
{
    public override void GeneralInit()
    {
        Debug.Log("testGeneralObjectBInit");
    }

    public override void GeneralUpdate()
    {

    }

    public override void GeneralRelease()
    {
        Debug.Log("testGeneralObjectBRelease");
    }

}
