using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public void IncrementScore()
    {
        score++;
        Debug.Log(score);
    }
    public void AddScore(int value)
    {
        score += value;
        Debug.Log(score);
    }
}
