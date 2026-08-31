using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private int sceneIndex;

    public static Action DisableCameraDamp = delegate { };
    public static Action EnableCameraDamp = delegate { };
    public static Action UpdatePlayerTransform = delegate { };
    public static Action<int> SceneLoadStart = delegate { };
    public static Action<int> SceneLoadComplete = delegate { };
    private bool bFadeOutComplete, bLoading;   
   
    private void OnEnable()
    {
        GameMaster.DelLoadSceneSequence += LoadSceneSequence;
        GameMaster.DelLoadScene += LoadScene;
        FadeScreen.ScreenFadeOutComplete += FadeOutComplete;
    }
    private void OnDisable()
    {
        GameMaster.DelLoadSceneSequence -= LoadSceneSequence;
        GameMaster.DelLoadScene -= LoadScene;
        FadeScreen.ScreenFadeOutComplete -= FadeOutComplete;
    }
    private void LoadSceneSequence(int value)
    {
        Debug.Log("Loading scene sequence for scene index: " + value);
        sceneIndex = value;
        if (bLoading) return;
        StartCoroutine(nameof(SceneLoadingSequence));
    }
    private void LoadScene(int value)
    {
        sceneIndex = value;
        StartCoroutine(nameof(SceneLoadingWithoutFade));
    }
    private void FadeOutComplete() 
    { 
        bFadeOutComplete = true;
    }
    IEnumerator SceneLoadingSequence()
    {
        bLoading = true;
        bFadeOutComplete = false;        
        GameMaster.FadeOut(); 

        while (!bFadeOutComplete)
            yield return new WaitForEndOfFrame();  

        SceneLoadStart(sceneIndex);
        AsyncOperation loader = SceneManager.LoadSceneAsync(sceneIndex);
        
        while(loader.progress != 1)
            yield return new WaitForEndOfFrame();       
        SceneLoadComplete(sceneIndex);  
        
        //updates player for camera tracking
        DisableCameraDamp();
        UpdatePlayerTransform();
        //yield return new WaitForSeconds(1);
        EnableCameraDamp();        
        
        bLoading = false;
        bFadeOutComplete = false;
    }
    IEnumerator SceneLoadingWithoutFade()
    {
        SceneLoadStart(sceneIndex);
        AsyncOperation loader = SceneManager.LoadSceneAsync(sceneIndex);
        
        while(loader.progress != 1)
            yield return new WaitForEndOfFrame();
        SceneLoadComplete(sceneIndex);
    }
}
