//handles the loading of the game state
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStateLoader : MonoBehaviour
{
    //assign one of these two. saveObject restores a transform that SaveOnExit or
    //SaveObjectGA wrote, levelRequirementCheck reveals something once a level is complete
    public GameObject saveObject;
    public string levelRequirementCheck;

    //saveStateActions replay the saved state of the object,
    //showActions are the reveal sequence when the requirement is met,
    //unloadActions hide the object when it is not
    [SerializeReference, SubclassSelector]
    public List<GameAction> saveStateActions, showActions, unloadActions;

    private void Start()
    {
        //must run before LoadState, which moves saveObject. Actions that capture an
        //original transform have to snapshot the authored one, not the restored one.
        GameAction.InitializeAll(saveStateActions);
        GameAction.InitializeAll(showActions);
        GameAction.InitializeAll(unloadActions);

        if(GameMaster.CurrentSave != null)
            LoadState();
    }
    private void OnDisable()
    {
        GameAction.ResetAll(saveStateActions);
        GameAction.ResetAll(showActions);
        GameAction.ResetAll(unloadActions);
    }
    private void LoadState()
    {
        if(saveObject != null)
        {
            //no saved entry means the object has never moved, so it stays where it was authored
            if(GameMaster.CurrentSave.TryGetObjectState(saveObject.name + saveObject.scene.name,out ObjectState state))
            {
                saveObject.transform.position = state.position;
                saveObject.transform.rotation = Quaternion.Euler(state.rotation);
                StartCoroutine(LoadSequence(saveStateActions));
            }
        }
        else if(GameMaster.CurrentSave.IsLevelComplete(levelRequirementCheck))
        {
            StartCoroutine(LoadSequence(showActions));
        }
        else
        {
            StartCoroutine(LoadSequence(unloadActions));
        }
    }
    IEnumerator LoadSequence(List<GameAction> loadActions)
    {
        foreach (GameAction action in loadActions)
        {
            yield return new WaitForSeconds(action.delay);
                action.Action();
            while(!action.GetState)
                 yield return null;
        }
    }
}
