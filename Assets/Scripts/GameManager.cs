using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadStartScene()
    {
        Debug.Log("Loading Start Scene...");
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }

    public void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void LoadGameOverScene()
    {
        Debug.Log("Loading Game Over Scene...");
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            PlayerPrefs.SetInt("FinalScore", scoreManager.Score);
        }
        else
        {
            Debug.LogError("ScoreManager not found. Make sure it's in the scene!");
        }

        SceneManager.LoadScene("GameOverScene", LoadSceneMode.Additive);
    }

    public void RetryGame()
    {
        Debug.Log("Retrying Game...");
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
