using System;
using UnityEngine;
[Serializable]
public class SetRendererEnabledGA : GameAction
{
    public bool bEnable;
    public Renderer[] renderer;
    public SetRendererEnabledGA()
    {
        name = "Set Renderer";
    }
    public override void Action()
    {
        foreach (Renderer renderer in renderer)
            renderer.enabled = bEnable;
        bState = true;
    }
    public override void Setup()
    {
        if(bEnable)
            name = "Enable Renderer";
        else
            name = "Disable Renderer";
    }
}
