using UnityEngine;
using System.IO;

public class GameConfig : MonoBehaviour
{
    [Header("Configuration File")]
    [Tooltip("Path to the JSON configuration file.")]
    [SerializeField] private string jsonFilePath = "Assets/Resources/doofus_diary.json";

    [Header("Default Player Configuration")]
    [Tooltip("Settings related to the player character.")]
    [SerializeField] private PlayerData playerData = new PlayerData();

    [Header("Default Pulpit Configuration")]
    [Tooltip("Settings related to the pulpit platforms.")]
    [SerializeField] private PulpitData pulpitData = new PulpitData();

    public PlayerData PlayerData => playerData; // Public getter for PlayerData
    public PulpitData PulpitData => pulpitData; // Public getter for PulpitData

    private void Start()
    {
        LoadSettingsFromJson();
    }

    private void LoadSettingsFromJson()
    {
        if (string.IsNullOrEmpty(jsonFilePath))
        {
            Debug.LogError("JSON file path is not set.");
            return;
        }

        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError($"JSON file not found at: {jsonFilePath}");
            return;
        }

        try
        {
            // Read the JSON file
            string json = File.ReadAllText(jsonFilePath);

            // Deserialize JSON into a wrapper class
            GameSettingsWrapper settingsWrapper = JsonUtility.FromJson<GameSettingsWrapper>(json);

            // Assign values if the wrapper contains data
            if (settingsWrapper.player_data != null)
            {
                playerData = settingsWrapper.player_data;
            }
            else
            {
                Debug.LogWarning("Player data not found in the JSON file.");
            }

            if (settingsWrapper.pulpit_data != null)
            {
                pulpitData = settingsWrapper.pulpit_data;
            }
            else
            {
                Debug.LogWarning("Pulpit data not found in the JSON file.");
            }

            Debug.Log("Settings loaded successfully.");
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
    public float speed = 3.0f;
}

[System.Serializable]
public class PulpitData
{
    public float min_pulpit_destroy_time = 4.0f;
    public float max_pulpit_destroy_time = 5.0f;
    public float pulpit_spawn_time = 2.5f;
}
