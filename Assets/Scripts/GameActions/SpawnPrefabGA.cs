using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnPrefabGA : GameAction
{
    public SpawnPrefabGA()
    {
        name = "Spawn Prefab";
    }

    [Header("Prefab")]
    [Tooltip("The prefab to spawn.")]
    public GameObject prefab;

    [Header("Spawn Transform")]
    [Tooltip("If set, this transform's position/rotation are used.\nIf null, uses the position/rotation values below.")]
    public Transform spawnPoint;

    [Tooltip("World position to spawn at (used if SpawnPoint is null).")]
    public Vector3 position = Vector3.zero;

    [Tooltip("World rotation to spawn with (used if SpawnPoint is null).")]
    public Vector3 eulerRotation = Vector3.zero;

    [Header("Parenting")]
    [Tooltip("Optional parent for the spawned instance.")]
    public Transform parent;

    [Header("Advanced")]
    [Tooltip("If true, position/rotation will be interpreted as local to Parent when SpawnPoint is null.")]
    public bool useLocalSpaceWhenNoSpawnPoint = false;

    [Header("Spawn Limit")]
    [Min(1)]
    [Tooltip("Maximum number of prefabs this action can have spawned at once.")]
    public int maxSpawnedPrefabs = 10;

    [NonSerialized]
    private List<GameObject> spawnedInstances;

    public override void Action()
    {
        if (prefab == null)
        {
            Debug.LogWarning("[SpawnPrefabAction] No prefab assigned.");
            bState = true;
            return;
        }

        Vector3 spawnPos;
        Quaternion spawnRot;

        if (spawnPoint != null)
        {
            spawnPos = spawnPoint.position;
            spawnRot = spawnPoint.rotation;
        }
        else
        {
            spawnRot = Quaternion.Euler(eulerRotation);

            if (useLocalSpaceWhenNoSpawnPoint && parent != null)
            {
                // Convert local to world relative to parent
                spawnPos = parent.TransformPoint(position);
                spawnRot = parent.rotation * spawnRot;
            }
            else
            {
                spawnPos = position;
            }
        }

        List<GameObject> instances = GetSpawnedInstances();

        // Remove instances that have been destroyed by other code.
        for (int i = instances.Count - 1; i >= 0; i--)
        {
            if (instances[i] == null)
                instances.RemoveAt(i);
        }

        int spawnLimit = Mathf.Max(1, maxSpawnedPrefabs);
        while (instances.Count >= spawnLimit)
        {
            GameObject oldestInstance = instances[0];
            instances.RemoveAt(0);

            if (oldestInstance != null)
                UnityEngine.Object.Destroy(oldestInstance);
        }

        GameObject instance = UnityEngine.Object.Instantiate(prefab, spawnPos, spawnRot, parent);
#if UNITY_EDITOR
        instance.name = $"{prefab.name} (Spawned)";
#endif
        instances.Add(instance);

        bState = true;
    }

    private List<GameObject> GetSpawnedInstances()
    {
        if (spawnedInstances == null)
            spawnedInstances = new List<GameObject>();

        return spawnedInstances;
    }
}