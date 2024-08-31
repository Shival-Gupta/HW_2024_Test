using UnityEngine;
using TMPro;
using System.Collections;

public class PulpitConfig : MonoBehaviour
{
    [SerializeField] private float pulseScale = 0.8f;
    [SerializeField] private float pulseDuration = 0.15f;

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

        StartCoroutine(PulseSize(pulseScale, pulseDuration));
        StartCoroutine(StartCountdown());
        StartCoroutine(ManagePulpitVisuals());
    }

    public void Initialize(float lifetime, Color initialColor)
    {
        pulpitLifetime = lifetime;

        MeshRenderer pulpitRenderer = GetComponentInChildren<MeshRenderer>();
        if (pulpitRenderer != null)
            pulpitRenderer.material.color = initialColor;
        else
            Debug.LogError("[PulpitConfig] MeshRenderer not found on pulpit or its children!");
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

    private IEnumerator ManagePulpitVisuals()
    {
        MeshRenderer pulpitRenderer = GetComponentInChildren<MeshRenderer>();
        if (pulpitRenderer == null)
        {
            Debug.LogError("[PulpitConfig] MeshRenderer not found on pulpit or its children!");
            yield break;
        }

        Color startColor = pulpitRenderer.material.color;
        Color endColor = new Color(0.6f, 0.2f, 0f); 

        float elapsedTime = 0f;
        while (elapsedTime < pulpitLifetime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / pulpitLifetime; 
            pulpitRenderer.material.color = Color.Lerp(startColor, endColor, t); 
            yield return null;
        }
    }

    private IEnumerator PulseSize(float targetScale, float duration)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScaleVector = originalScale * targetScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(targetScaleVector, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}