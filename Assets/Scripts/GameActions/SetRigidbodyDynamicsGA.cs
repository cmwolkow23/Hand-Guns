using System;
using UnityEngine;
[Serializable]
public class SetRigidbodyDynamicsGA : GameAction
{
    public Rigidbody[] rigidbodies;
    public bool bUseGravity;
    public bool bIsKinematic;
    
    public SetRigidbodyDynamicsGA()
    {
        name = "Set Rigidbody Dynamics";
    }
    public override void Action()
    {
        if(rigidbodies.Length == 0)
        {
            Debug.LogError("SetRigidbodyDynamicsGA: rigidbodies is null.");
            bState = true;
            return;
        }
        foreach(Rigidbody item in rigidbodies)
        {
            item.useGravity = bUseGravity;
            item.isKinematic = bIsKinematic;
            if(!bIsKinematic)
                item.WakeUp();
        }
        bState = true;
    }
}
