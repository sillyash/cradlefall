using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CapsuleDisplayScript : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Structures sérialisées — UI assignée dans l'Inspector
    // -------------------------------------------------------------------------

    [System.Serializable]
    public class CapsuleUIData
    {
        public TextMeshProUGUI label;
        public Image[] segments; 
    }

    [Header("Les 6 capsules UI (assignées dans l'Inspector)")]
    [SerializeField] private CapsuleUIData[] capsules = new CapsuleUIData[6];

    // -------------------------------------------------------------------------
    // Visuels
    // -------------------------------------------------------------------------

    [Header("Couleurs segments")]
    [SerializeField] private Color segmentActiveColor = Color.red;
    [SerializeField] private Color segmentInactiveColor = new Color(0.2f, 0.2f, 0.2f, 1f);

    [Header("Couleur TMP danger (≤ 2 segments)")]
    [SerializeField] private Color normalTMPColor = Color.white;
    [SerializeField] private Color dangerTMPColor = new Color(0.55f, 0f, 0f);

    [Header("Animation TMP danger")]
    [SerializeField] private float pulseScaleMax = 1.05f;
    [SerializeField] private float pulseSpeed = 1.05f;

    // -------------------------------------------------------------------------
    // Runtime
    // -------------------------------------------------------------------------

    private class CapsuleRuntime
    {
        public float lastHealth = float.MaxValue;
        public Coroutine hitFlash;
        public CryogenicBox box;
        public Vector3 baseScale;
        public Coroutine anim;
        public bool isAnimating;
        public NetworkVariable<float>.OnValueChangedDelegate onHealthChanged;
        public NetworkVariable<bool>.OnValueChangedDelegate onBrokenChanged;
    }

    private readonly CapsuleRuntime[] _rt = new CapsuleRuntime[6];
    private bool _boxesReady = false;

    // -------------------------------------------------------------------------
    // Lifecycle
    // -------------------------------------------------------------------------

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            _rt[i] = new CapsuleRuntime();
            if (capsules[i]?.label != null)
                _rt[i].baseScale = capsules[i].label.rectTransform.localScale;
        }

        CryogenicBox.OnBoxSpawned += _ => TryBindBoxes();

        TryBindBoxes();
    }

    private void OnDestroy()
    {
        CryogenicBox.OnBoxSpawned -= _ => TryBindBoxes();
        UnsubscribeAll();
    }

    // -------------------------------------------------------------------------
    // Récupération et binding des boxes
    // -------------------------------------------------------------------------

    private void TryBindBoxes()
    {
        var found = new List<CryogenicBox>(
            FindObjectsByType<CryogenicBox>(FindObjectsSortMode.None)
        );

        if (found.Count < 6)
        {
            Debug.Log($"[CryoDisplay] Seulement {found.Count}/6 boxes trouvées, on attend...");
            return;
        }

        // Tri par nom pour ordre stable : Capsule, Capsule (1) ... Capsule (4)
        found.Sort((a, b) => string.Compare(a.name, b.name, System.StringComparison.Ordinal));

        UnsubscribeAll();

        for (int i = 0; i < 6; i++)
        {
            _rt[i].box = found[i];
            Subscribe(i);
        }

        _boxesReady = true;
        Debug.Log("[CryoDisplay] 6 boxes bindées avec succès.");
        RefreshAll();
    }

    // -------------------------------------------------------------------------
    // Abonnements
    // -------------------------------------------------------------------------

    private void Subscribe(int i)
    {
        var box = _rt[i].box;
        if (box == null) return;

        int captured = i;
        _rt[i].onHealthChanged = (_, __) => RefreshOne(captured);
        _rt[i].onBrokenChanged = (_, __) => RefreshOne(captured);

        box.Health.OnValueChanged += _rt[i].onHealthChanged;
        box.Broken.OnValueChanged += _rt[i].onBrokenChanged;

        Debug.Log($"[CryoDisplay] Capsule[{i}] abonnée à '{box.name}'");
    }

    private void UnsubscribeAll()
    {
        for (int i = 0; i < 6; i++)
        {
            var box = _rt[i]?.box;
            var rt = _rt[i];
            if (box == null || rt == null) continue;

            if (rt.onHealthChanged != null)
            {
                box.Health.OnValueChanged -= rt.onHealthChanged;
                rt.onHealthChanged = null;
            }
            if (rt.onBrokenChanged != null)
            {
                box.Broken.OnValueChanged -= rt.onBrokenChanged;
                rt.onBrokenChanged = null;
            }
        }
    }

    // -------------------------------------------------------------------------
    // Refresh
    // -------------------------------------------------------------------------

    private void RefreshAll()
    {
        if (!_boxesReady) return;
        for (int i = 0; i < 6; i++)
            RefreshOne(i);
    }

    private void RefreshOne(int i)
    {
        var ui = capsules[i];
        var rt = _rt[i];

        if (ui == null || rt?.box == null) return;

        bool broken = rt.box.IsBroken();
        float health = rt.box.Gethealth();
        float percent = broken ? 0f : Mathf.Clamp01(health / rt.box.maxHealth);
        int activeSegs = broken ? 0 : Mathf.RoundToInt(percent * ui.segments.Length);

        // Flash hit si la vie baisse
        if (health < rt.lastHealth)
            StartHitFlash(i);
        rt.lastHealth = health;

        SetSegments(ui, activeSegs);

        bool danger = activeSegs <= 2;
        if (danger && !rt.isAnimating) StartDangerAnim(i);
        else if (!danger && rt.isAnimating) StopDangerAnim(i);
    }

    // -------------------------------------------------------------------------
    // Segments
    // -------------------------------------------------------------------------

    private void SetSegments(CapsuleUIData ui, int activeCount)
    {
        for (int s = 0; s < ui.segments.Length; s++)
            ui.segments[s].color = s < activeCount ? segmentActiveColor : segmentInactiveColor;
    }

    // -------------------------------------------------------------------------
    // Animation danger
    // -------------------------------------------------------------------------

    private void StartDangerAnim(int i)
    {
        if (_rt[i].anim != null) StopCoroutine(_rt[i].anim);
        _rt[i].isAnimating = true;
        _rt[i].anim = StartCoroutine(DangerPulse(i));
    }

    private void StopDangerAnim(int i)
    {
        _rt[i].isAnimating = false;
        if (_rt[i].anim != null) { StopCoroutine(_rt[i].anim); _rt[i].anim = null; }

        var label = capsules[i]?.label;
        if (label != null)
        {
            label.rectTransform.localScale = _rt[i].baseScale;
            label.color = normalTMPColor;
        }
    }

    private IEnumerator DangerPulse(int i)
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime;

            float scale = 1f + (pulseScaleMax - 1f) * (Mathf.Sin(t * pulseSpeed * Mathf.PI * 2f) * 0.5f + 0.5f);
            float pulse = Mathf.Sin(t * pulseSpeed * Mathf.PI * 2f) * 0.5f + 0.5f;

            var label = capsules[i]?.label;
            if (label != null)
            {
                label.rectTransform.localScale = _rt[i].baseScale * scale;
                label.color = Color.Lerp(dangerTMPColor, normalTMPColor, pulse);
            }

            yield return null;
        }
    }

    private void StartHitFlash(int i)
    {
        if (_rt[i].hitFlash != null) StopCoroutine(_rt[i].hitFlash);
        _rt[i].hitFlash = StartCoroutine(HitFlash(i));
    }

    private IEnumerator HitFlash(int i)
    {
        float duration = 0.4f;
        float t = 0f;

        while (t < duration)
        {
            // Ne pas interférer avec l'anim danger permanente
            if (!_rt[i].isAnimating)
            {
                t += Time.deltaTime;
                float pulse = Mathf.Sin((t / duration) * Mathf.PI); // 0→1→0

                float scale = 1f + (pulseScaleMax - 1f) * pulse;
                capsules[i].label.rectTransform.localScale = _rt[i].baseScale * scale;
                capsules[i].label.color = Color.Lerp(normalTMPColor, dangerTMPColor, pulse);
            }
            yield return null;
        }

        // Reset seulement si l'anim danger n'a pas pris le relais
        if (!_rt[i].isAnimating)
        {
            capsules[i].label.rectTransform.localScale = _rt[i].baseScale;
            capsules[i].label.color = normalTMPColor;
        }

        _rt[i].hitFlash = null;
    }
}