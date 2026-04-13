using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IPPanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform panelContent;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject submitButton;
    [SerializeField] private TMP_InputField inputField;

    void Start()
    {
        ResolveReferences();

        if (inputField == null)
        {
            Debug.LogError("[IP Panel] Missing TMP_InputField reference. Assign it in inspector or place it on/under panel.");
            enabled = false;
            return;
        }

        // Hide the panel until the player clicks Join.
        HidePanel();

        // Clear stale error state whenever the user edits the input.
        inputField.onValueChanged.RemoveListener(OnInputValueChanged);
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void ResolveReferences()
    {
        if (inputField == null && panel != null)
            inputField = panel.GetComponentInChildren<TMP_InputField>(true);

        if (panelContent == null && inputField != null)
        {
            RectTransform current = inputField.GetComponent<RectTransform>();
            while (current != null && panel != null)
            {
                RectTransform parent = current.parent as RectTransform;
                if (parent == null)
                    break;

                if (parent.gameObject == panel)
                {
                    panelContent = current;
                    break;
                }

                current = parent;
            }
        }

        if (panelContent == null && panel != null)
            panelContent = panel.GetComponent<RectTransform>();

        if (panel == null && inputField != null)
            panel = inputField.gameObject;

        if (panel == null)
            panel = gameObject;

        if (closeButton != null)
        {
            Button closeButtonComponent = closeButton.GetComponent<Button>();
            if (closeButtonComponent != null)
            {
                closeButtonComponent.onClick.RemoveListener(ClosePanel);
                closeButtonComponent.onClick.AddListener(ClosePanel);
            }
        }

        if (submitButton != null)
        {
            Button submitButtonComponent = submitButton.GetComponent<Button>();
            if (submitButtonComponent != null)
            {
                submitButtonComponent.onClick.RemoveListener(SubmitCurrentIP);
                submitButtonComponent.onClick.AddListener(SubmitCurrentIP);
            }
        }
    }

    private void Update()
    {
        if (panel == null || !panel.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SubmitCurrentIP();
            return;
        }

        if (!Input.GetMouseButtonDown(0))
            return;

        if (panelContent != null && RectTransformUtility.RectangleContainsScreenPoint(panelContent, Input.mousePosition, null))
            return;

        if (panelContent != null && !RectTransformUtility.RectangleContainsScreenPoint(panelContent, Input.mousePosition, null))
            ClosePanel();
    }

    public void DisplayPanel()
    {
        if (panel != null)
            panel.SetActive(true);

        if (closeButton != null)
            closeButton.SetActive(true);

        if (errorPanel != null)
            errorPanel.SetActive(false);

        if (inputField != null)
            inputField.ActivateInputField();
    }

    private void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);

        if (closeButton != null)
            closeButton.SetActive(false);

        if (errorPanel != null)
            errorPanel.SetActive(false);
    }

    public void SubmitCurrentIP()
    {
        string ip = inputField != null ? inputField.text : string.Empty;
        TrySubmitIP(ip);
    }

    public void ClosePanel()
    {
        HidePanel();
    }

    private void OnInputValueChanged(string _)
    {
        if (errorPanel != null)
            errorPanel.SetActive(false);
    }

    private void TrySubmitIP(string ip)
    {
        string trimmedIp = ip.Trim();
        Debug.Log("[IP Panel] Submitted IP : " + trimmedIp);

        if (IsValidIP(trimmedIp))
        {
            Debug.Log("[IP Panel] IP is valid.");
            SetHostIP(trimmedIp);

            if (GameManager.instance == null)
            {
                Debug.LogError("[IP Panel] GameManager.instance is null. Cannot start client.");
                return;
            }

            GameManager.instance.StartClient();
            HidePanel();
            return;
        }
        else 
        {
            Debug.Log("[IP Panel] IP is NOT valid !");
            DisplayPanelError("The IP you provided is not a valid IPv4 IP.");
            return;
        }
    }

    private void SetHostIP(string ip)
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("[IP Panel] GameManager.instance is null. Cannot set host IP.");
            return;
        }

        GameManager.instance.SetHostIP(ip);
    }

    private void DisplayPanelError(string message)
    {
        if (errorPanel == null)
            return;

        errorPanel.SetActive(true);
        TMP_Text panelText = errorPanel.GetComponent<TMP_Text>();
        if (panelText == null)
            panelText = errorPanel.GetComponentInChildren<TMP_Text>(true);

        if (panelText != null)
            panelText.text = message;
    }

    static bool IsValidIP(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return false;

        int n = s.Length;

        if (n < 7)
            return false;

        // Using regex to match and separate 
        // all the strings from '.'
        var matches = Regex.Matches(
            s, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

        // If the number of matches 
        // is not 1, return false
        if (matches.Count != 1)
            return false;

        var ipString = matches[0].Value;

        // Split the IP string by '.' 
        // and store in an array
        string[] parts = ipString.Split('.');

        // If the array length != 4,
        // return false
        if (parts.Length != 4)
            return false;

        // Iterating over the 
        // array of strings
        for (int i = 0; i < parts.Length; i++)
        {
            string temp = parts[i];

            if (temp.Length > 1 && temp[0] == '0')
                return false;

            if (temp.Any(char.IsLetter))
                return false;

            // Check if the number is 
            // greater than 255
            if (int.Parse(temp) > 255)
                return false;
        }
        return true;
    }
}
