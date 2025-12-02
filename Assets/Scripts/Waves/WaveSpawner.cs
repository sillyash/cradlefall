using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    public WaveBehavior[] waves;
    private int waveIndex = 0;

    private float countdownTimer;
    private bool isSpawning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (waves == null || waves.Length == 0)
        {
            Debug.LogError("No waves assigned to WaveSpawner.");
            this.enabled = false;
            return;
        }

        countdownTimer = countdown;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawning)
        {
            return;
        }

        if (waveIndex >= waves.Length)
        {
            return;
        }

        countdownTimer -= Time.deltaTime;
        if (countdownTimer <= 0f)
        {
            Debug.Log("Wave Started!");
            isSpawning = true;
            StartCoroutine(SpawnCurrentWave());
        }
    }

    private IEnumerator SpawnCurrentWave()
    {
        if (waveIndex >= waves.Length)
        {
            isSpawning = false;
            yield break;
        }

        WaveBehavior wave = waves[waveIndex];

        GameObject enemyToSpawn;
        
        while ((enemyToSpawn = wave.GetNextEnemyToSpawn()) != null)
        {
            Transform spawnPoint = wave.GetRandomSpawnPoint();
            GameObject instance;
            if (spawnPoint != null)
            {
                instance = Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                instance = Instantiate(enemyToSpawn, Vector3.zero, Quaternion.identity);
            }

            // Track spawned instance so we can wait for it to be destroyed
            if (instance != null)
            {
                wave.spawnedEnemies.Add(instance);
            }

            float spawnInterval = wave.GetRandomSpawnInterval();
            if (spawnInterval > 0f)
            {
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                // Prevent tight infinite loops if a wave returns zero/negative interval
                yield return null;
            }
        }

        // Wait until all spawned enemies are destroyed before finishing the wave
        while (true)
        {
            // Clean up any null references left in the list
            for (int i = wave.spawnedEnemies.Count - 1; i >= 0; i--)
            {
                if (wave.spawnedEnemies[i] == null)
                {
                    wave.spawnedEnemies.RemoveAt(i);
                }
            }

            if (wave.spawnedEnemies.Count == 0)
            {
                break;
            }

            yield return null;
        }

        // Wave finished
        waveIndex += 1;
        isSpawning = false;

        if (waveIndex < waves.Length)
        {
            countdownTimer = countdown;
            Debug.Log("Wave finished. Next wave in " + countdownTimer + " seconds.");
        }
        else
        {
            Debug.Log("All waves completed.");
            this.enabled = false;
        }
    }
}
