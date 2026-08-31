using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class PositionLerpGA : GameAction
{
    public float duration;
    public bool bToggleTargets,bMoveTriggerer;
    public Transform positionA,positionB,objectToMove;   
    private bool bChangeDirection;
    private Coroutine moveCoroutine;
    public PositionLerpGA()
    {
        name = "Moves between points";
    }
    public override void Action()
    {
        moveCoroutine = CoroutineRunner.Instance.RunCoroutine(MoveToTarget());
    }
    public void ToggleDirections()
    {
        bChangeDirection = !bChangeDirection;
    }
    public override void Reset()
    {
        bState = false;
        bChangeDirection = false;
        if(moveCoroutine != null)
        {
            CoroutineRunner.Instance.EndCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
    IEnumerator MoveToTarget()
    {
        float rate = 0;
        Vector3 offset = Vector3.zero;

        if(bMoveTriggerer && collider != null)
        {  
            offset = collider.transform.position - objectToMove.position;
        }
        
        while(rate < 1)
        {
            rate += Time.deltaTime / duration;
            rate = Mathf.Clamp01(rate);

            if(bToggleTargets)
            {
                if(bChangeDirection)
                    objectToMove.position = Vector3.Lerp(positionB.position,positionA.position,rate);
                else
                    objectToMove.position = Vector3.Lerp(positionA.position,positionB.position,rate);
            }
            else
                objectToMove.position = Vector3.Lerp(positionA.position,positionB.position,rate);

            if(bMoveTriggerer && collider != null)
                        collider.attachedRigidbody.MovePosition(objectToMove.position + offset);
            
            yield return new WaitForEndOfFrame();
        }
        bChangeDirection = !bChangeDirection;
        bState = true;
    }
}
