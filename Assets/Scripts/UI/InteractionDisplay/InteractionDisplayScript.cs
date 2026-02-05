using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractionDisplayScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private Slider progressBar;

    [Header("Text Effect")]
    [SerializeField] private float blinkSpeed = 2f;

    public Camera playerCamera;

    private basePlayer player;
    private Color baseTextColor;

    private void Start()
    {
        // Sécurité
        if (interactionText == null)
        {
            Debug.LogError("InteractionDisplayScript : InteractionText n'est pas assigné !");
            enabled = false;
            return;
        }

        if (progressBar == null)
        {
            Debug.LogError("InteractionDisplayScript : ProgressBar n'est pas assignée !");
            enabled = false;
            return;
        }

        interactionText.text = "";
        interactionText.enabled = false;

        progressBar.minValue = 0;
        progressBar.maxValue = 100;
        progressBar.value = 0;
        progressBar.gameObject.SetActive(false);

        baseTextColor = interactionText.color;

        player = InteractionManager.Instance.GetComponent<basePlayer>();
    }

    private void Update()
    {
        DetectInteractable();
        UpdateProgressBar();
        HandleTextBlink();
    }

    private void DetectInteractable()
    {
        Debug.LogWarning(player.stats.InteractionRange + " BBBBBBBBBBBBB");

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Debug.LogWarning(player.stats.InteractionRange + " AAAAAAAAAAAA");
        if (Physics.Raycast(ray, out hit, player.stats.InteractionRange))
        {
            GameObject hitObject = hit.transform.gameObject;

            BaseModule module = hitObject.GetComponent<BaseModule>();
            if (module != null && module.broken.Value)
            {
                ShowText("Maintenez [E] pour réparer");
                return;
            }
        }

        HideText();
    }

    private void UpdateProgressBar()
    {
        InteractionManager im = InteractionManager.Instance;

        if (!im.IsHoldingInteract || im.CurrentHoldDuration <= 0f)
        {
            progressBar.value = 0;
            progressBar.gameObject.SetActive(false);
            return;
        }

        float progress = (im.HoldTimer / im.CurrentHoldDuration) * 100f;
        progressBar.value = Mathf.Clamp(progress, 0f, 100f);
        progressBar.gameObject.SetActive(true);
    }

    private void HandleTextBlink()
    {
        if (!InteractionManager.Instance.IsHoldingInteract || !interactionText.enabled)
        {
            interactionText.color = baseTextColor;
            return;
        }

        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        interactionText.color = new Color(
            baseTextColor.r,
            baseTextColor.g,
            baseTextColor.b,
            alpha
        );
    }

    private void ShowText(string message)
    {
        interactionText.text = message;
        interactionText.enabled = true;
    }

    private void HideText()
    {
        interactionText.text = "";
        interactionText.enabled = false;

        progressBar.value = 0;
        progressBar.gameObject.SetActive(false);
    }
}
