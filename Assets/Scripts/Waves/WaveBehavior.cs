using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveBehavior : MonoBehaviour
{
    // Statistics and configuration for the wave
    [SerializeField] private WaveStats stats;

    // Spawn points available for this wave
    public Transform[] spawnPoints;

    // Variables used in logic sections
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    private int remainingPoints;
    public int enemiesLeft;
    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        remainingPoints = stats.spendablePoints;
        FillEnemiesToSpawn();
        enemiesLeft = enemiesToSpawn.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedEnemies.Count > 0)
        {
            //Get index of an enemy for the current frame
            index = Time.frameCount % spawnedEnemies.Count;

            if (spawnedEnemies[index] == null)
            {
                Debug.Log("removing enemy");
                spawnedEnemies.RemoveAt(index);
                enemiesLeft = Mathf.Max(0, enemiesToSpawn.Count + spawnedEnemies.Count);
            }
            else
            {
                // run logic for this enemy
                spawnedEnemies[index].GetComponent<BaseEnemy>().runMainLogic();
            }
        }
    }

    public GameObject GetNextEnemyToSpawn()
    {
        if (enemiesToSpawn.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, enemiesToSpawn.Count);
        GameObject enemyToSpawn = enemiesToSpawn[randomIndex];
        enemiesToSpawn.RemoveAt(randomIndex);

        return enemyToSpawn;
    }

    private void FillEnemiesToSpawn()
    {
        int minEnemyCost = GetMinEnemyCost();
        while (remainingPoints >= minEnemyCost)
        {
            // Select a random enemy from the available enemies
            GameObject randomEnemy = stats.availableEnemies[Random.Range(0, stats.availableEnemies.Length)];
            int enemyCost = randomEnemy.GetComponent<BaseEnemy>().GetCost();

            if (remainingPoints >= enemyCost && randomEnemy != null)
            {
                enemiesToSpawn.Add(randomEnemy);
                remainingPoints -= enemyCost;
            }
            else
            {
                // Not enough points to add this enemy, break
                break;
            }
        }
        for (int i = enemiesToSpawn.Count - 1; i >= 0; i--)
        {
            if (enemiesToSpawn[i] == null)
            {
                enemiesToSpawn.RemoveAt(i);
            }
        }
    }

    private int GetMinEnemyCost()
    {
        int minCost = int.MaxValue;
        foreach (GameObject enemy in stats.availableEnemies)
        {
            int cost = enemy.GetComponent<BaseEnemy>().GetCost();
            if (cost < minCost)
            {
                minCost = cost;
            }
        }
        return minCost;
    }

    public Transform GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex];
    }

    public float GetRandomSpawnInterval()
    {
        return Random.Range(stats.minSpawnInterval, stats.maxSpawnInterval);
    }
}
