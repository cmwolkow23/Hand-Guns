using System;
using UnityEngine;
[Serializable]
public class PlayOneShotGA : GameAction
{
    public AudioSource source;
    public AudioClip clip;
    [Range(0,1)]
    public float volume = 1;

    public PlayOneShotGA()
    {
        name = "Play One Shot";
    }
    public override void Action()
    {
        if(source == null)
        {
            Debug.LogError("PlayOneShotGA: source is null.");
            bState = true;
            return;
        }
        if(clip == null)
        {
            Debug.LogError("PlayOneShotGA: clip is null.");
            bState = true;
            return;
        }
        //PlayOneShot layers on top of whatever the source is already playing and does not
        //cancel a previous shot, so overlapping calls are safe
        source.PlayOneShot(clip,volume);
        bState = true;
    }
    public override void Setup()
    {
        if(clip == null)
            name = "Play One Shot";
        else
            name = "Play One Shot: " + clip.name;
    }
}
