using System;
using UnityEngine;
[Serializable]
public class SetStartPositionGA : GameAction
{
    public Transform targetTransform;
    private Vector3 startPosition;
    public SetStartPositionGA()
    {
         name = "Set Start Position";
    }
    public override void Action()
    {
        if(targetTransform == null)
        {
            Debug.LogError("SetStartPositionGA: targetTransform is null.");
            bState = true;
            return;
        }
        targetTransform.position = startPosition;
        bState = true;
    }
    public override void Initialize()
    {
        if(targetTransform == null)
        {
            Debug.LogError("SetStartPositionGA: targetTransform is null.");
            return;
        }
        startPosition = targetTransform.position;
    }
}
