using System;
using UnityEngine;
[Serializable]
public class RandomYRotationGA : GameAction
{
    public Transform localObject;

    public RandomYRotationGA()
    {
        name = "Randomly rotates along the Y";
    }
    public override void Action()
    {
        int index = UnityEngine.Random.Range(0,4);

        switch(index)
        {
            case 0:
                localObject.localRotation = Quaternion.Euler(new Vector3(0,0,0));
                break;
            case 1:
                localObject.localRotation = Quaternion.Euler(new Vector3(0,90,0));
                    break;
            case 2:
                localObject.localRotation = Quaternion.Euler(new Vector3(0,180,0));
                    break;
            case 3:
                localObject.localRotation = Quaternion.Euler(new Vector3(0,270,0));
                    break;
            default:
                localObject.localRotation = Quaternion.Euler(new Vector3(0,0,0));
                    break;
        }
        bState = true;
    }
}
