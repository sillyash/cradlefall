using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Constants;
using System;

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

    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] [Range(0f,1f)] private float buttonClickVolume = 1f;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] [Range(0f,1f)] private float buttonHoverVolume = 1f;

    private int roleIndex = 0;
    private List<string> roles;
    private int nbOfReadyClient;

    public void Awake()
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

        if (LobbyManager.Instance != null)
        {
            LobbyManager.Instance.readyClients.OnListChanged += OnReadyListChanged;
        }        
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.NbOfActiveClients.OnValueChanged += OnPlayerCountChange;
        }
    }

    private void Update()
    {
        // This is a fallback for client if update event is late
        if (LobbyManager.Instance == null || !LobbyManager.Instance.IsSpawned)
            return;

        int newValue = LobbyManager.Instance.readyClients.Count;
        if (nbOfReadyClient != newValue)
            UpdateUI();

        nbOfReadyClient = newValue;
    }

    public void OnDestroy()
    {
        if (LobbyManager.Instance != null)
        {
            LobbyManager.Instance.readyClients.OnListChanged -= OnReadyListChanged;
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.NbOfActiveClients.OnValueChanged -= OnPlayerCountChange;
        }
    }

    private void OnReadyListChanged(NetworkListEvent<ulong> changeEvent)
    {
        UpdateUI();
    }

    private void OnPlayerCountChange(int previousValue, int newValue)
    {
        UpdateUI();
    }

    private void PlayButtonSound()
    {
        if (buttonClickSound != null && SoundFXManager.instance != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(buttonClickSound, transform, buttonClickVolume);
        }
    }

    public void OnButtonHover()
    {
        if (buttonHoverSound != null && SoundFXManager.instance != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(buttonHoverSound, transform, buttonHoverVolume);
        }
    }

    private void OnReadyButtonClicked()
    {
        PlayButtonSound();
        if (LobbyManager.Instance == null) return;

        LobbyManager.Instance.SetLocalReady(roleIndex);
    }

    private void OnFlecheGaucheClicked()
    {
        PlayButtonSound();
        if (LobbyManager.Instance.IsReady(NetworkManager.Singleton.LocalClientId)) return;
        roleIndex = (roleIndex - 1 + roles.Count) % roles.Count;
        RoleText.text = roles[roleIndex];
    }

    private void OnFlecheDroiteClicked()
    {
        PlayButtonSound();
        if (LobbyManager.Instance.IsReady(NetworkManager.Singleton.LocalClientId)) return;
        roleIndex = (roleIndex + 1) % roles.Count;
        RoleText.text = roles[roleIndex];
    }

    private void UpdateUI()
    {
        Debug.Log("UPDATE");
        if (LobbyManager.Instance == null)
        {
            if (statusText != null)
                statusText.text = "Waiting for lobby...";
            return;
        }

        // Update ready button
        if (readyButton != null && readyButtonText != null)
        {
            readyButtonText.text = (LobbyManager.Instance.IsReady(NetworkManager.Singleton.LocalClientId)) ? "Not Ready" : "Ready";
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
}
