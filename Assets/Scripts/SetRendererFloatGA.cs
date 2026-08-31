using System;
using UnityEngine;
[Serializable]
public class SetRendererFloatGA : GameAction
{
    public Renderer localRenderer;
    public string propertyName;
    public float value;

    public SetRendererFloatGA()
    {
        name = "Set Renderer Float";
    }
    public override void Action()
    {
        if(localRenderer == null)
        {
            Debug.LogError("SetRendererFloatGA: localRenderer is null.");
            bState = true;
            return;
        }
        localRenderer.material.SetFloat(propertyName, value);
        bState = true;
    }
}
