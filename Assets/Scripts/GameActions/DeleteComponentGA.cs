using System;
using UnityEngine;
[Serializable]
public class DeleteComponentGA : GameAction
{
    public Component localComponent;

    public DeleteComponentGA()
    {
        name = "Deletes component";
    }
    public override void Action()
    {
        if(Application.isEditor)
            UnityEngine.Object.DestroyImmediate(localComponent);
        else
            UnityEngine.Object.Destroy(localComponent);
        bState = true;
    }
}
