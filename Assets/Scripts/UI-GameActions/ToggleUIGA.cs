using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
[Serializable]
public class ToggleUIGA : GameAction
{    
    public string uiName;
    private static bool bShow;
    public UIDocument document;
    private VisualElement ve;
    public ToggleUIGA()
    {
        name = "Toggles the visibility of a UI";
    }
    public override void Action()
    {        
        CoroutineRunner.Instance.RunCoroutine(ToggleView());
    }
    IEnumerator ToggleView()
    {        
        if(document == null)
        {
            Debug.Log("UI Document not assigned");    
            bState = true;
            yield break;        
        }

        if(!document.enabled)
            document.enabled = true;
        yield return new WaitForSeconds(0.1f);     
        VisualElement ve = document.rootVisualElement.Q<VisualElement>(uiName);

        if(bShow)
        {
            Debug.Log("Hide");
            bShow = false;            
            ve.AddToClassList("hideVE"); 
            ve.RegisterCallback<TransitionEndEvent>(HideComplete);  
        }
        else  
        {  
            Debug.Log("Show");
            bShow = true;
            ve.RemoveFromClassList("hideVE");
        }
        bState = true;
    }
    private void HideComplete(TransitionEndEvent evt)
    {
        if (evt.target != evt.currentTarget) return;   // ignore bubbled child transitions
        document.enabled = false;
        if(ve != null)
            ve.UnregisterCallback<TransitionEndEvent>(HideComplete);
    }
}
