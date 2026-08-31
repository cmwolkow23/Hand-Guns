using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class ScaleLerpGA : GameAction
{
    public Vector3 targetScale;
    public float duration;
    public Transform[] targetObjects;
    private Vector3 startScale;
    private bool bStartScaleCaptured;
    public ScaleLerpGA()
    {
        name = "Scales the triggerer to the target scale";
    }
    public override void Action()
    {
        if(duration <= 0)
        {
            foreach(Transform item in targetObjects)
                item.localScale = targetScale;
            bState = true;
            return;
        }
        
        CoroutineRunner.Instance.RunCoroutine(ScaleToTarget());
    }
    public override void Initialize()
    {
        //Reset rewinds to startScale, so it has to exist before anything scales the object.
        //Without this, a Reset that runs before Action collapses the target to zero.
        if(targetObjects.Length != 0)
        {
            startScale = targetObjects[0].localScale;
            bStartScaleCaptured = true;
        }
    }
    public override void Reset()
    {
        if(!bStartScaleCaptured) //nothing has scaled yet, so there is nothing to rewind
            return;

        if(targetObjects.Length != 0)
        {
            foreach(Transform item in targetObjects)
                item.localScale = startScale;
        }
        else if(collider != null)
        {
            collider.transform.localScale = startScale;
        }
    }
    IEnumerator ScaleToTarget()
    {
        float rate = 0;
        if(collider == null)
            startScale = Vector3.one;
        else
            startScale = collider.transform.localScale;
        bStartScaleCaptured = true;
        while(rate < 1)
        {
            rate += Time.deltaTime / duration;
            rate = Mathf.Clamp01(rate);
            if(targetObjects.Length != 0)
            {
                foreach(Transform item in targetObjects)
                    item.localScale = Vector3.Lerp(startScale,targetScale,rate);
            }
            else
                collider.transform.localScale = Vector3.Lerp(startScale,targetScale,rate);
            yield return new WaitForEndOfFrame();
        }
        bState = true;
    }
}
