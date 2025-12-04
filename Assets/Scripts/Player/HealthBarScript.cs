using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;          
    public basePlayer player;      

    [Header("Smooth settings")]
    public bool smooth = true;    
    public float smoothSpeed = 10f;

    private void Start()
    {
        if (player != null)
        {
            slider.maxValue = (int)player.stats.health;
            slider.value = (int)player.health;
        }
    }

    private void Update()
    {
        if (player == null) return;

        //Debug.Log("Vie du joueur : " + player.health);

        if (smooth)
        {
            slider.value = Mathf.Lerp(slider.value, player.health, Time.deltaTime * smoothSpeed);
        }
        else
        {
            slider.value = player.health;
        }
    }
}
