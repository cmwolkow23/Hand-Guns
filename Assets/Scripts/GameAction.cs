using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class GameAction
{
    //every host must call this before it runs a sequence for the first time. Actions that
    //capture original state (SetToOriginPositionGA, SetStartPositionGA, SwitchMaterialGA)
    //are empty or wrong until Initialize has run, and capturing lazily on first Action()
    //would snapshot the object after it had already moved.
    public static void InitializeAll(IEnumerable<GameAction> actions)
    {
        if(actions == null)
            return;

        foreach(GameAction item in actions)
        {
            if(item != null)
                item.Initialize();
        }
    }

    //Reset rewinds an action to its pre-sequence state: PositionLerpGA cancels its
    //coroutine and WaitForScreenFadeGA drops its static event subscription, but
    //ScaleLerpGA and SwitchMaterialGA restore scale and material. Call this only where
    //undoing the sequence is what you want, never straight after running one.
    public static void ResetAll(IEnumerable<GameAction> actions)
    {
        if(actions == null)
            return;

        foreach(GameAction item in actions)
        {
            if(item != null)
                item.Reset();
        }
    }

    public string name;
    public float delay;
    protected bool bState; //tracks whether the action is complete
    protected Collider collider; //used for actions that require a collider trigger

    public void SetCollider(Collider value)
    {
        collider = value;
    }
    public void SetState(bool value)
    {
        bState = value;
    }
    public void SetDelay(float value)
    {
        delay = value;
    }
    public bool GetState => bState;
    public float GetDelay => delay;
    public Collider GetCollider => collider;
    public virtual void Action(){}
    public virtual void Initialize(){}
    public virtual void Reset(){}
    public virtual void Setup(){} //used for editor setup
}
