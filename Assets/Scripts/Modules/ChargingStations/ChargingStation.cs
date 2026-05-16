using Unity.Netcode;
using UnityEngine;

public class ChargingStation : NetworkBehaviour
{
    [SerializeField] private float batteryPerSeconde = 3;
    public NetworkVariable<bool> Broken = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [Header("Audio")]
    [SerializeField] private AudioClip chargingSound;
    [SerializeField] [Range(0f, 1f)] private float chargingSoundVolume = 1f;

    // Compteur de joueurs présents dans le trigger (géré côté serveur)
    private int playersInRange = 0;

    // AudioSource local utilisé par les clients pour boucler le son de charge
    private AudioSource chargeAudioSource;
    void OnTriggerStay(Collider other)
    {
        if (!IsServer) return;
        if (Broken.Value) return;

        if (other.TryGetComponent(out basePlayer player))
        {
            player.battery.Recharge(batteryPerSeconde * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (Broken.Value) return;

        if (other.TryGetComponent(out basePlayer player))
        {
            playersInRange++;
            if (playersInRange == 1)
            {
                PlayChargeSoundClientRpc();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;
        if (other.TryGetComponent(out basePlayer player))
        {
            playersInRange = Mathf.Max(0, playersInRange - 1);
            if (playersInRange == 0)
            {
                StopChargeSoundClientRpc();
            }
        }
    }

    [ClientRpc]
    private void PlayChargeSoundClientRpc()
    {
        if (chargingSound == null) return;
        if (chargeAudioSource == null)
        {
            chargeAudioSource = gameObject.AddComponent<AudioSource>();
            chargeAudioSource.playOnAwake = false;
            chargeAudioSource.loop = true;
            chargeAudioSource.spatialBlend = 1f;
        }
        chargeAudioSource.clip = chargingSound;
        chargeAudioSource.volume = chargingSoundVolume;
        if (!chargeAudioSource.isPlaying)
            chargeAudioSource.Play();
    }

    [ClientRpc]
    private void StopChargeSoundClientRpc()
    {
        if (chargeAudioSource != null && chargeAudioSource.isPlaying)
        {
            chargeAudioSource.Stop();
        }
    }
}
