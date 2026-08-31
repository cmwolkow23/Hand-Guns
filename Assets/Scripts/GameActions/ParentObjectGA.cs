using System;
using UnityEngine;
[Serializable]
public class ParentObjectGA : GameAction
{
    public bool bParentToTriggeringObject,bParentToPlayer;
    public Transform target;
    public ParentObjectGA()
    {
        name = "Parents object to target";
    }
    public override void Action()
    {        
        if (bParentToTriggeringObject)
        {
            if (target != null && collider != null)
            {
                target.SetParent(collider.transform);
                target.localPosition = Vector3.zero;
            }
        }
        else if(bParentToPlayer)
        {
            if (target != null)
            {
                target.SetParent(GameMaster.PlayerTransform);
                target.localPosition = Vector3.zero;
            }
        }
        else
        {
            if (collider != null && target != null)
            {                
                collider.transform.SetParent(target);
                collider.transform.localPosition = Vector3.zero;
            }
        }
        bState = true;
    }
}
