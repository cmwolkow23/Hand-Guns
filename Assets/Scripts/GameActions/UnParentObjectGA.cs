using System;
using UnityEngine;
using UnityEngine.SceneManagement;
[Serializable]
public class UnParentObjectGA : GameAction
{
    public Transform target;
    public UnParentObjectGA()
    {
        name = "Unparents object";
    }
    public override void Action()
    {
        if(target != null)   
        {     
            target.parent = null;
            SceneManager.MoveGameObjectToScene(target.gameObject, SceneManager.GetActiveScene());
        }
        bState = true;
    }
}
