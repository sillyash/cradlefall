using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Constants;

public class MultiScreenScript : MonoBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;

    private void Awake()
    {
        HostButton.onClick.AddListener(() =>
        {
            GameManager.instance.HostGame();
        });

        ClientButton.onClick.AddListener(() =>
        {
            GameManager.instance.StartClient();
        });
    }
}
