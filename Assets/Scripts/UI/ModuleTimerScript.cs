using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModuleTimerScript : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image timerImage;
    [SerializeField] private Image dangerIcon;
    [SerializeField] private Image safeIcon;

    [Header("Critical Timer Settings")]
    [SerializeField] private float criticalTimeThreshold = 11f;
    [SerializeField] private Color criticalColor = new Color(0.6f, 0f, 0f);

    [Header("Danger Icon Animation")]
    [SerializeField, Range(0f, 1f)] private float dangerPulseIntensity = 0.25f;
    [SerializeField] private float dangerPulseSpeed = 2.5f;

    [Header("Timer Text Animation")]
    [SerializeField, Range(0f, 0.5f)] private float textPulseIntensity = 0.01f;
    [SerializeField] private float textPulseSpeed = 4f;

    private BaseModule targetModule;
    private float maxTime;

    private Vector3 originalDangerScale;
    private Vector3 originalTextScale;
    private Vector3 originalTimerScale;

    void Awake()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
            originalTextScale = timerText.rectTransform.localScale;
        }

        if (timerImage != null)
        {
            timerImage.gameObject.SetActive(true);
            originalTimerScale = timerImage.rectTransform.localScale;
        }

        if (dangerIcon != null)
        {
            dangerIcon.gameObject.SetActive(false);
            originalDangerScale = dangerIcon.rectTransform.localScale;
        }

        if (safeIcon != null)
            safeIcon.gameObject.SetActive(true);
    }

    void Start()
    {
        foreach (var m in FindObjectsOfType<BaseModule>())
        {
            if (m.gameObject.name == "OxygenModule")
            {
                targetModule = m;
                break;
            }
        }

        if (targetModule != null && targetModule.stats is TimerModuleStats s)
            maxTime = s.timerDuration;
    }

    void Update()
    {
        if (targetModule == null) { SetSafeState(); return; }

        var timerBehavior = targetModule.behaviour as TimerModuleBehavior;
        if (timerBehavior == null) { SetSafeState(); return; }

        if (maxTime == 0f && targetModule.stats is TimerModuleStats s)
            maxTime = s.timerDuration;

        bool isBroken = timerBehavior.timerRunning;
        float t = timerBehavior.remainingTime;

        // ===== SAFE / DANGER STATE =====
        if (safeIcon != null)
            safeIcon.gameObject.SetActive(!isBroken);

        if (dangerIcon != null)
            dangerIcon.gameObject.SetActive(isBroken);

        // ===== TIMER TEXT =====
        if (timerText != null)
        {
            timerText.gameObject.SetActive(isBroken);

            if (isBroken)
            {
                int minutes = Mathf.FloorToInt(t / 60f);
                int seconds = Mathf.FloorToInt(t % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }

        // ===== TIMER IMAGE (fill) =====
        if (timerImage != null && maxTime > 0f)
        {
            timerImage.fillAmount = t / maxTime;
            timerImage.rectTransform.localScale = originalTimerScale;
        }

        // ===== DANGER ICON ANIMATION =====
        if (dangerIcon != null && isBroken)
        {
            float pulse = 1f + Mathf.Sin(Time.time * dangerPulseSpeed) * dangerPulseIntensity;
            dangerIcon.rectTransform.localScale = Vector3.Lerp(
                dangerIcon.rectTransform.localScale,
                originalDangerScale * pulse,
                Time.deltaTime * 5f
            );
        }
        else if (dangerIcon != null)
        {
            dangerIcon.rectTransform.localScale = originalDangerScale;
        }

        // ===== CRITICAL TEXT EFFECT =====
        if (timerText != null && isBroken)
        {
            if (t <= criticalTimeThreshold)
            {
                float pulse = 1f + Mathf.Sin(Time.time * textPulseSpeed) * textPulseIntensity;
                timerText.rectTransform.localScale = originalTextScale * pulse;

                float lerp = 1f - (t / criticalTimeThreshold);
                timerText.color = Color.Lerp(Color.white, criticalColor, lerp);
            }
            else
            {
                timerText.rectTransform.localScale = originalTextScale;
                timerText.color = Color.white;
            }
        }
    }

    private void SetSafeState()
    {
        if (timerText != null) timerText.gameObject.SetActive(false);
        if (dangerIcon != null) dangerIcon.gameObject.SetActive(false);
        if (safeIcon != null) safeIcon.gameObject.SetActive(true);
        if (timerImage != null) timerImage.rectTransform.localScale = originalTimerScale;
    }
}