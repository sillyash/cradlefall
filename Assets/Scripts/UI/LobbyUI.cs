using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI controller for the lobby scene
/// </summary>
public class LobbyUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI readyButtonText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI playerCountText;

    private bool isLocalPlayerReady = false;

    private void Start()
    {
        if (readyButton != null)
        {
            readyButton.onClick.AddListener(OnReadyButtonClicked);
        }

        UpdateUI();
    }

    private void Update()
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
        LobbyManager.Instance.SetLocalReady(isLocalPlayerReady);

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
            playerCountText.text = $"Players: {LobbyManager.Instance.ConnectedCount}\nReady: {LobbyManager.Instance.ReadyCount}";
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
