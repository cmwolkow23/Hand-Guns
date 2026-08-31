using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class RotateTowardsGA : GameAction
{
    public bool bSnap;
    public float rotationTime = 0.25f;
    public Transform targetToRotate;
    public Transform triggerer;
    protected Vector3 direction;
    protected Quaternion desiredRotation;
    public RotateTowardsGA()
    {
        name ="Rotate towards triggerer";
    }
    public override void Action()
    {
        //allows for the triggerer to be set externally if needed otherwise use object that collided
        if(triggerer == null)
            triggerer = collider.transform;

        direction = (triggerer.position - targetToRotate.transform.position).normalized;        

        if(bSnap)
        {
            direction.y = 0;
            direction = SnapToAxis(direction);
        }

        desiredRotation = Quaternion.LookRotation(direction);
        CoroutineRunner.Instance.StartCoroutine(RotateToward());
    }
    protected IEnumerator RotateToward()
    {
        float rate = 0;      

        Quaternion currentRotation = targetToRotate.rotation;
        while(rate < 1)
        {
            rate += 1/rotationTime * Time.deltaTime;
            targetToRotate.rotation = Quaternion.Lerp(currentRotation,desiredRotation,rate);            
            yield return new WaitForEndOfFrame();
        }
        bState = true;
    }
    protected static Vector3 SnapToAxis(Vector3 v) //makes available to all
    {
        float ax = Mathf.Abs(v.x), ay = Mathf.Abs(v.y), az = Mathf.Abs(v.z);
        if (ax >= ay && ax >= az) return new Vector3(Mathf.Sign(v.x), 0, 0);
        if (ay >= az)             return new Vector3(0, Mathf.Sign(v.y), 0);
        return new Vector3(0, 0, Mathf.Sign(v.z));
    }
}
