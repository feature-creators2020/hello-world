using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGeneralObjectA : GeneralObject
{

    public override void GeneralInit()
    {
        Debug.Log("testGeneralObjectAInit");
    }

    public override void GeneralUpdate()
    {

    }

    public override void GeneralRelease()
    {
        Debug.Log("testGeneralObjectARelease");
    }

}
