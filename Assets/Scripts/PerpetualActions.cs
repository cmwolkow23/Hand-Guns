using UnityEngine;
using System.Collections;
public class PerpetualActions : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    private GameAction[] gameActions;
    private bool bActive;
    private Coroutine perpetualLoop;
    private void OnEnable()
    {
        GameAction.InitializeAll(gameActions);
        bActive = true;
        ResetLoop();        
    }
    private void OnDisable()
    {
        bActive = false;
        GameAction.ResetAll(gameActions); // rewind each action when the object is disabled
    }
    public void StopLoop()
    {
        bActive = false;
    }
    private void ResetLoop()
    {
        StopAllCoroutines();
        perpetualLoop = StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (bActive)
        {
            foreach (GameAction action in gameActions)
            {
                yield return new WaitForSeconds(action.delay); // Wait for the specified delay before executing the action
                action.Action(); // Execute the action
                while(!action.GetState) // Wait until the action's bState is true before proceeding to the next action
                    yield return null; // Wait for the next frame before checking again    
                action.Reset(); // Reset the action's state for the next loop iteration
            }
        }
    }
}
