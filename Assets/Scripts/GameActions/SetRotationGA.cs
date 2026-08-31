using System;
using UnityEngine;
[Serializable]
public class SetRotationGA : GameAction
{
    public Vector3 rotation;
    public Transform objectToRotate;
    public SetRotationGA()
    {
        name = "Rotates object";
    }
    public override void Action()
    {
        objectToRotate.rotation = Quaternion.Euler(rotation);
        bState = true;
    }
}
