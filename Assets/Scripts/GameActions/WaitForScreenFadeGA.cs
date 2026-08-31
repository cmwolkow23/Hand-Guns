using System;
using UnityEngine;
[Serializable]
public class WaitForScreenFadeGA : GameAction
{
    public bool bWaitForFadeIn;
    public WaitForScreenFadeGA()
    {
        name = "Wait For Screen Fade";       
    }
    public override void Action()
    {
        if(bWaitForFadeIn)
            FadeScreen.ScreenFadeInComplete += FadeInComplete;
        else
            FadeScreen.ScreenFadeOutComplete += FadeOutComplete;
    }
    private void FadeInComplete()
    {     
        FadeScreen.ScreenFadeInComplete -= FadeInComplete;
        bState = true;
    }
    private void FadeOutComplete()
    {
        FadeScreen.ScreenFadeOutComplete -= FadeOutComplete;
        bState = true;
    }
    public override void Reset()
    {        
        if (bWaitForFadeIn)
            FadeScreen.ScreenFadeInComplete -= FadeInComplete;
        else
            FadeScreen.ScreenFadeOutComplete -= FadeOutComplete;
    }
}
