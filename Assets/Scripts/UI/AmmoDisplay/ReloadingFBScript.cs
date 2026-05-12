using UnityEngine;
using System.Collections;

public class ReloadingFBScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Image reloadCircle; // L'image "donut" fillable
    [SerializeField] private basePlayer player;
    [SerializeField] private GameObject cursor;

    private BaseWeapon weapon;
    private Coroutine currentCoroutine;

    private void Start()
    {
        if (reloadCircle == null)
        {
            reloadCircle = GetComponent<UnityEngine.UI.Image>();
            if (reloadCircle == null)
            {
                Debug.LogError("ReloadFBScript : aucune Image assignée !");
                return;
            }
        }

        if (player == null)
        {
            Debug.LogError("ReloadFBScript : aucun Player assigné !");
            return;
        }

        weapon = player.CurrentWeapon;
        if (weapon == null)
        {
            Debug.LogWarning("ReloadFBScript : le Player n'a pas d'arme équipée !");
            return;
        }

        // S'abonner aux événements de l'arme
        weapon.OnReloadStart += StartReloadFeedback;
        weapon.OnReloadEnd += StopReloadFeedback;

        // Commencer vide
        reloadCircle.fillAmount = 0f;
    }

    private void StartReloadFeedback()
    {
        if (weapon == null) return;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        cursor.SetActive(false);
        currentCoroutine = StartCoroutine(FillReloadCoroutine(weapon.ReloadTime));
    }

    private void StopReloadFeedback()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        reloadCircle.fillAmount = 0f;
        cursor.SetActive(true);
    }

    private IEnumerator FillReloadCoroutine(float duration)
    {
        float elapsed = 0f;
        reloadCircle.fillAmount = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            reloadCircle.fillAmount = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        reloadCircle.fillAmount = 1f;
        currentCoroutine = null;
    }
}