using System;
using UnityEngine;
[Serializable]
public class TriggererCheckGA : GameAction
{
    public bool bPlayer;
    public GameActionTrigger gameActionTrigger;
    public TriggererCheckGA()
    {
        name = "Checks if the triggerer is the player or not";
    }
    public override void Action()
    {
        if(collider.CompareTag("Player") && bPlayer)
        {
            bState = true;
        }
        else if(!bPlayer && !collider.CompareTag("Player"))
        {
            bState = true;
        }
        else
        {
            gameActionTrigger.ResetTrigger();
        }
    }
}
