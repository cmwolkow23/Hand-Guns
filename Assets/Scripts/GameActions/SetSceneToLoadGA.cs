using System;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SetSceneToLoadGA : GameAction
{
    public int sceneIndex;
    public SetSceneToLoadGA()
    {
        name = "Sets the next scene to load";
    }
#if UNITY_EDITOR
    [SerializeField]
    private SceneAsset sceneToLoad;
#endif
    public override void Action()
    {
        GameMaster.SetSceneToLoad(sceneIndex);
        bState = true;
    }

    public override void Setup()
    {
#if UNITY_EDITOR
        if(Selection.activeGameObject != null)
            Undo.RecordObject(Selection.activeGameObject, "Set Scene Index");
        if (sceneToLoad != null)
        {
            string scenePath = AssetDatabase.GetAssetPath(sceneToLoad);
            sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
        }
#endif
    }
}
