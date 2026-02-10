using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class NetworkBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);        
    }
}
