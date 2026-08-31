using System;
using UnityEngine;
[Serializable]
public class TogglePositionLerpGA : GameAction
{
    public GameActionTrigger gat;
    public TogglePositionLerpGA()
    {
        name = "Used specifically for changing the state of the movement toggle";
    }
    public override void Action()
    {
        foreach(GameAction item in gat.enterActions)
        {
            if(item is PositionLerpGA posLerper)
            {
                posLerper.ToggleDirections();
                break;
            }
        }
        bState = true;
    }
}
