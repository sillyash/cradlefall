using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaveDisplay : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI waveText;
    public WaveSpawner spawner; 

    [Header("Animation Settings")]
    public float appearScale = 1.5f;
    public float appearDuration = 0.5f;
    public float bounceScale = 1.2f;
    public float shrinkScale = 1f;
    public float shrinkDuration = 0.5f;

    [Header("Color Settings")]
    public Color startColor = Color.yellow;
    public Color endColor = new Color(0.5f, 0, 0); // rouge foncé
    public int maxWaves = 30;

    [Header("Font Size Settings")]
    public float startFontSize = 25f;
    public float maxFontSize = 40f;
    public int fontStepWaves = 5; // augmentation taille toutes les 5 vagues

    [Header("Bold / Outline Settings")]
    public bool useBoldInsteadOfOutline = true;
    public int outlineStepWaves = 10;
    public float startOutline = 0.05f;
    public float maxOutline = 0.3f;

    void Start()
    {
        if (waveText != null)
        {
            waveText.text = "VAGUE 0";
            waveText.color = startColor;
            waveText.fontSize = startFontSize;
            waveText.fontStyle = FontStyles.Normal;
            waveText.outlineWidth = startOutline;
            waveText.transform.localScale = Vector3.one * shrinkScale;
        }
    }

   

    void Update()
    {
        if (spawner == null) return;

        // Utilise soit le compteur du spawner, soit le debug si il a été incrémenté
        int currentWave = spawner.CurrentWaveNumber;

        ShowWave(currentWave);
    }


    private int lastWaveIndex = -1; // pour savoir quand une nouvelle vague arrive
    private Tween currentTween;
    private Tween permanentShake; // shake permanent pour vague finale

    private void ShowWave(int index)
    {
        if (waveText == null) return;

        waveText.text = "VAGUE " + index;

        // -------------------------------
        // 1️⃣ Couleur crescendo
        waveText.color = Color.Lerp(startColor, endColor, Mathf.Clamp01((float)index / maxWaves));

        // -------------------------------
        // 2️⃣ Taille par step toutes les 5 vagues
        int steps = index / fontStepWaves;
        int totalSteps = maxWaves / fontStepWaves;
        float incrementPerStep = (maxFontSize - startFontSize) / totalSteps;
        waveText.fontSize = Mathf.Min(startFontSize + steps * incrementPerStep, maxFontSize);

        // -------------------------------
        // 3️⃣ Bold ou Outline toutes les 10 vagues
        if (useBoldInsteadOfOutline && index % outlineStepWaves == 0 && index != 0)
            waveText.fontStyle = FontStyles.Bold;
        else if (!useBoldInsteadOfOutline && index % outlineStepWaves == 0 && index != 0)
            waveText.outlineWidth = Mathf.Lerp(startOutline, maxOutline, (float)index / maxWaves);

        // -------------------------------
        // 4️⃣ Bordure noire progressive pour dernières vagues 25 à 30
        if (index >= 25 && index <= 30)
        {
            float borderProgress = (float)(index - 25 + 1) / 5f; // 0.2 → 1
            waveText.outlineColor = Color.black;
            waveText.outlineWidth = Mathf.Lerp(0.05f, 0.25f, borderProgress);
        }

        // -------------------------------
        // 5️⃣ Animations (une seule fois par nouvelle vague)
        if (index != lastWaveIndex)
        {
            lastWaveIndex = index;

            // Kill previous tweens
            if (currentTween != null && currentTween.IsActive()) currentTween.Kill();
            if (permanentShake != null && permanentShake.IsActive()) permanentShake.Kill();

            // Scale animation smooth : 0 -> appearScale -> shrinkScale (taille finale)
            waveText.transform.localScale = Vector3.zero;
            currentTween = DOTween.Sequence()
                .Append(waveText.transform.DOScale(appearScale, appearDuration * 1.5f).SetEase(Ease.OutBack))
                .Append(waveText.transform.DOScale(shrinkScale, shrinkDuration * 1.5f).SetEase(Ease.InOutSine))
                .Play();

            // Shake léger permanent pour la dernière vague
            if (index == maxWaves)
            {
                permanentShake = waveText.transform.DOShakePosition(1f, 2f, 10, 90f, true, true).SetLoops(-1, LoopType.Restart);
            }
        }
        else
        {
            // Assure que le TMP reste visible et à scale normal si aucune nouvelle vague
            waveText.transform.localScale = shrinkScale * Vector3.one;
        }
    }




}
