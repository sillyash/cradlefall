using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class EnnemiesRemainingScript : MonoBehaviour
{
    public WaveSpawner spawner;
    public TextMeshProUGUI enemiesText;
    public Image enemyIcon;

    [Header("Animation settings")]
    public Color flashColor = new Color(0.8f, 0.05f, 0.05f, 0.5f); // bleu très clair, semi-transparent
    public float flashDuration = 0.25f;
    public float scalePunch = 1.15f; // léger effet de “pulsation”

    private int lastEnemiesLeft = -1;
    private Color iconOriginalColor;
    private Color textOriginalColor;
    private Vector3 textOriginalScale;
    private Vector3 iconOriginalScale;

    private void Awake()
    {
        if (spawner == null)
            spawner = Object.FindFirstObjectByType<WaveSpawner>();

        iconOriginalColor = enemyIcon.color;
        textOriginalColor = enemiesText.color;

        textOriginalScale = enemiesText.transform.localScale;
        iconOriginalScale = enemyIcon.transform.localScale;
    }

    void Update()
    {
        if (spawner.CurrentWaveNumber >= spawner.waves.Length)
        {
            enemiesText.text = "0";
            return;
        }

        var currentWave = spawner.waves[spawner.CurrentWaveNumber];
        int enemiesLeft = currentWave.enemiesLeft;

        if (enemiesLeft != lastEnemiesLeft)
        {
            enemiesText.text = enemiesLeft.ToString();

            if (lastEnemiesLeft != -1 && enemiesLeft < lastEnemiesLeft)
            {
                // Lancement de l'animation subtile
                StartCoroutine(FlashAndPulse(enemyIcon, iconOriginalColor, flashColor, iconOriginalScale));
                StartCoroutine(FlashAndPulse(enemiesText, textOriginalColor, flashColor, textOriginalScale));
            }

            lastEnemiesLeft = enemiesLeft;
        }
    }

    private IEnumerator FlashAndPulse(Graphic graphic, Color originalColor, Color flashColor, Vector3 originalScale)
    {
        // Couleur + scale rapide
        graphic.color = flashColor;
        graphic.transform.localScale = originalScale * scalePunch;

        yield return new WaitForSeconds(flashDuration);

        // Retour à l'état normal
        graphic.color = originalColor;
        graphic.transform.localScale = originalScale;
    }
}
