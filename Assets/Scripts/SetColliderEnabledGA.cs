using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SetColliderEnabledGA : GameAction
{
    public bool bEnable;
    public List<Collider> colliders = new List<Collider>();
    public SetColliderEnabledGA()
    {
        if(bEnable)
            name = "Enable Colliders";
        else
            name = "Disable Colliders";
    }
    public override void Action()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = bEnable;
        }
        bState = true;
    }
}
