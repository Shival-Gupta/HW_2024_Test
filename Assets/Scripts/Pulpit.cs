using UnityEngine;
using TMPro;
using System.Collections;

public class Pulpit : MonoBehaviour
{
    private float pulpitLifetime = 5f;
    private bool isScored = false;
    private TextMeshPro timerText;

    void Start()
    {
        timerText = GetComponentInChildren<TextMeshPro>();
        StartCoroutine(StartCountdown());
    }

    // Initialize method to set the pulpit lifetime from PulpitManager
    public void Initialize(float lifetime)
    {
        pulpitLifetime = lifetime;
    }

    private IEnumerator StartCountdown()
    {
        float remainingTime = pulpitLifetime;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = remainingTime.ToString("F1");  // Update timer display
            yield return null;
        }

        // Once the time is up, destroy the pulpit
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isScored)
        {
            // Ensure score is only added once
            isScored = true;

            // Increment score
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AddScore(1);
            }
        }
    }
}
