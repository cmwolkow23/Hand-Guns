using System;
using UnityEngine;
[Serializable]
public class SwipeOutGA : GameAction
{
    public SwipeOutGA()
    {
        name = "Swipes out in the opposite direction of the last swipe in";
    }
    public static Action<SwipeDirection,bool> SwipeOut = delegate{};
    public override void Action()
    {
        if(GameMaster.GetLastSwipeInDirection == SwipeDirection.Left)
            SwipeOut(SwipeDirection.Right,false);
        else if(GameMaster.GetLastSwipeInDirection == SwipeDirection.Right)
            SwipeOut(SwipeDirection.Left,false);
        else if(GameMaster.GetLastSwipeInDirection == SwipeDirection.Up)
            SwipeOut(SwipeDirection.Down,false);
        else if(GameMaster.GetLastSwipeInDirection == SwipeDirection.Down)
            SwipeOut(SwipeDirection.Up,false);
        else
            SwipeOut(SwipeDirection.Down,false);
        bState = true;
    }
}
