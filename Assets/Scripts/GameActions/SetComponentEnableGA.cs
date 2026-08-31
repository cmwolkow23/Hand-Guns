using System;
using UnityEngine;
[Serializable]
public class SetComponentEnableGA : GameAction
{
    public MonoBehaviour componentToToggle;
    public bool bEnableComponent;
    public SetComponentEnableGA()
    {
        name = "Enables or disables provided component";
    }
    public override void Action()
    {
        if (componentToToggle != null)
            componentToToggle.enabled = bEnableComponent;
        bState = true;
    }
}
