using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string defaultPath = Application.persistentDataPath;
    private const string extension = ".ccs";
    public static void Save(GameData gData)
    {
        Save(gData,defaultPath,"SaveData" + extension);
    }
    public static void Save(GameData gData,string name)
    {
        Save(gData, defaultPath,name + extension);
    }
    private static void Save(GameData gData,string path, string name)
    {
        //creates the final path
        string savePath = Path.Combine(path,name);

        //convert data 
        string jsonData = JsonUtility.ToJson(gData,true);

        //save to disk
        File.WriteAllText(savePath,jsonData);
        Debug.Log("Game saved");
    }
    public static GameData Load()
    {
        return Load("SaveData");
    }
    public static GameData Load(string name)
    {
        //create path
        string loadPath = Path.Combine(defaultPath,name + extension);
        Debug.Log("Loading from: " + loadPath);
        //load file
        if(File.Exists(loadPath))
        {
            //loads text data
            string jsonData = File.ReadAllText(loadPath);

            //convert to game data
            GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

            Debug.LogWarning("Game loaded");
            return gameData;
        }
        Debug.LogAssertion("File not found.");
        return null;
    }
}
