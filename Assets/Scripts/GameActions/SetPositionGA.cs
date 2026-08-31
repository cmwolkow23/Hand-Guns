using System;
using UnityEngine;
[Serializable]
public class SetPositionGA : GameAction
{
    public bool bUseTransform,bMoveTriggerer;
    public Vector3 targetPosition;
    public Transform objectToMove;
    public Transform targetTransformPosition;
    public SetPositionGA()
    {
        name = "Sets object's position";
    }
    public override void Action()
    {
        if(bUseTransform)
        {
            if(bMoveTriggerer)
                collider.transform.position = targetTransformPosition.position;
            else if(objectToMove)
                objectToMove.position = targetTransformPosition.position;
        }
        else
        {
            if(bMoveTriggerer)
                collider.transform.position = targetPosition;
            else if(objectToMove)
                objectToMove.position = targetPosition;
        }
        bState = true;
    }
}
