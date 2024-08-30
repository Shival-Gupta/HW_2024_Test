using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        if (scoreText == null)
            Debug.LogError("ScoreText TextMeshProUGUI component not found!");
        else
            UpdateScoreDisplay();
    }

    public void AddScore(int value = 1)
    {
        score += value;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}