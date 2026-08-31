using System;
using UnityEngine;
[Serializable]
public class UnlockAchievementGA : GameAction
{
    public string achievementID;

    public UnlockAchievementGA()
    {
        name = "Unlocks Achievement";
    }
    public override void Action()
    {
        GameMaster.RecordAchievement(achievementID);
        bState = true;
    }
}
