using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitManager : MonoBehaviour
{
    [Header("Pulpit Prefab")]
    [SerializeField] private GameObject pulpitPrefab;
    [SerializeField] private float pulpitSize = 9f;

    [Header("Initial Spawn")]
    [SerializeField] private Vector3 firstSpawnLocation = Vector3.zero;

    private GameConfig gameConfig;
    private Queue<GameObject> activePulpits = new Queue<GameObject>();

    void Start()
    {
        gameConfig = FindObjectOfType<GameConfig>();
        if (gameConfig == null)
        {
            Debug.LogError("GameConfig not found in the scene.");
            return;
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
                Destroy(activePulpits.Dequeue());   // Dequeing oldest pulpit
                yield return new WaitForSeconds(gameConfig.PulpitData.pulpit_spawn_time);
            }
            
            GameObject newPulpit = Instantiate(pulpitPrefab, currentSpawnLocation, Quaternion.identity);
            activePulpits.Enqueue(newPulpit);

            currentSpawnLocation = GetNextSpawnLocation(currentSpawnLocation);

            float destroyTime = Random.Range(gameConfig.PulpitData.min_pulpit_destroy_time, gameConfig.PulpitData.max_pulpit_destroy_time);
            yield return new WaitForSeconds(destroyTime);
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
