using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SetToOriginPositionGA : GameAction
{
    public Transform[] target;
    [SerializeField]
    private List<Vector3> origPosition;
    public SetToOriginPositionGA()
    {
        name = "Set to original position";
    }
    public override void Action()
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i].position = origPosition[i];
        }
        bState = true;
    }
    public override void Initialize()
    {
        origPosition = new List<Vector3>();
        foreach (Transform t in target)
        {
            origPosition.Add(t.position);
        }
    }
}
