using System;
using UnityEngine;
[Serializable]
public class FadeScreenGA : GameAction
{
    public bool bFadeIn;
    public FadeScreenGA()
    {
        name = "Fades Screen";    
    }
    public override void Action()
    {
        if(bFadeIn)
            GameMaster.FadeIn();
        else   
            GameMaster.FadeOut();
        bState = true;
    }
}
