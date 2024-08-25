using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitManager : MonoBehaviour
{
    [Header("Initial Spawn Settings")]
    [Tooltip("The starting location for the first pulpit.")]
    [SerializeField] private Vector3 firstSpawnLocation = Vector3.zero;

    [Header("Pulpit Prefab")]
    [Tooltip("Prefab of the pulpit to spawn.")]
    [SerializeField] private GameObject pulpitPrefab;

    private GameConfig gameConfig;
    private Queue<GameObject> activePulpits = new Queue<GameObject>();

    void Start()
    {
        // Find the GameConfig script in the scene
        gameConfig = FindObjectOfType<GameConfig>();

        if (gameConfig == null)
        {
            Debug.LogError("GameConfig not found in the scene. Please ensure a GameObject with the GameConfig script is present.");
            return;
        }

        // Start the pulpit spawning process
        StartCoroutine(SpawnPulpits());
    }

    private IEnumerator SpawnPulpits()
    {
        Vector3 currentSpawnLocation = firstSpawnLocation;

        while (true)
        {
            // Check if there are more than 2 pulpits in the scene and destroy the oldest one if necessary
            if (activePulpits.Count >= 2)
            {
                // Destroy the oldest pulpit
                GameObject oldestPulpit = activePulpits.Dequeue();
                Destroy(oldestPulpit);

                // Wait for the pulpit_spawn_time before spawning the next pulpit
                yield return new WaitForSeconds(gameConfig.PulpitData.pulpit_spawn_time);
            }

            // Spawn a new pulpit
            GameObject newPulpit = Instantiate(pulpitPrefab, currentSpawnLocation, Quaternion.identity);
            activePulpits.Enqueue(newPulpit);

            // Determine the next spawn location
            currentSpawnLocation = GetNextSpawnLocation(currentSpawnLocation);

            // Wait for the next pulpit's destroy time before spawning the next pulpit
            float destroyTime = Random.Range(gameConfig.PulpitData.min_pulpit_destroy_time, gameConfig.PulpitData.max_pulpit_destroy_time);
            yield return new WaitForSeconds(destroyTime);
        }
    }

    private Vector3 GetNextSpawnLocation(Vector3 currentLocation)
    {
        // Define the possible directions
        Vector3[] directions = new Vector3[]
        {
            new Vector3(9, 0, 0),  // +x direction
            new Vector3(-9, 0, 0), // -x direction
            new Vector3(0, 0, 9),  // +z direction
            new Vector3(0, 0, -9)  // -z direction
        };

        // Choose a random direction
        Vector3 nextLocation = currentLocation + directions[Random.Range(0, directions.Length)];

        // Ensure the next pulpit doesn't spawn in the same position
        while (activePulpits.Count > 0 && nextLocation == activePulpits.Peek().transform.position)
        {
            nextLocation = currentLocation + directions[Random.Range(0, directions.Length)];
        }

        return nextLocation;
    }
}
