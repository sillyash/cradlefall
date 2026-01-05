using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MultiScreenScript : MonoBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;

    private void Awake()
    {
        HostButton.onClick.AddListener(() =>
        {
            Debug.Log("HOST");
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene(
                "MainGame",
                LoadSceneMode.Single
            );
        });

        ClientButton.onClick.AddListener(() =>
        {
            Debug.Log("CLIENT");
            NetworkManager.Singleton.StartClient();
        });
    }
}
