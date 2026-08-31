using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class SwitchMaterialGA : GameAction
{
    public Renderer[] renderers;
    public Material materialToSwitchTo, originalMaterial;

    public SwitchMaterialGA()
    {
        name = "Switch Material";
    }
    public override void Action()
    {
        foreach (var renderer in renderers)
            renderer.material = materialToSwitchTo;
        bState = true;
    }

    public override void Initialize()
    {
        Reset();
    }

    public override void Reset()
    {
        foreach (Renderer item in renderers)
            item.material = originalMaterial;
    }
}
