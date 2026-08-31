using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("CoroutineRunner");
                _instance = obj.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    private void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnSceneUnloaded(Scene current)
    {
        //Claude bug fix: underlying object can already be destroyed here (dangling sceneUnloaded subscription), causing a MissingReferenceException
        if (this == null) return;
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void EndCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }
}
