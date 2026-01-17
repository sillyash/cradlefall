using UnityEngine;
using TMPro;

public class WaveTimerDisplay : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI timerText;
    public WaveSpawner spawner;

    private Color originalColor;
    private Color startColor = new Color(0.89f, 0.67f, 0.14f); // orange vif
    private Color endColor = new Color(0.63f, 0.12f, 0.01f);   // rouge foncé
    private Color shadowColor = new Color(0.6f, 0f, 0f, 0f);   // initialement transparent

    private void Awake()
    {
        if (timerText != null)
        {
            originalColor = timerText.color;
            timerText.enableVertexGradient = false;
        }

        if (spawner == null)
            spawner = Object.FindFirstObjectByType<WaveSpawner>();

        if (spawner != null)
        {
            spawner.WaveFinished += OnWaveFinished;
            spawner.WaveStarted += OnWaveStarted;
        }
    }

    private void Update()
    {
        if (spawner == null || timerText == null) return;

        float currentTimer = spawner.WaveTimer;
        showTimer(currentTimer);

        // --- Dégradé couleur principale + ombre ---
        if (currentTimer <= 6f && currentTimer > 1f)
        {
            float t = currentTimer / 6f; // 6 → 0
            timerText.color = Color.Lerp(endColor, startColor, t);

            // Ombre rouge progressive
            float shadowAlpha = Mathf.Clamp01(1f - t); // 0 → 1
            timerText.outlineColor = new Color(0.6f, 0f, 0f, shadowAlpha);
            timerText.outlineWidth = 0.2f + shadowAlpha * 0.3f; // augmente légèrement la largeur
        }
        // --- Dernière seconde : fade out + ombre max ---
        else if (currentTimer <= 1f && currentTimer > 0f)
        {
            timerText.color = endColor;
            timerText.alpha = currentTimer; // opacité diminue
            timerText.outlineColor = new Color(0.6f, 0f, 0f, 1f); // ombre max
            timerText.outlineWidth = 0.5f;
        }
        else if (currentTimer <= 0f)
        {
            timerText.gameObject.SetActive(false);
            timerText.color = originalColor;
            timerText.alpha = 1f;
            timerText.outlineColor = new Color(0f, 0f, 0f, 0f);
            timerText.outlineWidth = 0f;
        }
        else
        {
            timerText.color = originalColor;
            timerText.alpha = 1f;
            timerText.outlineColor = new Color(0f, 0f, 0f, 0f);
            timerText.outlineWidth = 0f;
        }
    }

    private void showTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.gameObject.SetActive(true);
    }

    private void OnWaveStarted()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    private void OnWaveFinished()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            timerText.alpha = 1f;
            timerText.outlineColor = new Color(0f, 0f, 0f, 0f);
            timerText.outlineWidth = 0f;
        }
    }
}
