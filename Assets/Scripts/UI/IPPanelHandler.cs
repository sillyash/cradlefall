using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using static TMPro.TMP_InputField;

public class IPPanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject closeButton;
    private TMP_InputField inputField;
    private SubmitEvent submitEvent;

    void Start()
    {
        // Retrieve input field
        inputField = panel.GetComponent<TMP_InputField>();

        // Hide error panel
        errorPanel.SetActive(false);

        // Add event on submit
        submitEvent = new SubmitEvent();
        submitEvent.AddListener(SubmitIP);
        inputField.onEndEdit = submitEvent;
    }

    public void DisplayPanel()
    {
        if (panel != null)
            panel.SetActive(true);

        if (closeButton != null)
            closeButton.SetActive(true);

        if (errorPanel != null)
            errorPanel.SetActive(false);
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

    private void SubmitIP(string arg0)
    {
        Debug.Log("[IP Panel] Submitted IP : " + arg0);
        if (IsValidIP(arg0))
        {
            Debug.Log("[IP Panel] IP is valid.");
            SetHostIP(arg0);
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
