using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SwitchColorGA : GameAction
{
    public Color newColor;
    public List<MeshRenderer> mRenderers;
    public SwitchColorGA()
    {
        name = "Switch color of renderer";
    }
    public override void Action()
    {
        foreach(MeshRenderer item in mRenderers)
            item.material.SetColor("_BaseColor",newColor);
        bState = true;
    }
}
