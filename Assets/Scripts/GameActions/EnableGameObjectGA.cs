using System;
using UnityEngine;

[Serializable]
public class EnableGameObjectGA : GameAction
{
   public GameObject[] objectsToEnable;
   public EnableGameObjectGA()
    {
      name = "Enable GameObjects";
    }
   public override void Action()
   {
      foreach (GameObject item in objectsToEnable)
         item.SetActive(true);
      bState = true;
   }
}
