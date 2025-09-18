using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Networking;
#endif

public static class DefaultJsonLoader
{
    public static string LoadDefaultJson(string filename)
    {
        string path = Path.Combine(Application.streamingAssetsPath, filename);

#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest www = UnityWebRequest.Get(path);
        www.SendWebRequest();
        while (!www.isDone) { }
        if (www.result == UnityWebRequest.Result.Success)
            return www.downloadHandler.text;
        else
            Debug.LogError("Failed to load default JSON: " + www.error);
        return null;
#else
        return File.ReadAllText(path);
#endif
    }
}

public class GameStatsManager : MonoBehaviour
{
    private const string StatsFileName = "game_stats.json";

    public static GameStatsManager Instance { get; private set; }
    public GameData.GameStatsData Data { get; private set; }
    public DefaultData.LevelData LevelData { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadStats()
    {
        string path = Path.Combine(Application.persistentDataPath, StatsFileName);
        bool hasExistingData = File.Exists(path);
        if (hasExistingData)
        {
            string json = File.ReadAllText(path);
            Data = JsonUtility.FromJson<GameData.GameStatsData>(json);
        }
        else
        {
            Data = new GameData.GameStatsData();
        }
        string levelDataJson = DefaultJsonLoader.LoadDefaultJson(DefaultValueFileName(Data.currentLevel));
        if (levelDataJson != null)
        {
            LevelData = JsonUtility.FromJson<DefaultData.LevelData>(levelDataJson);
            if (!hasExistingData)
            {
                Data.playerInventory.content = new Dictionary<FullProductId, int>();
                Data.standsData.stands = new Dictionary<FullProductId, GameData.StandData>();
                foreach (FullProductId id in Data.standsData.stands.Keys)
                {
                    Data.standsData.stands[id].basePrice = LevelData.stands[id].basePrice;
                    Data.standsData.stands[id].preparationDuration = LevelData.stands[id].basePreparationDuration;
                }
                SaveStats();
            }
        }
        else
        {
            Debug.LogError("Default values file does not exists!");
        }
    }

    public void SaveStats()
    {
        string path = Path.Combine(Application.persistentDataPath, StatsFileName);
        string json = JsonUtility.ToJson(Data);
        File.WriteAllText(path, json);
    }

    private string DefaultValueFileName(int level)
    {
        return $"data_level_{level}.json";
    }
}
