using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGeneralObjectC : GeneralObject
{
    public override void GeneralInit()
    {
        Debug.Log("testGeneralObjectCInit");
    }

    public override void GeneralUpdate()
    {

    }

    public override void GeneralRelease()
    {
        Debug.Log("testGeneralObjectCRelease");
    }

}
