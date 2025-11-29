using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    public WaveBehavior[] waves;
    private int waveIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (waves.Length == 0)
        {
            Debug.LogError("No waves assigned to WaveSpawner.");
            this.enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            Debug.Log("Wave Started!");
            StartCoroutine(SpawnWave());
        }

    }

    private IEnumerator SpawnWave()
    {
        while (waveIndex < waves.Length)
        {
            WaveBehavior wave = waves[waveIndex];
            Transform spawnPoint = wave.GetRandomSpawnPoint();
            GameObject enemyToSpawn = wave.GetNextEnemyToSpawn();

            Instantiate(enemyToSpawn, spawnPoint);

            float spawnInterval = wave.GetRandomSpawnInterval();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
