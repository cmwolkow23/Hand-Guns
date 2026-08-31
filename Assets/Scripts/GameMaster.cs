using System;
using UnityEngine;

//slim global hub for the GameAction system. It holds only the state and broadcasts that
//the action framework and the systems shipped with this template need, so a new project
//can grow its own game-specific manager alongside this without the action system
//depending on it.
public static class GameMaster
{
    private static Transform playerTransform;
    private static SwipeDirection lastSwipeDirection;
    private static int sceneToLoadIndex;
    private static GameData gameData;

    //public accessors
    //null until LoadGame() has been called. The GameSystems prefab does that on Awake
    public static GameData CurrentSave => gameData;
    public static SwipeDirection GetLastSwipeInDirection => lastSwipeDirection;
    public static int SceneToLoadIndex => sceneToLoadIndex;
    public static Transform PlayerTransform
    {
        get
        {
            return playerTransform;
        }
        set
        {
            playerTransform = value;
        }
    }

    //delegates
    //these are invoked directly, so an exception in one subscriber aborts the rest of the
    //invocation list and the listeners after it are silently skipped.

    //action framework
    //re-arms every GameActionTrigger that has not opted out via bIgnoreReset, and calls
    //Reset() on each of its actions. Raise this when a level or encounter restarts.
    public static Action DelResetBlocks = delegate { };
    //clears the triggered/active flags on triggers that have not latched a checkpoint.
    public static Action DelCheckpointReset = delegate { };
    //stops triggers from firing, used while a cutscene or transition owns the scene.
    public static Action DelStateLock = delegate { };
    public static Action DelStateUnlock = delegate { };
    //latches "trigger once" triggers that have already fired so a checkpoint reset keeps them fired.
    public static Action<Vector3> DelCheckpoint = delegate { };
    //runs the undo actions on GameActionOnStart and destroys those objects.
    public static Action DelResetPlayerDefaults = delegate { };

    //screen fade, listened to by FadeScreen
    public static Action DelFadeIn = delegate { };
    public static Action DelFadeOut = delegate { };

    //scene loading, listened to by SceneLoader
    public static Action<int> DelLoadSceneSequence = delegate { }; //fades out, loads, fades back in
    public static Action<int> DelLoadScene = delegate { };         //loads with no fade

    //raised after an achievement is written, for a HUD popup or similar
    public static Action<string> DelAchievementUnlocked = delegate { };

    //fade
    public static void FadeIn() { DelFadeIn(); }
    public static void FadeOut() { DelFadeOut(); }
    //FadeScreen reports which way the last swipe-in ran so SwipeOutGA can reverse it
    public static void SetLastSwipeDirection(SwipeDirection sDirection)
    {
        lastSwipeDirection = sDirection;
    }

    //scene loading
    //stashes a build index now so LoadNextScene can be triggered later without re-supplying it
    public static void SetSceneToLoad(int value)
    {
        sceneToLoadIndex = value;
    }
    public static void LoadSceneSequence(int value)
    {
        DelLoadSceneSequence(value);
    }
    public static void LoadScene(int value)
    {
        DelLoadScene(value);
    }
    public static void LoadNextScene()
    {
        DelLoadSceneSequence(sceneToLoadIndex);
    }

    //save/load
    //every Record* helper commits to disk immediately, so a crash never loses the beat
    //that just happened. Batch them behind your own flag if that becomes too chatty.
    public static void SaveGame()
    {
        SaveSystem.Save(gameData);
    }
    public static void SaveGame(string name)
    {
        SaveSystem.Save(gameData,name);
    }
    public static void LoadGame()
    {
        gameData = SaveSystem.Load();
        if(gameData == null)
            gameData = new GameData(); //first run, start from an empty save
    }
    public static void LoadGame(string name)
    {
        gameData = SaveSystem.Load(name);
        if(gameData == null)
            gameData = new GameData();
    }
    public static void RecordLevel(string name)
    {
        gameData.SaveLevel(name);
        SaveGame();
    }
    public static void SaveObjectState(ObjectState oState)
    {
        gameData.SaveObjectState(oState);
        SaveGame();
    }
    public static void RecordFlag(string id)
    {
        gameData.SaveFlag(id);
        SaveGame();
    }
    public static void RecordAchievement(string id)
    {
        gameData.SaveAchievements(id);
        DelAchievementUnlocked(id);
        SaveGame();
    }
}
