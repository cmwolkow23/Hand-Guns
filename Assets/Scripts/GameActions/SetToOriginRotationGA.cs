using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SetToOriginRotationGA : GameAction
{
    public Transform[] target;
    [SerializeField]
    private List<Quaternion> origRotations;
    public SetToOriginRotationGA()
    {
        name = "Set to original rotation";
    }
    public override void Action()
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i].rotation = origRotations[i];
        }
        bState = true;
    }
    public override void Initialize()
    {
        origRotations = new List<Quaternion>();
        foreach (Transform t in target)
        {
            origRotations.Add(t.rotation);
        }
    }
}
