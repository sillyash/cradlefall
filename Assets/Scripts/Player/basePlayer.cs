using UnityEngine;

public class basePlayer : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    void Start()
    {
        if (!stats)
        {
            Debug.LogError($"Le Joueur {this.name} n'a pas SO stats");
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
