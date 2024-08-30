using UnityEngine;
using System.IO;

public class GameConfiguration : MonoBehaviour
{
    public bool isPlayerAlive = true;
    public float playerDieThreshold = -8f;
    
    [Header("Configuration File")]
    [SerializeField] private string jsonFilePath = "Assets/Resources/doofus_diary.json";

    [Header("Default Configuration")]
    [SerializeField] private PlayerData playerData = new PlayerData();
    [SerializeField] private PulpitData pulpitData = new PulpitData();

    public PlayerData PlayerData => playerData;
    public PulpitData PulpitData => pulpitData;

    private void Awake()
    {
        LoadSettingsFromJson();
    }

    private void LoadSettingsFromJson()
    {
        if (string.IsNullOrEmpty(jsonFilePath))
        {
            Debug.LogError("JSON file path is not set. Proceeding with Default Configuration.");
            return;
        }

        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError($"JSON file not found at: {jsonFilePath}. Proceeding with Default Configuration.");
            return;
        }

        try
        {
            string json = File.ReadAllText(jsonFilePath);
            GameSettingsWrapper settingsWrapper = JsonUtility.FromJson<GameSettingsWrapper>(json);

            if (settingsWrapper.player_data != null)
            {
                playerData = settingsWrapper.player_data;
            }
            else
            {
                Debug.LogWarning("Player data not found in the JSON file. Using default values.");
            }

            if (settingsWrapper.pulpit_data != null)
            {
                pulpitData = settingsWrapper.pulpit_data;
            }
            else
            {
                Debug.LogWarning("Pulpit data not found in the JSON file. Using default values.");
            }

            Debug.Log($"Settings loaded successfully from {jsonFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load settings from JSON: {e.Message}");
        }
    }
}

[System.Serializable]
public class GameSettingsWrapper
{
    public PlayerData player_data;
    public PulpitData pulpit_data;
}

[System.Serializable]
public class PlayerData
{
    public float speed = 3f;
}

[System.Serializable]
public class PulpitData
{
    public float min_pulpit_destroy_time = 4f;
    public float max_pulpit_destroy_time = 5f;
    public float pulpit_spawn_time = 2.5f;
}
