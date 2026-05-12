using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTagInputHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string placeholderText = "Player Tag";
    [SerializeField] private int maxLength = 16;
    [SerializeField] private string defaultPrefix = "Player";

    [SerializeField] public TMP_InputField InputField => inputField;

    private void Awake()
    {
        ConfigureInputField();
    }

    public string GetPlayerTagOrFallback(ulong localClientId)
    {
        string sanitized = Sanitize(inputField != null ? inputField.text : string.Empty);
        if (!string.IsNullOrWhiteSpace(sanitized))
            return sanitized;

        return localClientId > 0 ? $"{defaultPrefix}{localClientId}" : defaultPrefix;
    }

    public void SetInteractable(bool interactable)
    {
        if (inputField != null)
            inputField.interactable = interactable;
    }

    private void ConfigureInputField()
    {
        if (inputField == null)
            return;

        inputField.lineType = TMP_InputField.LineType.SingleLine;
        inputField.characterLimit = Mathf.Max(1, maxLength);
        inputField.contentType = TMP_InputField.ContentType.Standard;

        if (inputField.placeholder is TMP_Text placeholder)
            placeholder.text = placeholderText;

        inputField.onValueChanged.RemoveListener(OnInputChanged);
        inputField.onValueChanged.AddListener(OnInputChanged);

        string sanitized = Sanitize(inputField.text);
        if (sanitized != inputField.text)
            inputField.SetTextWithoutNotify(sanitized);
    }

    private void OnInputChanged(string currentValue)
    {
        if (inputField == null)
            return;

        string sanitized = Sanitize(currentValue);
        if (sanitized == currentValue)
            return;

        int caretPosition = inputField.caretPosition;
        inputField.SetTextWithoutNotify(sanitized);
        inputField.caretPosition = Mathf.Clamp(caretPosition - 1, 0, sanitized.Length);
    }

    private string Sanitize(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return string.Empty;

        StringBuilder builder = new StringBuilder(raw.Length);
        for (int i = 0; i < raw.Length && builder.Length < maxLength; i++)
        {
            char c = raw[i];
            if (char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == ' ')
                builder.Append(c);
        }

        return builder.ToString().Trim();
    }
}
