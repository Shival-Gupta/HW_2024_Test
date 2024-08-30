using UnityEngine;
using TMPro;
using System.Collections;

public class PulpitConfig : MonoBehaviour
{
    private float pulpitLifetime = 5f;
    private TextMeshPro timerText;
    private BoxCollider boxCollider;
    private bool hasBeenScored = false;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        timerText = GetComponentInChildren<TextMeshPro>();

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider not found");
            return;
        }
        if (transform.childCount == 0)
        {
            Debug.LogError("Pulpit Prefab has no children.");
            return;
        }

        Transform firstChild = transform.GetChild(0);
        boxCollider.size = firstChild.localScale;

        if (timerText == null)
            Debug.LogWarning("Text (TMP) component missing on the pulpit prefab.");

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenScored)
        {
            hasBeenScored = true;

            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AddScore();
            }
        }
    }
}
