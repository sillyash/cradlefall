using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public GameObject damageable;
    private IDamageable thing;

    [Header("HP Text")]
    public TMP_Text hpText;

    [Header("Effects")]
    public float shakeDuration = 0.2f;
    public float shakeStrength = 5f;
    public Color damageColor = Color.red;
    public float effectDuration = 0.3f;

    [Header("Smooth settings")]
    public bool smooth = false;
    public float smoothSpeed = 10f;

    private float lastHealth;

    private Vector3 originalTextPos;
    private Color originalOutlineColor;
    private float originalOutlineWidth;

    private Image fillImage;

    private void Start()
    {
        damageable.TryGetComponent(out thing);

        if (thing != null)
        {
            slider.maxValue = (int)thing.maxHealth;
            slider.value = (int)thing.health;
            lastHealth = thing.health;

            UpdateHPText();
        }

        // Récupérer le fill du slider
        fillImage = slider.fillRect.GetComponent<Image>();

        // Sauvegarde état texte
        if (hpText != null)
        {
            originalTextPos = hpText.rectTransform.localPosition;
            originalOutlineColor = hpText.outlineColor;
            originalOutlineWidth = hpText.outlineWidth;
        }
    }

    private void Update()
    {
        if (thing == null) return;

        int maxHealth = (int)thing.maxHealth;
        int health = (int)thing.health;

        if (slider.maxValue != maxHealth)
        {
            slider.maxValue = maxHealth;
        }

        // Détection dégâts
        if (health < lastHealth)
        {
            TriggerDamageEffects();
        }

        // Update slider
        if (smooth)
        {
            slider.value = Mathf.Lerp(slider.value, thing.health, Time.deltaTime * smoothSpeed);
        }
        else
        {
            slider.value = thing.health;
        }

        UpdateHPText();

        lastHealth = health;
    }

    private void UpdateHPText()
    {
        if (hpText != null && thing != null)
        {
            hpText.text = $"{(int)thing.health}";
        }
    }

    private void TriggerDamageEffects()
    {
        if (hpText != null)
        {
            StopAllCoroutines();
            StartCoroutine(TextShake());
            StartCoroutine(TextOutlineFlash());
        }

        if (fillImage != null)
        {
            StartCoroutine(FillFlash());
        }
    }

    IEnumerator TextShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 offset = Random.insideUnitSphere * shakeStrength;
            offset.z = 0;

            hpText.rectTransform.localPosition = originalTextPos + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        hpText.rectTransform.localPosition = originalTextPos;
    }

    IEnumerator TextOutlineFlash()
    {
        hpText.outlineWidth = 0.3f;
        hpText.outlineColor = damageColor;

        yield return new WaitForSeconds(effectDuration);

        hpText.outlineColor = originalOutlineColor;
        hpText.outlineWidth = originalOutlineWidth;
    }

    IEnumerator FillFlash()
    {
        Color originalColor = fillImage.color;

        fillImage.color = damageColor;

        yield return new WaitForSeconds(effectDuration);

        fillImage.color = originalColor;
    }
}