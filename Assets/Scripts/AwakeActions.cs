using System.Collections;
using UnityEngine;

public class AwakeActions : MonoBehaviour
{
    public float delay;
    [SerializeReference,SubclassSelector]
    public GameAction[] actions;

    public void Awake()
    {
        GameAction.InitializeAll(actions);
        StartCoroutine(TriggerActions());
    }
    //actions that run on CoroutineRunner outlive this object, so they must be cancelled here
    private void OnDisable()
    {
        GameAction.ResetAll(actions);
    }
    IEnumerator TriggerActions()
    {
        yield return new WaitForSeconds(delay);

        foreach(GameAction item in actions)
        {
            yield return new WaitForSeconds(item.delay);
                item.Action();

            while(!item.GetState)
                yield return null;            
        }
    }
}
