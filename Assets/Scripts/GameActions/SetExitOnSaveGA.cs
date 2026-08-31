using System;
using UnityEngine;
[Serializable]
public class SetExitOnSaveGA : GameAction
{
    public bool bSetState;
    public SaveOnExit[] saveOnExits;

    public SetExitOnSaveGA()
    {
        name = "Disable or enables save on exit";
    }
    public override void Action()
    {
        foreach(SaveOnExit item in saveOnExits)
            item.bSkipSave = bSetState;
        bState = true;
    }
}
