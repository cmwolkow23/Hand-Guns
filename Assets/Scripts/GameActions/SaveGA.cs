using System;
using UnityEngine;
[Serializable]
public class SaveGA : GameAction
{
    public SaveGA()
    {
        name = "Saves game";
    }
    public override void Action()
    {
        GameMaster.SaveGame();
        bState = true;
    }
}
