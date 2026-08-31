using System;
using UnityEngine;
[Serializable]
public class TerminateSequenceGA : GameAction
{
    public GameActionTrigger gat;
    public TerminateSequenceGA()
    {
        name = "Terminates an active GameActionTrigger sequence";
    }
    public override void Action()
    {
        if(gat)
            gat.TerminateSequence();
        else
            Debug.LogError("Game Action Trigger not set");
        bState = true;
    }
}
