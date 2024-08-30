using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    private GameManager gameManager;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        finalScoreText.text = "Final Score: " + finalScore;

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("[GameOverScene] GameManager not found!");
        }
    }

    public void RetryGame()
    {
        if (gameManager != null)
        {
            gameManager.RetryGame();
        }
    }

    public void ExitGame()
    {
        if (gameManager != null)
        {
            gameManager.ExitGame();
        }
    }
}