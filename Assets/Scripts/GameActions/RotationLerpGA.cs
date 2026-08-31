using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class RotationLerpGA : GameAction
{
    public bool bWaitForCompletion;
    public float duration;
    public Transform target,targetRotation;
    public Vector3 desiredRot;

    public RotationLerpGA()
    {
        name = "Rotates the target to the desired rotation over the duration";
    }
    public override void Action()
    {
        if(duration == 0 && target != null) //immediately rotate if the duration hasn't been set.
        {
            target.rotation = Quaternion.Euler(desiredRot);
            bState = true;
            return;
        }

        if (target == null)
        {
            bState = true;
            return;
        }
        else
            CoroutineRunner.Instance.RunCoroutine(RotateOverTime()); 

        if(bWaitForCompletion) return;
            bState = true;
        
    }
    IEnumerator RotateOverTime()
    {
        Quaternion startRot = target.rotation;
        Quaternion endRot = Quaternion.Euler(desiredRot);

        if(targetRotation)
            endRot = targetRotation.rotation;
            
        float rate = 0;
        if(startRot.eulerAngles == desiredRot)
        {
            bState= true;
            yield break;
        }
       
        while (rate < 1)
        {
            rate += Time.deltaTime / duration;
            rate = Mathf.Clamp01(rate);
            target.rotation = Quaternion.Slerp(startRot, endRot, rate);            
            yield return null;
        }
        bState = true;
    }
}
