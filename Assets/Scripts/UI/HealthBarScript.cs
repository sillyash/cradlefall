using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;          
    public GameObject damageable;
    private IDamageable thing;

    [Header("Smooth settings")]
    public bool smooth = true;    
    public float smoothSpeed = 10f;

    private void Start()
    {
        damageable.TryGetComponent(out thing);

        if (thing != null)
        {
            slider.maxValue = (int)thing.maxHealth;
            slider.value = (int)thing.health;
        }
    }

    private void Update()
    {
        if (thing == null) return;

        if (smooth)
        {
            slider.value = Mathf.Lerp(slider.value, thing.health, Time.deltaTime * smoothSpeed);
        }
        else
        {
            slider.value = thing.health;
        }
    }
}
