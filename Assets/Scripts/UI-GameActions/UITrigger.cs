using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UITrigger : MonoBehaviour
{
    public UIDocument uIDocument;
    public UIInfo[] uIInfos;

    private void Start()
    {
        VisualElement rootElement = uIDocument.rootVisualElement;

        foreach(UIInfo item in uIInfos)
        {
            GameAction.InitializeAll(item.actions);
            if(item.uIType == UIType.Button)
            {
                Debug.Log("Registered");
                rootElement.Q<Button>(item.name).clicked += () => ExecuteActions(item.actions);
            }
        }
    }
    private void OnDisable()
    {
        foreach(UIInfo item in uIInfos)
            GameAction.ResetAll(item.actions);
    }
    private void ExecuteActions(GameAction[] actions)
    {
        Debug.Log("Clicked");
        foreach(GameAction item in actions)
            item.Action();
    }
}
[Serializable]
public struct UIInfo
{
    public string name;
    public UIType uIType;    
    [SerializeReference,SubclassSelector]
    public GameAction[] actions;
}
public enum UIType{Button,Slider,Toggle}