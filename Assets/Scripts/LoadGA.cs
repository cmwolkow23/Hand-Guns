using System;
using UnityEngine;
[Serializable]
public class LoadGA : GameAction
{
    public string saveName;
    public LoadGA()
    {
        name = "Loads game save";
    }
    public override void Action()
    {
        if(string.IsNullOrEmpty(saveName))
        {
            GameMaster.LoadGame();
        }
        else
            GameMaster.LoadGame(saveName);
        
        bState = true;
    }
}
