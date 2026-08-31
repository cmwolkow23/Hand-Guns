using System;
using UnityEngine;
using UnityEngine.SceneManagement;
[Serializable]
public class SaveSceneGA : GameAction
{
    public SaveSceneGA()
    {
        name = "Saves current scene";
    }
    public override void Action()
    {
        GameMaster.RecordLevel(SceneManager.GetActiveScene().name);
        //Debug.Log("Scene saved: " + SceneManager.GetActiveScene().name);
        bState = true;
    }
}
