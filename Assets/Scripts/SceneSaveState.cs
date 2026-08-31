//used to block the scene from changes
using System.Collections.Generic;
using UnityEngine;

public class SceneSaveState : MonoBehaviour
{
    [SerializeReference,SubclassSelector]
    private List<GameAction> stateActions;

    private void Awake()
    {
        GameAction.InitializeAll(stateActions);
    }
    private void OnDisable()
    {
        GameAction.ResetAll(stateActions);
    }
    private void OnTriggerEnter(Collider other)
    {
        foreach(GameAction item in stateActions)
            item.Action();  
    }
}
