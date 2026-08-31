using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class FadeScreen : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image fadeScreenImage;
    [SerializeField]
    private Material swipeMat,fadeMat;

    public static Action ScreenFadeInComplete = delegate { };
    public static Action ScreenFadeOutComplete = delegate { };   

    private int visibilityID = Shader.PropertyToID("_Visibility");
    private int offsetID = Shader.PropertyToID("_Offset");
    private int colorID = Shader.PropertyToID("_FadeScreenColor");

    private float rate;
    private SwipeDirection swipeDirection;
    private Vector2 desiredPosition;

    [SerializeField]
    private bool bActive, bFadeIn,bSwipeIn;

    private void OnEnable()
    {
        GameMaster.DelFadeIn += FadeIn;
        GameMaster.DelFadeOut += FadeOut;        
        ScreenSwipeGA.SwipeScreen += Swipe;
        SwipeOutGA.SwipeOut += Swipe;
    }
    private void OnDisable()
    {
        GameMaster.DelFadeIn -= FadeIn;
        GameMaster.DelFadeOut -= FadeOut;
        ScreenSwipeGA.SwipeScreen -= Swipe;
        SwipeOutGA.SwipeOut -= Swipe;
    }
    private void FadeIn()
    {        
        MatchResolution();
        fadeScreenImage.material = fadeMat;
        if (bFadeIn && bActive) return; //prevents multiple calls if already active

        bFadeIn = true;
        if(bActive)
        {
            rate = 1 - rate;
        }
        else
        {
            StartCoroutine(nameof(ScreenFader));
        }
    }       
    private void FadeOut()
    {
        MatchResolution();
        fadeScreenImage.material = fadeMat;
        if (!bFadeIn && bActive) return;//prevents multiple calls if already active

        bFadeIn = false;
        if(bActive) 
        {
            rate = 1 - rate;
        }
        else
        {
            StartCoroutine(nameof(ScreenFader));
        }
    }
    private void Swipe(SwipeDirection direction,bool value)
    {
        //set swipe direction
        GameMaster.SetLastSwipeDirection(direction);
        
        bSwipeIn = value;
        fadeScreenImage.material = swipeMat;
        swipeDirection = direction;
        if(swipeDirection == SwipeDirection.Down)
        {
            desiredPosition.x = 0;
            desiredPosition.y = -1;
        }
        else if(swipeDirection == SwipeDirection.Up)
        {
            desiredPosition.x = 0;
            desiredPosition.y = 1;
        }
        else if(swipeDirection == SwipeDirection.Left)
        {
            desiredPosition.x = 1;
            desiredPosition.y = 0;
        }
        else if(swipeDirection == SwipeDirection.Right)
        {
            desiredPosition.x = -1;
            desiredPosition.y = 0;
        }
        StartCoroutine(nameof(SwipeAnimation));
    }
    private void MatchResolution()
    {
        fadeScreenImage.rectTransform.anchorMin = Vector2.zero;
        fadeScreenImage.rectTransform.anchorMax = Vector2.one;
        fadeScreenImage.rectTransform.offsetMin = Vector2.zero;
        fadeScreenImage.rectTransform.offsetMax = Vector2.zero;
    }
    IEnumerator ScreenFader()
    {
        rate = 0;
        bActive = true;

        //set initial opacity state before enabling to prevent flickering
        if(bFadeIn)
            Shader.SetGlobalFloat(visibilityID,1);
        else
            Shader.SetGlobalFloat(visibilityID,0);     

        canvas.enabled = true;  
        while (rate < 1f)
        {
            yield return new WaitForEndOfFrame();            

            rate += Time.deltaTime * 2;

            if(bFadeIn)
                 Shader.SetGlobalFloat(visibilityID,Mathf.Lerp(1,0,rate));
            else
                Shader.SetGlobalFloat(visibilityID,Mathf.Lerp(0,1,rate));
        }
        if (Shader.GetGlobalFloat(visibilityID) == 0)
        {
            //Debug.Log("FadeIn complete");
            canvas.enabled = false;
            ScreenFadeInComplete();
        }
        else
        {
            //Debug.Log("FadeOut complete");
            ScreenFadeOutComplete();
        }
        bActive = false;
    }
    IEnumerator SwipeAnimation()
    {
        //prevents flicker by setting the start position
        if(bSwipeIn)
            Shader.SetGlobalVector(offsetID,Vector2.Lerp(desiredPosition,Vector2.zero,0));
        else
            Shader.SetGlobalVector(offsetID,Vector2.Lerp(Vector2.zero,desiredPosition,0));

        rate = 0;
        canvas.enabled = true;          

        while(rate < 1)
        {
            rate += Time.deltaTime * 5;

            yield return new WaitForEndOfFrame();
            if(bSwipeIn)
                Shader.SetGlobalVector(offsetID,Vector2.Lerp(desiredPosition,Vector2.zero,rate));
            else
                Shader.SetGlobalVector(offsetID,Vector2.Lerp(Vector2.zero,desiredPosition,rate));
        }

        if(bSwipeIn)
            ScreenFadeInComplete();
        else
        {
            ScreenFadeOutComplete();    
            canvas.enabled = false;
        }
    }
}
public enum SwipeDirection {Right,Left,Up,Down}
