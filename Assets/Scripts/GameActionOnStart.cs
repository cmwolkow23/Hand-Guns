using System.Collections;
using UnityEngine;

public class GameActionOnStart : MonoBehaviour
{
    [SerializeReference,SubclassSelector]
    public GameAction[] modifierActions,undoModifierActions;

    private void Awake()
    {
        GameAction.InitializeAll(modifierActions);
        GameAction.InitializeAll(undoModifierActions);
        GameMaster.DelResetPlayerDefaults += DestroyOnRestart;
    }
    private void Start()
    {
        StartCoroutine(nameof(TriggerActions));
    }
    IEnumerator TriggerActions()
    {
        foreach(GameAction action in modifierActions)
        {
            yield return new WaitForSeconds(action.GetDelay);
            action.Action();
        }
    }
    private void OnDestroy()
    {
        GameMaster.DelResetPlayerDefaults -= DestroyOnRestart;
    }
    private void DestroyOnRestart()
    {
        foreach (GameAction action in undoModifierActions)
            action.Action();
        Destroy(this.gameObject);
    }
}
