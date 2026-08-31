using System;
using UnityEngine;
[Serializable]
public class TagCheckGA : GameAction
{
    public string tagToCheck;
    public GameActionTrigger gat;
    public TagCheckGA()
    {
        name = "Checks collider tag against provided string";
    }
    public override void Action()
    {
        bool bTagMatches = collider != null && collider.CompareTag(tagToCheck);
        if (!bTagMatches)
            gat.ExitSequence();
        bState = true;
    }
}
