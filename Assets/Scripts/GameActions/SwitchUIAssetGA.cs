using System;
using UnityEngine;
using UnityEngine.UIElements;
[Serializable]
public class SwitchUIAssetGA : GameAction
{
    public VisualTreeAsset uiAsset;
    public UIDocument document;

    public SwitchUIAssetGA()
    {
        name = "Switch UI Asset for UI Document";
    }
    public override void Action()
    {
        document.visualTreeAsset = uiAsset;
        bState = true;
    }
}
