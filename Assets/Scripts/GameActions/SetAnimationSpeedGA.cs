using System;
using UnityEngine;
[Serializable]
public class SetAnimationSpeedGA : GameAction
{
    public float speed = 1f;
    public Animator animator;

    public SetAnimationSpeedGA()
    {
        name ="Sets the speed of the animator";
    }
    public override void Action()
    {
        animator.speed = speed;
        bState = true;
    }
}
