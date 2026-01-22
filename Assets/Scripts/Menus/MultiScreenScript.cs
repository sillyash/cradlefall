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
            NetworkManager.Singleton.StartHost();
            GameManager.instance.ChangeScene(Scenes.MainGameMulti);
        });

        ClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
