using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SwitchLightColorGA : GameAction
{
    public Color newColor;
    public List<Light> lights;
    public SwitchLightColorGA()
    {
        name = "Switch color of Light Emission";
    }
    public override void Action()
    {
        foreach (Light item in lights)
            item.color = newColor;
        bState = true;
    }
}
