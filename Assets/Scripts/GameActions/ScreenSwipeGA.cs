using System;
using UnityEngine;

[Serializable]
public class ScreenSwipeGA : GameAction
{
    public Transform localTransform;// used to determine exiting direction    
    public bool bSwipeIn;
    
    public ScreenSwipeGA()
    {
        name = "Performs screen swipes";
    }

    public static Action<SwipeDirection,bool> SwipeScreen = delegate{};
    public override void Action()
    {
        Vector3 enteringDirection = (localTransform.position - GameMaster.PlayerTransform.position).normalized;
        enteringDirection = new Vector3(Mathf.RoundToInt(enteringDirection.x),0,Mathf.RoundToInt(enteringDirection.z));
       
        if(enteringDirection.x < 0)
            SwipeScreen(SwipeDirection.Left,bSwipeIn);
        else if(enteringDirection.x > 0)
            SwipeScreen(SwipeDirection.Right,bSwipeIn);
        else if(enteringDirection.z < 0)
            SwipeScreen(SwipeDirection.Up,bSwipeIn);
        else if(enteringDirection.z > 0)
            SwipeScreen(SwipeDirection.Down,bSwipeIn);
        else
            SwipeScreen(SwipeDirection.Up,bSwipeIn);
        bState = true;
    }
}
