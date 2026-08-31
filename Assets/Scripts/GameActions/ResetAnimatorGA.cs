using System;
using UnityEngine;
[Serializable]
public class ResetAnimatorGA : GameAction
{
    public Animator animator;
    public ResetAnimatorGA()
    {
        name = "Resets an animator";
    }
    public override void Action()
    {        
        if(animator)
            animator.Rebind();
        bState = true;
    }
}
