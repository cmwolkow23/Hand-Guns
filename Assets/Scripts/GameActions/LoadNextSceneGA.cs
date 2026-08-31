using System;
using UnityEngine;
[Serializable]
public class LoadNextSceneGA : GameAction
{
    public LoadNextSceneGA()
    {
        name = "Loads Next Scene";
    }
    public override void Action()
    {
        GameMaster.LoadNextScene();
        bState = true;
    }
}
