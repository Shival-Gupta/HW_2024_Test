using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitManager : MonoBehaviour
{
    [SerializeField] private GameObject pulpitPrefab;
    [SerializeField] private float pulpitSize = 9f;
    [SerializeField] private Vector3 firstSpawnLocation = Vector3.zero;
    [SerializeField] private Color startColor = Color.green;
    [SerializeField] private Color endColor = new Color(0.6f, 0.2f, 0f);

    private GameConfiguration gameConfiguration;
    private Queue<GameObject> activePulpits = new Queue<GameObject>();
    private Transform activePulpitsParent;
    private int pulpitCount = 0;

    void Start()
    {
        gameConfiguration = FindObjectOfType<GameConfiguration>();
        if (gameConfiguration == null)
        {
            Debug.LogError("Game Configuration not found in the scene.");
            return;
        }

        activePulpitsParent = GameObject.Find("ActivePulpits")?.transform;
        if (activePulpitsParent == null)
        {
            activePulpitsParent = new GameObject("ActivePulpits").transform;
        }

        StartCoroutine(SpawnPulpits());
    }

    private IEnumerator SpawnPulpits()
    {
        Vector3 currentSpawnLocation = firstSpawnLocation;
        while (gameConfiguration.isPlayerAlive)
        {
            if (activePulpits.Count >= 2)
            {
                Destroy(activePulpits.Dequeue());
            }

            GameObject newPulpit = Instantiate(pulpitPrefab, currentSpawnLocation, Quaternion.identity, activePulpitsParent);
            pulpitCount++;
            newPulpit.name = "Pulpit " + pulpitCount;
            activePulpits.Enqueue(newPulpit);

            float pulpitLifetime = Random.Range(gameConfiguration.PulpitData.min_pulpit_destroy_time, gameConfiguration.PulpitData.max_pulpit_destroy_time);

            PulpitConfig pulpitScript = newPulpit.GetComponent<PulpitConfig>();
            if (pulpitScript != null)
            {
                pulpitScript.Initialize(pulpitLifetime, startColor);
            }
            else
            {
                Debug.LogError("PulpitConfig script missing on the pulpit prefab.");
            }

            currentSpawnLocation = GetNextSpawnLocation(currentSpawnLocation);

            yield return new WaitForSeconds(pulpitLifetime - gameConfiguration.PulpitData.pulpit_spawn_time);
        }
    }

    private Vector3 GetNextSpawnLocation(Vector3 currentLocation)
    {
        Vector3 nextLocation = currentLocation;
        int direction = Random.Range(0, 4);

        switch (direction)
        {
            case 0: 
                nextLocation = currentLocation + new Vector3(pulpitSize, 0, 0);
                break;
            case 1: 
                nextLocation = currentLocation + new Vector3(-pulpitSize, 0, 0);
                break;
            case 2: 
                nextLocation = currentLocation + new Vector3(0, 0, pulpitSize);
                break;
            case 3:
                nextLocation = currentLocation + new Vector3(0, 0, -pulpitSize);
                break;
        }
        return nextLocation;
    }
}