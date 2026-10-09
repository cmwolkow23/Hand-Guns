using UnityEngine;
using System;

[Serializable]
public class CheckpointGA : GameAction
{
    public GameObject gameObject;

    public CheckpointGA()
    {
        name = "Set Checkpoint";
    }

    public Transform transform
    {
        get { return gameObject != null ? gameObject.transform : null; }
    }

    public override void Action()
    {
        Transform checkpointTransform = transform;
        if (checkpointTransform == null)
        {
            Debug.LogWarning("Checkpoint GameObject is not assigned.");
            bState = true;
            return;
        }

        Respawner respawner = UnityEngine.Object.FindFirstObjectByType<Respawner>();
        if (respawner != null)
        {
            respawner.respawnPoint = checkpointTransform;
            Debug.Log($"Checkpoint set at position: {checkpointTransform.position}");
        }
        else
        {
            Debug.LogWarning("No Respawner found in the scene.");
        }

        bState = true;
    }
}
