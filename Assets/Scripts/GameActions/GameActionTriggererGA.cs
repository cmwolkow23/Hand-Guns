using System;
using UnityEngine;
[Serializable]
public class GameActionTriggererGA : GameAction
{
    public bool bEnter;
    public GameActionTrigger gat;
    public GameActionTriggererGA()
    {
        name = "Activates a GameActionTrigger";
    }
    public override void Action()
    {
        if(gat)
        {
            if(bEnter)
                gat.TriggerEnterActions();
            else
                gat.TriggerExitActions();
        }
        else
            Debug.LogError("Game Action Trigger not set");
        bState = true;
    }
}
