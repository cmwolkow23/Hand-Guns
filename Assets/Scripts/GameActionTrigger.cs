using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GameActionTrigger : MonoBehaviour
{
    [SerializeField]
    private bool bTriggerOnce,bEntryOnly;
    [Tooltip("Colliders with this tag are ignored by the trigger. Leave empty to accept every collider")]
    public string ignoreTag;
    [Tooltip("Ignore ResetBlocks events, useful for triggers that should not be reset")]
    public bool bIgnoreReset;
    [SerializeReference,SubclassSelector]
    public List<GameAction> enterActions = new List<GameAction>(),exitActions = new List<GameAction>();
    private bool bActive,bDeActive,bTriggered,bLock,bStateChange,bExitSequence;
    private Collider objectThatTriggered;
#if UNITY_EDITOR
    [NonSerialized]
    private string actionsSignature; //serialized state of both action lists as of the last setup pass
#endif
    private void OnValidate()
    {
#if UNITY_EDITOR
        string signature = BuildActionsSignature();
        if(signature == actionsSignature)
            return; //something else on the component changed, the action lists did not

        bool bBaseline = actionsSignature == null; //first pass after a domain/scene load, not a user edit
        actionsSignature = signature;

        if(!bBaseline)
            Undo.RegisterCompleteObjectUndo(this,"Run Actions setup");

        foreach(GameAction item in enterActions)
        {
            if(item != null)
                item.Setup();
        }
        foreach(GameAction item in exitActions)
        {
            if(item != null)
                item.Setup();
        }
#endif
    }
#if UNITY_EDITOR
    //builds a snapshot of both action lists so edits made inside an action are detected too
    private string BuildActionsSignature()
    {
        StringBuilder builder = new StringBuilder();
        AppendActions(builder,enterActions);
        builder.Append('|');
        AppendActions(builder,exitActions);
        return builder.ToString();
    }
    private void AppendActions(StringBuilder builder,List<GameAction> actions)
    {
        if(actions == null)
            return;

        foreach(GameAction item in actions)
        {
            if(item == null)
            {
                builder.Append("null;");
                continue;
            }
            builder.Append(item.GetType().FullName);
            builder.Append(JsonUtility.ToJson(item)); //called on the concrete type, so SerializeReference is not a factor
            builder.Append(';');
        }
    }
#endif
    private void OnEnable()
    {
        if(!bIgnoreReset)
        {
            GameMaster.DelResetBlocks += ResetTrigger;
        }
        GameMaster.DelCheckpointReset += CheckpointReset;
        GameMaster.DelStateLock += Lock;
        GameMaster.DelStateUnlock += UnLock;
        GameMaster.DelCheckpoint += Checkpoint;

        //initialization
        GameAction.InitializeAll(enterActions);
        GameAction.InitializeAll(exitActions);
    }
    private void OnDisable()
    {
        if(!bIgnoreReset)
        {
            GameMaster.DelResetBlocks -= ResetTrigger;
        }
        GameMaster.DelCheckpointReset -= CheckpointReset;
        GameMaster.DelStateLock -= Lock;
        GameMaster.DelStateUnlock -= UnLock;
        GameMaster.DelCheckpoint -= Checkpoint;
    }
    public void TriggerEnterActions()
    {
        StartCoroutine(nameof(EnterActionSequence));
    }
    public void TriggerExitActions()
    {
        StartCoroutine(nameof(ExitActionSequence));
    }
    private void OnTriggerEnter(Collider other)
    {  
        if (bActive || bLock) return;
        if (bTriggered && bTriggerOnce) return;    
        if(!string.IsNullOrEmpty(ignoreTag) && other.CompareTag(ignoreTag)) return;

        bLock = true; // inquire about this
        objectThatTriggered = other;
        Invoke("UnLock",0.1f); //prevents multiple triggers
        StartCoroutine(nameof(EnterActionSequence));
    }
    private void OnTriggerExit(Collider other)
    {   
        if (bDeActive || bLock || bEntryOnly) return;
        if (bTriggerOnce && bTriggered) return;        
        objectThatTriggered = other;
        StartCoroutine(nameof(ExitActionSequence));
    }
    public void ExitSequence()
    {
        bExitSequence = true;
        ResetTrigger();
    }
    //stops whichever sequence is currently running without re-arming the trigger (unlike ResetTrigger)
    public void TerminateSequence()
    {
        StopAllCoroutines();
        ClearCollider();
        bActive = bDeActive = false;
    }
    public void ResetTrigger()
    {
        StopAllCoroutines();

        ClearCollider();

        GameAction.ResetAll(enterActions);
        GameAction.ResetAll(exitActions);
        bLock = bActive = bDeActive = bTriggered = bStateChange = bExitSequence =false;       
    }
    //full release, used when the trigger is reset or torn down
    private void ClearCollider()
    {
        objectThatTriggered = null;
        ClearColliders(enterActions);
        ClearColliders(exitActions);
    }
    //a finished sequence releases only its own list, so completing one cannot strip the
    //collider out from under the other while both are still running
    private void ClearColliders(List<GameAction> actions)
    {
        foreach(GameAction item in actions)
        {
            if(item != null)
                item.SetCollider(null);
        }
    }
    //to be removed since checkpoints are no longer supported. It is to be supported, refactor
    private void CheckpointReset()
    {
        if (bStateChange) return;
        bLock = bActive = bDeActive = bTriggered = false;
    }
    private void Checkpoint(Vector3 value)
    {
        if (bTriggerOnce && bTriggered)
            bStateChange = true;
    }
    public void Lock() { bLock = true; }
    public void UnLock() { bLock = false; }
    IEnumerator EnterActionSequence()
    {  
        bActive = true;
        if (bTriggerOnce)
            bTriggered = true;

        //resets states for actions
        foreach(GameAction item in enterActions)
        {
            item.SetState(false);
            item.SetCollider(objectThatTriggered);
        }
        
        //runs actions
        foreach(GameAction item in enterActions)
        {
            yield return new WaitForSeconds(item.GetDelay);
                item.Action();
            while(!item.GetState)
                yield return null;
        }
        ClearColliders(enterActions);
        bActive = false;
    }
    IEnumerator ExitActionSequence()
    {
        bDeActive = true;
       //resets states for actions
        foreach(GameAction item in exitActions)
        {
            item.SetState(false);
            item.SetCollider(objectThatTriggered);
        }
        foreach(GameAction item in exitActions)
        {
            yield return new WaitForSeconds(item.GetDelay);
                item.Action();
            while(!item.GetState)
                yield return null;
            if(bExitSequence) //if the exit sequence is triggered while the enter sequence is still running, it will stop the enter sequence and run the exit sequence
                yield break;            
        }     
        ClearColliders(exitActions);
        bDeActive = false;
    }
}
