using System;
using UnityEngine;
[Serializable]
public class NameCheckGA : GameAction
{
    public string targetName;
    public GameActionTrigger gat;
    public NameCheckGA()
    {
        name ="Checks the name of the collided object";
    }
    public override void Action()
    {
        if(collider.name != targetName)
            gat.ExitSequence();
        bState = true;
    }
}