using System;
using UnityEngine;

[Serializable]
public class ObjectiveCheckGA : GameAction
{
    [Tooltip("Drag the specific DoorCheck GameObject here in the inspector")]
    public DoorCheck targetDoor;

    [Tooltip("The index of the trigger on the target door")]
    public int triggerIndex;

    [Tooltip("Check this box to send a TRUE signal. Uncheck it to send a FALSE signal.")]
    public bool signalState = true;

    public ObjectiveCheckGA()
    {
        name = "Updates a specific door trigger signal";
    }

    public override void Action()
    {
        if (targetDoor != null)
        {
            // Sends your chosen true/false state to the target door instance
            targetDoor.UpdateTriggerSignal(triggerIndex, signalState);
        }
        else
        {
            Debug.LogWarning($"[ObjectiveCheckGA] '{name}' is missing a Target Door assignment!");
        }

        bState = true;
    }

    public void Deactivate()
    {
        if (targetDoor != null)
        {
            // Inverts the desired state when deactivated, or custom logic if needed
            targetDoor.UpdateTriggerSignal(triggerIndex, !signalState);
        }
    }
}
