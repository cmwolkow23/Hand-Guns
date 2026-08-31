using System;
using UnityEngine;
[Serializable]
public class AddForceGA : GameAction
{
    public bool bExplosion;
    public float forceIntensity;
    public Vector3 forceDirection;
    public float explosionRadius;
    public Transform explosionPosition;
    public Rigidbody[] rigidbodies;

    public AddForceGA()
    {
        name = "Adds force to rigidbodies";
    }
    public override void Action()
    {
        foreach(Rigidbody item in rigidbodies)
        {
            if(bExplosion)
                item.AddExplosionForce(forceIntensity,explosionPosition.position,explosionRadius);
            else 
                item.AddForce(forceDirection * forceIntensity,ForceMode.Impulse);
        }
        bState = true;
    }
}
