using System;
using UnityEngine;
[Serializable]
public class SwitchMeshGA : GameAction
{
    public Mesh desiredMesh;
    public MeshFilter[] meshFilters;
    
    public SwitchMeshGA()
    {
        name = "Switch mesh";
    }
    public override void Action()
    {
        foreach(MeshFilter item in meshFilters)
            item.mesh = desiredMesh;
        bState = true;
    }
}
