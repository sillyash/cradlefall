using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class WaveSpawner : NetworkBehaviour
{
    [SerializeField] private float countdown;
    public WaveBehavior[] waves;
    public event System.Action WaveFinished; // Pour affichage HUD
    public event System.Action WaveStarted; // Pour affichage HUD
    private bool isSpawning = false;


    public NetworkVariable<int> waveIndex = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    
    public NetworkVariable<float> countdownTimer = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        Debug.Log("[Network] WaveSpawner spawned.");
        GetNetworkHooks();
        if ( ! IsServer) return;

        if (waves == null || waves.Length == 0)
        {
            Debug.LogError("[Server] No waves assigned to WaveSpawner.");
            this.enabled = false;
            return;
        }

        countdownTimer.Value = countdown;
    }

    public override void OnNetworkDespawn()
    {
        waveIndex.OnValueChanged -= OnWaveIndexChanged;
    }

    // Setup network variable change callbacks
    private void GetNetworkHooks()
    {
        waveIndex.OnValueChanged += OnWaveIndexChanged;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"[WaveSpawner] Wave {waveIndex.Value + 1}.");
        if ( ! IsServer) return;
        if (isSpawning) return;
        if (waveIndex.Value >= waves.Length) return;

        countdownTimer.Value -= Time.deltaTime;

        if (countdownTimer.Value <= 0f)
        {
            Debug.Log("[Server] Wave Started!");
            WaveStarted?.Invoke();
            isSpawning = true;
            StartCoroutine(SpawnCurrentWave());
        }
    }

    private IEnumerator SpawnCurrentWave()
    {
        if (waveIndex.Value >= waves.Length)
        {
            isSpawning = false;
            yield break;
        }

        WaveBehavior wave = waves[waveIndex.Value];

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
        waveIndex.Value += 1;
        WaveFinished?.Invoke();
        isSpawning = false;

        if (waveIndex.Value < waves.Length)
        {
            countdownTimer.Value = countdown;
        }
        else
        {
            Debug.Log("All waves completed.");
            this.enabled = false;
            GameManager.instance.Victory();
        }
    }

    /* ------------- Network Variable Callbacks ------------- */

    private void OnWaveIndexChanged(int previousValue, int newValue)
    {
        Debug.Log($"[Network] Wave index changed from {previousValue} to {newValue}");
    }

    /* ------------- Getters & Setters ------------- */

    public int CurrentWaveNumber => waveIndex.Value + 1; // Pour affichage HUD
    public float TimeUntilNextWave => countdownTimer.Value;
}
