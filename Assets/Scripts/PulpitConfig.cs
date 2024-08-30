using UnityEngine;
using TMPro;
using System.Collections;

public class PulpitConfig : MonoBehaviour
{
    private float pulpitLifetime = 5f;
    private TextMeshPro timerText;

    void Start()
    {
        timerText = GetComponentInChildren<TextMeshPro>();
        if (timerText == null)
            Debug.LogError("TextMeshPro component missing on the pulpit prefab.");
        else
            StartCoroutine(StartCountdown());
    }

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
            if (timerText != null)
            {
                timerText.text = remainingTime.ToString("F1");
            }
            yield return null;
        }

        Destroy(gameObject);
    }
}