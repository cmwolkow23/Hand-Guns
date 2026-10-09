using UnityEngine;
using System;
[Serializable]

public class KillGA : GameAction
{
    public KillGA()
    {
        name = "Kill Player";
    }
    public override void Action()
    {
        // Use UnityEngine.Object.FindFirstObjectByType<T>() instead of the obsolete FindObjectOfType<T>()
        Respawner respawner = UnityEngine.Object.FindFirstObjectByType<Respawner>();
        if (respawner != null)
        {
            respawner.Death();
            Debug.Log("Player has been killed.");
        }
        else
        {
            Debug.LogWarning("No Respawner found in the scene.");
        }
        bState = true; // Mark the action as completed
    }
}