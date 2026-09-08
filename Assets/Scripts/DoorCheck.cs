using UnityEngine;
using System.Collections.Generic;

public class DoorCheck : MonoBehaviour
{
    public int requiredSignalCount = 3;

    [SerializeReference, SubclassSelector]
    public GameAction actionToPerform;

    [SerializeReference, SubclassSelector]
    public GameAction actionToDeactivate;

    public List<GameActionTrigger> triggers = new List<GameActionTrigger>();

    private bool wasActivated = false;

    // Track state per door instance rather than globally across the whole game
    private Dictionary<int, bool> triggerStates = new Dictionary<int, bool>();

    // This is now an instance method requiring a target door reference to call
    public void UpdateTriggerSignal(int triggerIndex, bool state)
    {
        triggerStates[triggerIndex] = state;
    }

    private void Update()
    {
        int activeCount = 0;

        for (int i = 0; i < triggers.Count; i++)
        {
            if (triggerStates.TryGetValue(i, out bool isActive) && isActive)
                activeCount++;
        }

        Debug.Log($"[DoorCheck] {gameObject.name} Active signals: {activeCount}/{requiredSignalCount}");

        if (activeCount >= requiredSignalCount)
        {
            if (!wasActivated)
            {
                actionToPerform?.Action();
                wasActivated = true;
            }
        }
        else
        {
            if (wasActivated)
            {
                actionToDeactivate?.Action();
                wasActivated = false;
            }
        }
    }
}
