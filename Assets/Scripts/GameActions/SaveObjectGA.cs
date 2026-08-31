using System;
using UnityEngine;
[Serializable]
public class SaveObjectGA : GameAction
{
    public GameObject[] gameObjects;
    public SaveObjectGA()
    {
        name = "Saves object state";
    }
    public override void Action()
    {
        foreach(GameObject item in gameObjects)
        {       
            GameMaster.SaveObjectState(new ObjectState(item.name + item.scene.name,
                                item.transform.position,item.transform.rotation.eulerAngles));
        }
        bState = true;
    }
}
