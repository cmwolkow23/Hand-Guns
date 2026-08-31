using UnityEngine;
using System;
[Serializable]
public class TeleportGA : GameAction
{
    public Transform target;
    public TeleportGA()
    {
        name = "Teleports the triggerer to the target position";
    }
    public override void Action()
    {
        if(target)
        {
            collider.transform.position = target.position;
            bState = true;
        }
        else
        {
            Debug.LogWarning("Target not set");
            bState = true;
        }
    }
}
