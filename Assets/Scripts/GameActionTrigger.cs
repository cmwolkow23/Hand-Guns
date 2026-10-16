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
    private bool bTriggerOnce, bEntryOnly;
    [Tooltip("Legacy single ignored tag. Existing scene values are still honored; use Ignore Tags to configure additional tags.")]
    public string ignoreTag;
    [Tooltip("Tags whose colliders are ignored by the trigger. Leave empty to accept every tag not listed in the legacy field.")]
    public List<string> ignoreTags = new List<string>();
    [Tooltip("Ignore ResetBlocks events, useful for triggers that should not be reset")]
    public bool bIgnoreReset;
    public bool ignoreShootRaycast = false;

    [Header("Action Set A")]
    [SerializeReference, SubclassSelector]
    public List<GameAction> enterActions = new List<GameAction>(), exitActions = new List<GameAction>();

    [Header("Action Set B")]
    [SerializeReference, SubclassSelector]
    public List<GameAction> alternateEnterActions = new List<GameAction>(), alternateExitActions = new List<GameAction>();

    private bool bActive, bDeActive, bTriggered, bLock, bStateChange, bExitSequence;
    private bool bUseAlternateActions, bNextActivationUsesAlternateActions;
    private Collider objectThatTriggered;
#if UNITY_EDITOR
    [NonSerialized]
    private string actionsSignature; //serialized state of all action lists as of the last setup pass
#endif
    private void OnValidate()
    {
#if UNITY_EDITOR
        string signature = BuildActionsSignature();
        if (signature == actionsSignature)
            return; //something else on the component changed, the action lists did not

        bool bBaseline = actionsSignature == null; //first pass after a domain/scene load, not a user edit
        actionsSignature = signature;

        if (!bBaseline)
            Undo.RegisterCompleteObjectUndo(this, "Run Actions setup");

        SetupActions(enterActions);
        SetupActions(exitActions);
        SetupActions(alternateEnterActions);
        SetupActions(alternateExitActions);
#endif
    }
#if UNITY_EDITOR
    //builds a snapshot of all action lists so edits made inside an action are detected too
    private string BuildActionsSignature()
    {
        StringBuilder builder = new StringBuilder();
        AppendActions(builder, enterActions);
        builder.Append('|');
        AppendActions(builder, exitActions);
        builder.Append('|');
        AppendActions(builder, alternateEnterActions);
        builder.Append('|');
        AppendActions(builder, alternateExitActions);
        return builder.ToString();
    }
    private void AppendActions(StringBuilder builder, List<GameAction> actions)
    {
        if (actions == null)
            return;

        foreach (GameAction item in actions)
        {
            if (item == null)
            {
                builder.Append("null;");
                continue;
            }
            builder.Append(item.GetType().FullName);
            builder.Append(JsonUtility.ToJson(item)); //called on the concrete type, so SerializeReference is not a factor
            builder.Append(';');
        }
    }
    private void SetupActions(List<GameAction> actions)
    {
        if (actions == null)
            return;

        foreach (GameAction item in actions)
        {
            if (item != null)
                item.Setup();
        }
    }
#endif
    private void OnEnable()
    {
        if (!bIgnoreReset)
        {
            GameMaster.DelResetBlocks += ResetTrigger;
        }
        GameMaster.DelCheckpointReset += CheckpointReset;
        GameMaster.DelStateLock += Lock;
        GameMaster.DelStateUnlock += UnLock;
        GameMaster.DelCheckpoint += Checkpoint;

        GameAction.InitializeAll(enterActions);
        GameAction.InitializeAll(exitActions);
        GameAction.InitializeAll(alternateEnterActions);
        GameAction.InitializeAll(alternateExitActions);
    }
    private void OnDisable()
    {
        if (!bIgnoreReset)
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
        if (HasAlternateActions())
        {
            bUseAlternateActions = bNextActivationUsesAlternateActions;
            bNextActivationUsesAlternateActions = !bNextActivationUsesAlternateActions;
        }
        else
        {
                       bUseAlternateActions = false;
            bNextActivationUsesAlternateActions = false;
        }
            StartCoroutine(nameof(EnterActionSequence));
    }
    public void TriggerEnterActions(Collider triggeringCollider)
    {
        if (IsIgnoredTag(triggeringCollider))
            return;

        objectThatTriggered = triggeringCollider;
        TriggerEnterActions();
    }
    public void TriggerExitActions()
    {
        StartCoroutine(nameof(ExitActionSequence));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsIgnoredTag(other)) return;
        if (bActive || bLock) return;
        if (bTriggered && bTriggerOnce) return;

        bLock = true;
        objectThatTriggered = other;
        Invoke(nameof(UnLock), 0.1f);
        TriggerEnterActions();
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsIgnoredTag(other)) return;
        if (bDeActive || bLock || bEntryOnly) return;
        if (bTriggerOnce && bTriggered) return;

        objectThatTriggered = other;
        StartCoroutine(nameof(ExitActionSequence));
    }
    private bool IsIgnoredTag(Collider other)
    {
        if (other == null)
            return false;

        if (!string.IsNullOrEmpty(ignoreTag) && other.CompareTag(ignoreTag))
            return true;

        if (ignoreTags == null)
            return false;

        foreach (string tag in ignoreTags)
        {
            if (!string.IsNullOrEmpty(tag) && other.CompareTag(tag))
                return true;
        }

        return false;
    }
    private List<GameAction> GetCurrentEnterActions()
    {
        return bUseAlternateActions ? alternateEnterActions : enterActions;
    }
    private List<GameAction> GetCurrentExitActions()
    {
        return bUseAlternateActions ? alternateExitActions : exitActions;
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
        GameAction.ResetAll(alternateEnterActions);
        GameAction.ResetAll(alternateExitActions);
        bLock = bActive = bDeActive = bTriggered = bStateChange = bExitSequence = false;
    }
    //full release, used when the trigger is reset or torn down
    private void ClearCollider()
    {
        objectThatTriggered = null;
        ClearColliders(enterActions);
        ClearColliders(exitActions);
        ClearColliders(alternateEnterActions);
        ClearColliders(alternateExitActions);
    }
    //a finished sequence releases only its own list
    private void ClearColliders(List<GameAction> actions)
    {
        if (actions == null)
            return;

        foreach (GameAction item in actions)
        {
            if (item != null)
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

        List<GameAction> actions = GetCurrentEnterActions();

        if (actions != null)
        {
            foreach (GameAction item in actions)
            {
                if (item == null)
                    continue;

                item.SetState(false);
                item.SetCollider(objectThatTriggered);
            }

            foreach (GameAction item in actions)
            {
                if (item == null)
                    continue;

                yield return new WaitForSeconds(item.GetDelay);
                item.Action();
                while (!item.GetState)
                    yield return null;
            }

            ClearColliders(actions);
        }

        bActive = false;
    }
    IEnumerator ExitActionSequence()
    {
        bDeActive = true;
        List<GameAction> actions = GetCurrentExitActions();

        if (actions != null)
        {
            foreach (GameAction item in actions)
            {
                if (item == null)
                    continue;

                item.SetState(false);
                item.SetCollider(objectThatTriggered);
            }

            foreach (GameAction item in actions)
            {
                if (item == null)
                    continue;

                yield return new WaitForSeconds(item.GetDelay);
                item.Action();
                while (!item.GetState)
                    yield return null;
                if (bExitSequence)
                    yield break;
            }

            ClearColliders(actions);
        }

        bDeActive = false;
    }

    private bool HasAlternateActions()
    {
        return HasActions(alternateEnterActions) || HasActions(alternateExitActions);
    }
    private bool HasActions(List<GameAction> actions)
    {
        if (actions == null)
            return false;

        foreach (GameAction item in actions)
        {
            if (item != null)
                return true;
        }

        return false;
    }
}       