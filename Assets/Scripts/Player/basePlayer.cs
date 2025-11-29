using UnityEngine;

public class basePlayer : MonoBehaviour
{
    public PlayerStats stats;
    [SerializeField] private float health;

    void Start()
    {
        if (!stats)
        {
            Debug.LogError($"Le Joueur {this.name} n'a pas SO stats");
            enabled = false;
            return;
        }

        health = stats.health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
