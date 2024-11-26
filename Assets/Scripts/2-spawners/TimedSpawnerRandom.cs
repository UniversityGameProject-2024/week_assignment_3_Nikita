﻿using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

/**
 * This component instantiates a given prefab at random time intervals and random bias from its object position.
 */
public class TimedSpawnerRandom: MonoBehaviour {
    [SerializeField] Mover prefabToSpawn;
    [SerializeField] BossMover prefabBoss;
    [SerializeField] Vector3 velocityOfSpawnedObject;
    [Tooltip("Minimum time between consecutive spawns, in seconds")] [SerializeField] float minTimeBetweenSpawns = 0.2f;
    [Tooltip("Maximum time between consecutive spawns, in seconds")] [SerializeField] float maxTimeBetweenSpawns = 1.0f;
    [Tooltip("Maximum distance in X between spawner and spawned objects, in meters")] [SerializeField] float maxXDistance = 1.5f;
    [SerializeField] Transform parentOfAllInstances;

    void Start() {
        if (prefabToSpawn != null)
        {
            SpawnRoutine();
        }
        if (prefabBoss != null)
        {
            BossSpawnRoutine();
        }
    }

    async void SpawnRoutine() {
        while (true) {
            float timeBetweenSpawnsInSeconds = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
            await Awaitable.WaitForSecondsAsync(timeBetweenSpawnsInSeconds);       // co-routines
            if (!this) break;   // might be destroyed when moving to a new scene
            Vector3 positionOfSpawnedObject = new Vector3(
                transform.position.x + Random.Range(-maxXDistance, +maxXDistance),
                transform.position.y,
                transform.position.z);
            GameObject newObject = Instantiate(prefabToSpawn.gameObject, positionOfSpawnedObject, Quaternion.identity);
            newObject.GetComponent<Mover>().SetVelocity(velocityOfSpawnedObject);
            newObject.transform.parent = parentOfAllInstances;
        }
    }

    private void BossSpawnRoutine()
    {
        // Wait for a random amount of time before spawning the boss
        float timeBetweenSpawnsInSeconds = 1f;
        // Check if this object is still valid (not destroyed)
        if (!this) return;

        // Calculate the position for spawning the boss
        Vector3 positionOfSpawnedObject = new Vector3(
            transform.position.x + Random.Range(-maxXDistance, +maxXDistance),
            transform.position.y,
            transform.position.z);

        // Instantiate the boss object
        GameObject newObject = Instantiate(prefabBoss.gameObject, positionOfSpawnedObject, Quaternion.identity);


    }
}
