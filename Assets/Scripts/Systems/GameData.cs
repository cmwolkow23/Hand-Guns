using System;
using System.Collections.Generic;
using UnityEngine;

//the serialized save payload. JsonUtility writes this whole object to disk, so every field
//here must be a serializable type. Add your own game's fields alongside these.
[Serializable]
public class GameData
{
    //this list is additive
    public List<string> savedLevels = new List<string>();
    public List<string> achievements = new List<string>();
    public List<string> flags = new List<string>();
    public List<ObjectState> objectStates = new List<ObjectState>();
    //add your data types in here

    public bool IsLevelComplete(string levelName) => savedLevels.Contains(levelName);
    public bool IsAchievementComplete(string achievementID) => achievements.Contains(achievementID);
    public bool IsFlagSet(string flagID) => flags.Contains(flagID);

    public void SaveLevel(string levelName)
    {
        if(!savedLevels.Contains(levelName))
            savedLevels.Add(levelName);
    }
    public void SaveAchievements(string achievementId)
    {
        if(!achievements.Contains(achievementId))
            achievements.Add(achievementId);
    }
    public void SaveFlag(string flagId)
    {
        if(!flags.Contains(flagId))
            flags.Add(flagId);
    }
    public void SaveObjectState(ObjectState oState)
    {
        for(int i = 0; i < objectStates.Count; i++)
        {
            if(objectStates[i].objectName == oState.objectName)
            {
                objectStates[i] = oState; //existing entry — override with the new state
                return;
            }
        }
        objectStates.Add(oState); //no existing entry — add it
    }
    public bool TryGetObjectState(string objectName, out ObjectState state)
    {
        foreach(ObjectState item in objectStates)
        {
            if(item.objectName == objectName)
            {
                state = item;
                return true;
            }
        }
        state = default;
        return false;
    }
    public void ClearSave()
    {
        savedLevels.Clear();
        achievements.Clear();
        flags.Clear();
        objectStates.Clear();
    }
}
//a transform snapshot keyed by name. SaveOnExit and SaveObjectGA build the key as
//gameObject.name + scene.name so the same object name in two scenes stays distinct.
[Serializable]
public struct ObjectState
{
    public string objectName;
    public Vector3 position,rotation;

    public ObjectState(string name, Vector3 pos, Vector3 rot)
    {
        objectName = name;
        position = pos;
        rotation = rot;
    }
};
