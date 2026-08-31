using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class WaitForVelocityBelowGA : GameAction
{
    public float timeToWait;
    public float velocityThreshold;
    public Rigidbody localRigidbody;

    [SerializeReference, SubclassSelector]
    public GameAction[] onSettled;
    [SerializeReference, SubclassSelector]
    public GameAction[] onTimeout;

    public WaitForVelocityBelowGA()
    {
        name = "Wait For Velocity Below";
    }

    //the branch arrays are a nested sequence, so they need initializing too
    public override void Initialize()
    {
        GameAction.InitializeAll(onSettled);
        GameAction.InitializeAll(onTimeout);
    }
    public override void Reset()
    {
        GameAction.ResetAll(onSettled);
        GameAction.ResetAll(onTimeout);
    }
    public override void Action()
    {       
        if (localRigidbody == null)
        {
            Debug.LogError("WaitForVelocityBelowGA: localRigidbody is null.");
            bState = true;
            return;
        }
        CoroutineRunner.Instance.RunCoroutine(WaitLoop());
    }

    IEnumerator WaitLoop()
    {  
        float elapsedTime = 0f;

        yield return new WaitForFixedUpdate(); // Initial wait to ensure physics updates

        while (elapsedTime < timeToWait && localRigidbody.linearVelocity.magnitude > velocityThreshold)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bool settled = localRigidbody.linearVelocity.magnitude <= velocityThreshold;
        GameAction[] branch = settled ? onSettled : onTimeout;
        if (branch != null)
        {
            foreach (GameAction action in branch)
            {
                    if (action != null) 
                        action.Action();
            }
        }
        bState = true;
    }
}