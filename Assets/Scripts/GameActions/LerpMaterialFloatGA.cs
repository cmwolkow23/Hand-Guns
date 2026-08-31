using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class LerpMaterialFloatGA : GameAction
{
    public float startValue,endValue;
    public float duration;
    public string propertyName;
    public Renderer[] renderers;

    public LerpMaterialFloatGA()
    {
        name = "Changes the float overtime";
    }
    public override void Action()
    {
        CoroutineRunner.Instance.StartCoroutine(ChangeOverTime());
    }
    IEnumerator ChangeOverTime()
    {
        float rate = 0;
        
        while(rate < 1)
        {
            rate += Time.deltaTime / duration;
            rate = Mathf.Clamp01(rate);

            foreach(Renderer item in renderers)
            {
                item.material.SetFloat(propertyName,Mathf.Lerp(startValue,endValue,rate));
            }            
            yield return new WaitForEndOfFrame();
        }
        bState = true;
    }
}
