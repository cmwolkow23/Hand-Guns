using UnityEngine;

public class OnDisableActions : MonoBehaviour
{
    [SerializeReference,SubclassSelector]
    public GameAction[] actions;

    private void OnEnable()
    {
        GameAction.InitializeAll(actions);
    }
    private void OnDisable()
    {
        foreach(GameAction item in actions)
            item.Action();
    }
}
