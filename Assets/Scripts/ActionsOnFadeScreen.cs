using UnityEngine;
using System.Collections;
public class ActionsOnFadeScreen : MonoBehaviour
{
    public bool bFadeIn;
    [SerializeReference,SubclassSelector]
    public GameAction[] actions;
    private void OnEnable()
    {
        GameAction.InitializeAll(actions);
        if(bFadeIn)
            FadeScreen.ScreenFadeInComplete += TriggerActions;
        else
            FadeScreen.ScreenFadeOutComplete += TriggerActions;
    }
    private void OnDisable()
    {
        if(bFadeIn)
            FadeScreen.ScreenFadeInComplete -= TriggerActions;
        else
            FadeScreen.ScreenFadeOutComplete -= TriggerActions;

        GameAction.ResetAll(actions);
    }
    private void TriggerActions()
    {
        foreach(GameAction item in actions)
            item.Action();
    }
    private void OnValidate()
    {
        foreach (GameAction item in actions)
        {
            if(item != null)
                item.Setup();
        }
    }
}
