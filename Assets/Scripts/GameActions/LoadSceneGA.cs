using System;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class LoadSceneGA : GameAction
{
    public int sceneIndex;
#if UNITY_EDITOR
    [SerializeField]
    private SceneAsset sceneToLoad;
#endif
    public LoadSceneGA()
    {
        name = "Load scene provided";
    }
    public override void Action()
    {
        //Debug.Log("Loading Scene Index: " + sceneIndex);
        GameMaster.LoadScene(sceneIndex);
        bState = true;
    }
    #if UNITY_EDITOR
    public override void Setup()
    {
        if (sceneToLoad != null)
        {
            BuildSceneTools.AddScene(AssetDatabase.GetAssetPath(sceneToLoad));
            string scenePath = AssetDatabase.GetAssetPath(sceneToLoad);
            sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
        }
    }
    public void SetSceneInfo(SceneAsset asset)
    {
        sceneToLoad = asset;        
        string scenePath = AssetDatabase.GetAssetPath(sceneToLoad);
        sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
    }
    #endif
}
