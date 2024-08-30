using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitManager : MonoBehaviour
{
    [Header("PulpitConfig Prefab")]
    [SerializeField] private GameObject pulpitPrefab;
    [SerializeField] private float pulpitSize = 9f;

    [Header("Initial Spawn")]
    [SerializeField] private Vector3 firstSpawnLocation = Vector3.zero;

    private GameConfiguration GameConfiguration;
    private Queue<GameObject> activePulpits = new Queue<GameObject>();
    private Transform activePulpitsParent;
    private int pulpitCount = 0;

    void Start()
    {
        GameConfiguration = FindObjectOfType<GameConfiguration>();
        if (GameConfiguration == null)
        {
            Debug.LogError("GameConfiguration not found in the scene.");
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
        while (true)
        {
            if (activePulpits.Count >= 2)
            {
                Destroy(activePulpits.Dequeue());
            }

            GameObject newPulpit = Instantiate(pulpitPrefab, currentSpawnLocation, Quaternion.identity, activePulpitsParent);
            pulpitCount++;
            newPulpit.name = "Pulpit " + pulpitCount;
            activePulpits.Enqueue(newPulpit);

            float pulpitLifetime = Random.Range(GameConfiguration.PulpitData.min_pulpit_destroy_time, GameConfiguration.PulpitData.max_pulpit_destroy_time);

            PulpitConfig pulpitScript = newPulpit.GetComponent<PulpitConfig>();
            if (pulpitScript != null)
            {
                pulpitScript.Initialize(pulpitLifetime);
            }
            else
            {
                Debug.LogError("PulpitConfig script missing on the pulpit prefab.");
            }

            currentSpawnLocation = GetNextSpawnLocation(currentSpawnLocation);

            yield return new WaitForSeconds(pulpitLifetime - GameConfiguration.PulpitData.pulpit_spawn_time);
        }
    }

    private Vector3 GetNextSpawnLocation(Vector3 currentLocation)
    {
        Vector3 nextLocation = currentLocation;
        int direction = Random.Range(0, 4);

        switch (direction)
        {
            case 0: // +x direction
                nextLocation = currentLocation + new Vector3(pulpitSize, 0, 0);
                break;
            case 1: // -x direction
                nextLocation = currentLocation + new Vector3(-pulpitSize, 0, 0);
                break;
            case 2: // +z direction
                nextLocation = currentLocation + new Vector3(0, 0, pulpitSize);
                break;
            case 3: // -z direction
                nextLocation = currentLocation + new Vector3(0, 0, -pulpitSize);
                break;
        }
        return nextLocation;
    }
}
