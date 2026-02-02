using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Constants;

/// <summary>
/// UI controller for the lobby scene
/// </summary>
public class LobbyUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button readyButton;
    [SerializeField] private Button flecheGauche;
    [SerializeField] private Button flecheDroite;
    [SerializeField] private TextMeshProUGUI readyButtonText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI RoleText;

    private bool isLocalPlayerReady = false;
    private int roleIndex = 0;
    private List<string> roles;

    private void Awake()
    {
        if (readyButton != null)
        {
            readyButton.onClick.AddListener(OnReadyButtonClicked);
        }
        if (flecheGauche != null)
        {
            flecheGauche.onClick.AddListener(OnFlecheGaucheClicked);
        }
        if (flecheDroite != null)
        {
            flecheDroite.onClick.AddListener(OnFlecheDroiteClicked);
        }

        roles = Roles.roles;
        RoleText.text = roles[roleIndex];
        Debug.Log(roles[roleIndex]);

        UpdateUI();
    }

    public void Update()
    {
        // Update UI every frame to reflect lobby state
        if (LobbyManager.Instance != null)
        {
            UpdateUI();
        }
    }

    private void OnReadyButtonClicked()
    {
        if (LobbyManager.Instance == null) return;

        isLocalPlayerReady = !isLocalPlayerReady;
        LobbyManager.Instance.SetLocalReady(isLocalPlayerReady, roleIndex);

        UpdateUI();
    }

    private void OnFlecheGaucheClicked()
    {
        roleIndex--;
        if (roleIndex < 0) roleIndex = roles.Count - 1;
        RoleText.text = roles[roleIndex];
        UpdateUI();
    }

    private void OnFlecheDroiteClicked()
    {
        roleIndex++;
        if (roleIndex >= roles.Count) roleIndex = 0;
        RoleText.text = roles[roleIndex];
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (LobbyManager.Instance == null)
        {
            if (statusText != null)
                statusText.text = "Waiting for lobby...";
            return;
        }

        // Update ready button
        if (readyButton != null && readyButtonText != null)
        {
            readyButtonText.text = isLocalPlayerReady ? "Not Ready" : "Ready";
            readyButton.interactable = NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient;
        }

        // Update player count
        if (playerCountText != null)
        {
            playerCountText.text = $"Players: {PlayerManager.Instance?.NbOfActiveClients.Value}\nReady: {LobbyManager.Instance.ReadyCount}";
        }

        // Update status
        if (statusText != null)
        {
            if (LobbyManager.Instance.AllReady)
            {
                statusText.text = "Starting game...";
            }
            else
            {
                statusText.text = $"Waiting for players to ready up...";
            }
        }
    }

    private void OnDestroy()
    {
        if (readyButton != null)
        {
            readyButton.onClick.RemoveListener(OnReadyButtonClicked);
        }
    }
}
