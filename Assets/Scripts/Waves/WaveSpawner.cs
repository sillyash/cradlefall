using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class WaveSpawner : NetworkBehaviour
{

    [Header("Musique d'intro pour chaque vague")]
    [SerializeField] private AudioClip introMusicClip;
    [SerializeField, Range(0f,1f)] private float introMusicVolume = 1f;
    [SerializeField] private AudioSource introMusicSource;
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
        if ( ! IsServer) return;
        if (isSpawning) return;
        if (waveIndex.Value >= waves.Length) return;

        countdownTimer.Value -= Time.deltaTime;

        if (countdownTimer.Value <= 0f)
        {
            // Joue la musique d'intro en superposition, sans couper l'ambiance
            if (introMusicClip != null)
            {
                if (introMusicSource == null)
                {
                    introMusicSource = gameObject.AddComponent<AudioSource>();
                    introMusicSource.playOnAwake = false;
                    introMusicSource.loop = false;
                    introMusicSource.spatialBlend = 0f;
                }
                introMusicSource.clip = introMusicClip;
                introMusicSource.volume = introMusicVolume;
                introMusicSource.Play();
            }

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

            if (spawnPoint == null)
            {
                spawnPoint.position = Vector3.zero;
                spawnPoint.rotation = Quaternion.identity;
            }

            GameObject instance;

            if (wave.deadEnemies.Count > 0)
            {
                int lastIndex = wave.deadEnemies.Count - 1;

                //Reuse dead enemy
                instance = wave.deadEnemies[lastIndex];
                wave.deadEnemies.RemoveAt(lastIndex);

                instance.GetComponent<BaseEnemy>().resetEnemy(spawnPoint);
            } else
            {
                instance = Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
                instance.GetComponent<NetworkObject>().Spawn();
            }

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
        while (wave.spawnedEnemies.Count > 0 || wave.enemiesToSpawn.Count > 0)
        {
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
            this.enabled = false;
            GameManager.instance.Victory();
        }
    }

    /* ------------- Network Variable Callbacks ------------- */

    private void OnWaveIndexChanged(int previousValue, int newValue)
    {
        //Debug.Log($"[Network] Wave index changed from {previousValue} to {newValue}");
    }

    /* ------------- Getters & Setters ------------- */

    public int CurrentWaveNumber => waveIndex.Value; // Pour affichage HUD
    public float TimeUntilNextWave => countdownTimer.Value;
}
