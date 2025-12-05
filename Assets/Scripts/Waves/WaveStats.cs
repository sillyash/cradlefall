using UnityEngine;

[CreateAssetMenu(fileName = "WaveStats", menuName = "Wave/WaveStats")]
public class WaveStats : ScriptableObject
{
    // Enemies that can be spawned in this wave
    public GameObject[] availableEnemies;

    // Points available to spend on enemies in this wave
    public int spendablePoints;

    // Minimum and maximum spawn intervals between enemies
    public float minSpawnInterval;
    public float maxSpawnInterval;

    // Time to next wave after this one ends
    public float timeToNextWave;
}
