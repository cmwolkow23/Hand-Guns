using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ToggleAnimatorBoolGA : GameAction
{
    public Animator animator;
    public List<string> boolParameterNames = new List<string>();

    public ToggleAnimatorBoolGA()
    {
        name = "Toggle Animator Bool Parameter(s)";
    }

    public override void Action()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is not assigned.");
            bState = true;
            return;
        }

        if (boolParameterNames != null)
        {
            foreach (string parameterName in boolParameterNames)
            {
                if (string.IsNullOrWhiteSpace(parameterName))
                    continue;

                bool foundBoolParameter = false;

                foreach (AnimatorControllerParameter parameter in animator.parameters)
                {
                    if (parameter.name != parameterName)
                        continue;

                    if (parameter.type == AnimatorControllerParameterType.Bool)
                    {
                        bool currentValue = animator.GetBool(parameter.nameHash);
                        animator.SetBool(parameter.nameHash, !currentValue);
                        foundBoolParameter = true;
                    }

                    break;
                }

                if (!foundBoolParameter)
                    Debug.LogWarning($"Animator parameter '{parameterName}' was not found or is not a Bool parameter.", animator);
            }
        }

        bState = true;
    }
}