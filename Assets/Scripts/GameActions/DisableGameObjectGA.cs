using System;
using UnityEngine;

[Serializable]
public class DisableGameObjectGA : GameAction
{
   public bool bIgnoreReset;
   public GameObject[] objectsToDisable;
   public DisableGameObjectGA()
    {
      name = "Disable GameObjects";
    }
   public override void Action()
   {
      foreach (GameObject item in objectsToDisable)
         item.SetActive(false);
      bState = true;
   }
}
