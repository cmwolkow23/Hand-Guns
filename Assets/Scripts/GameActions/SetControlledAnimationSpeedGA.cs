using System;
using UnityEngine;

[Serializable]
public class SetAnimationSpeedParameterGA : GameAction
{
    public float speed = 1f;
    public Animator animator;

    // The exact name of the Float Parameter you made in the Animator tab (e.g., "AnimSpeed")
    public string parameterName = "AnimSpeed";

    // The exact name of the Animation State node in your Animator grid
    public string stateName = "YourAnimationState";

    public SetAnimationSpeedParameterGA()
    {
        name = "Sets the parameter speed and playhead of the animator";
    }

    public override void Action()
    {
        if (animator != null)
        {
            // If playing forward from a stopped state, snap to the beginning (0f)
            if (speed > 0f)
            {
                animator.Play(stateName, layer: 0, normalizedTime: 0f);
            }
            // If playing in reverse from a stopped state, snap to the absolute end (1f)
            else if (speed < 0f)
            {
                animator.Play(stateName, layer: 0, normalizedTime: 1f);
            }

            // Sets the Float parameter inside your Animator window
            animator.SetFloat(parameterName, speed);
        }

        bState = true;
    }
}
