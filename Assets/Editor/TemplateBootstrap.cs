#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//first-time scaffolding for the template: switches the project to URP and authors the
//FadeScreen and GameSystems prefabs. Idempotent, so re-running it is harmless.
//Delete this file once the project is set up.
public static class TemplateBootstrap
{
    private const string pipelinePath = "Assets/Settings/TemplateRenderPipeline.asset";
    private const string rendererPath = "Assets/Settings/TemplateRenderer.asset";
    private const string fadeScreenPath = "Assets/Prefabs/FadeScreen.prefab";
    private const string gameSystemsPath = "Assets/Prefabs/GameSystems.prefab";

    [MenuItem("Tools/Game Template/Run First-Time Setup")]
    public static void RunFromMenu()
    {
        Setup();
    }
    //batchmode entry point: Unity -batchmode -executeMethod TemplateBootstrap.Run
    public static void Run()
    {
        Setup();
        EditorApplication.Exit(0);
    }
    private static void Setup()
    {
        SetupUniversalRenderPipeline();
        //a clean scene per build, so nothing from a previous pass leaks into the prefab
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        BuildFadeScreenPrefab();
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        BuildGameSystemsPrefab();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Verify(fadeScreenPath);
        Verify(gameSystemsPath);
        Debug.Log("TemplateBootstrap: setup complete");
    }
    //reads the saved asset back off disk and reports what actually landed in it
    private static void Verify(string path)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError("TemplateBootstrap VERIFY: " + path + " did not save");
            return;
        }
        string report = "TemplateBootstrap VERIFY: " + path;
        foreach (Component component in prefab.GetComponentsInChildren<Component>(true))
            report += "\n  " + component.gameObject.name + " -> " + component.GetType().Name;
        Debug.Log(report);
    }
    //SaveAsPrefabAsset onto an existing path merges into that asset, so clear it first
    private static void ReplaceAsset(string path)
    {
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            AssetDatabase.DeleteAsset(path);
    }
    private static void SetupUniversalRenderPipeline()
    {
        Directory.CreateDirectory("Assets/Settings");

        UniversalRenderPipelineAsset pipeline =
            AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(pipelinePath);

        if (pipeline == null)
        {
            UniversalRendererData rendererData =
                AssetDatabase.LoadAssetAtPath<UniversalRendererData>(rendererPath);
            if (rendererData == null)
            {
                rendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
                AssetDatabase.CreateAsset(rendererData,rendererPath);
            }
            pipeline = UniversalRenderPipelineAsset.Create(rendererData);
            AssetDatabase.CreateAsset(pipeline,pipelinePath);
            AssetDatabase.SaveAssets();
        }
        GraphicsSettings.defaultRenderPipeline = pipeline;
        QualitySettings.renderPipeline = pipeline;
        Debug.Log("TemplateBootstrap: URP pipeline assigned");
    }
    private static void BuildFadeScreenPrefab()
    {
        Material fadeMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/UI/Fade.mat");
        Material swipeMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/UI/Swipe.mat");

        GameObject root = new GameObject("FadeScreen");
        root.AddComponent<DonotDestroy>();

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100; //draws over everything else
        CanvasScaler scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920,1080);

        GameObject imageObject = new GameObject("FadeImage",typeof(Image));
        imageObject.transform.SetParent(root.transform,false);
        Image image = imageObject.GetComponent<Image>();
        image.material = fadeMat;
        image.raycastTarget = false; //the fade must never swallow input
        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        FadeScreen fadeScreen = root.AddComponent<FadeScreen>();
        SerializedObject serialized = new SerializedObject(fadeScreen);
        serialized.FindProperty("canvas").objectReferenceValue = canvas;
        serialized.FindProperty("fadeScreenImage").objectReferenceValue = image;
        serialized.FindProperty("fadeMat").objectReferenceValue = fadeMat;
        serialized.FindProperty("swipeMat").objectReferenceValue = swipeMat;
        serialized.ApplyModifiedPropertiesWithoutUndo();

        canvas.enabled = false; //the fade coroutines switch this on when they run

        Directory.CreateDirectory("Assets/Prefabs");
        ReplaceAsset(fadeScreenPath);
        PrefabUtility.SaveAsPrefabAsset(root,fadeScreenPath);
        Object.DestroyImmediate(root);
        Debug.Log("TemplateBootstrap: FadeScreen.prefab built");
    }
    private static void BuildGameSystemsPrefab()
    {
        GameObject root = new GameObject("GameSystems");
        root.AddComponent<DonotDestroy>();
        root.AddComponent<SceneLoader>();

        //loads the save on Awake so GameMaster.CurrentSave is populated before any
        //SaveStateLoader hits its Start, and before SaveOnExit can write to it
        AwakeActions awakeActions = root.AddComponent<AwakeActions>();
        SerializedObject serialized = new SerializedObject(awakeActions);
        SerializedProperty actions = serialized.FindProperty("actions");
        actions.arraySize = 1;
        actions.GetArrayElementAtIndex(0).managedReferenceValue = new LoadGA();
        serialized.ApplyModifiedPropertiesWithoutUndo();

        Directory.CreateDirectory("Assets/Prefabs");
        ReplaceAsset(gameSystemsPath);
        PrefabUtility.SaveAsPrefabAsset(root,gameSystemsPath);
        Object.DestroyImmediate(root);
        Debug.Log("TemplateBootstrap: GameSystems.prefab built");
    }
}
#endif
