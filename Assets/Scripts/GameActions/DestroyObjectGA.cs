using System;
using UnityEngine;
[Serializable]
public class DestroyObjectGA : GameAction
{
   public GameObject[] objectsToDestroy;
   public DestroyObjectGA()
   {
       name = "Destroys Object(s)";
   }
   public override void Action()
   {
       foreach (GameObject obj in objectsToDestroy)
       {
           if (obj != null)
               GameObject.Destroy(obj);
       }
       bState = true;
   }
}
