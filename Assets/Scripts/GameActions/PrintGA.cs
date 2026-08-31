using System;
using UnityEngine;
[Serializable]
public class PrintGA : GameAction
{
    public bool bPrintCollider;
    public string message;
    public PrintGA()
    {
        name = "Print";
    }

    public override void Action()
    {
        Debug.Log(message);
        if(bPrintCollider)
            Debug.Log(collider.name + " " + collider.tag);
        bState = true;
    }
}
