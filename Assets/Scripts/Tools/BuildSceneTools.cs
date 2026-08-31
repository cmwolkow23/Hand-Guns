#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;

public static class BuildSceneTools
{
    //adds a scene to the build settings scene list if it isn't already present.
    public static void AddScene(string path, bool enabled = true)
    {
        //already in the list? bail
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(
            EditorBuildSettings.scenes);
        if (scenes.Exists(s => s.path == path))
            return;

        scenes.Add(new EditorBuildSettingsScene(path, enabled));
        EditorBuildSettings.scenes = scenes.ToArray(); //must reassign the whole array to persist
    }
}
#endif