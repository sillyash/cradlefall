using Unity.Netcode;
using UnityEngine;

public class BaseModule : NetworkBehaviour
{
    [Header("Sound FX Manager")]
    [SerializeField] private SoundFXManager soundFXManager;
    [Header("Module Sounds")]
    [SerializeField] private AudioClip repairedSound;
    [SerializeField] [Range(0f, 1f)] private float repairedSoundVolume = 1f;
    [SerializeField] private AudioClip repairSound;
    [SerializeField] [Range(0f, 1f)] private float repairSoundVolume = 1f;
    [SerializeField] private AudioClip damagedSound;
    [SerializeField] [Range(0f, 1f)] private float damagedSoundVolume = 1f;
    [SerializeField] private AudioClip damagedSound2;
    [SerializeField] [Range(0f, 1f)] private float damagedSound2Volume = 1f;
    [SerializeField] private AudioClip activationSound;
    [SerializeField] [Range(0f, 1f)] private float activationSoundVolume = 1f;

    // AudioSource dédié pour la réparation
    private AudioSource repairAudioSource;

    public ModuleBehavior behaviour;
    public ModuleStats stats;

    // NetworkVariable pour afficher le temps restant sur le timer module
    public NetworkVariable<float> remainingTime = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    // NetworkVariables, wrapper de variables natives, synchronis�es automatiquement par NetCode
    // On acc�de � leur valeur par variable.Value pour toutes les op�rations.
    // Peuvent �tre initialis�e � la cr�ation par le constructeur (recommand�)
    //
    // Permet une gestion des permissions Read / Write par Owner ou Client / Server
    // ex: NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    //
    // Hook possible sur OnValueChange pour s'abonner aux changement de valeur
    // Abonnement = health.OnValueChanged += foo
    //
    // /!\ Un joueur qui arrive en cours de partie, sera automatiquement sync aux nouvelles valeurs
    public NetworkVariable<float> health = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> broken = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // OnNetworkSpawn remplace strictement les fonctions Start et Awake qui ne font plus sens en multi.
    // /!\ Ne s'ex�cute qu'a l'apparition de l'objet sur le Network.
    // Potentiellement, les joueurs qui arrivent en cours d epartie ne sont pas encore l�.
    public override void OnNetworkSpawn()
    {
        // Prépare l'AudioSource pour la réparation
        if (repairAudioSource == null)
        {
            repairAudioSource = gameObject.AddComponent<AudioSource>();
            repairAudioSource.playOnAwake = false;
            repairAudioSource.loop = true;
            repairAudioSource.spatialBlend = 1f; // 3D par défaut, ajuste si besoin
        }
        // Chaque client et le serveur ex�cute tous les codes
        // Ainsi, tous les clients vont essayer de run cette fonction aussi. Il n'est pas utile et m�me dangereux de laisser tout le faire
        // health.Value = stats.health; peut potentiellement cr�er des d�synchro et ne doit �tre �crite que par le serveur.
        // Alors, on ignore tous ceux qui ne sont pas le serveur. Ainsi, la suite du code ne se fait que sur Server.
        //
        // /!\ A ne pas simplement d�sactiver le script avec enabled = false, car les clients ont besoin des ServerRpc
        if (!IsServer) return;

        if (!stats)
        {
            enabled = false;
            Debug.LogError($"module = {this.name} n'a pas de stats");
            return;
        }
        if (!behaviour)
        {
            enabled = false;
            Debug.LogError($"module = {this.name} n'a pas de behaviour");
            return;
        }

        // Automatiquement sync
        health.Value = stats.health;
        behaviour.OnStart(this);
    }

    public void Update()
    {
        // Meme logique que OnNetworkSpawn. Le code ne doit petre ex�cuter que sur Serveur.
        if (!IsServer) return;

        float chancePerFrame = Time.deltaTime / stats.breakChance;
        if (!broken.Value && Random.value < chancePerFrame)
        {
            Break();
            Debug.LogError($"The module IS BROKEN");
        }
    }

    // Une ServerRPC est du code APPEL� par un CLIENT et EX�CUT�E sur le SERVEUR
    // C'est � voir comme une demande aupr�s du client pour ex�cuter du code
    // C'est ici que le serveur doit analyser la demande et r�agir en fonction. 
    // Souvent le serveur doit syncro cette demande envers tous les clients.
    // Ainsi, on verra souvent des ClientRPC appel�es ici et des NetworkVariable m�j.
    //
    // /!\ LES CLIENTS NE DOIVENT PAS (ou quasiment jamais) EXECUTER DE LOGIQUE QUI PEUT ETRE SYNC
    // par exemple, ici, on met le module a broken uniquement sur le serveur 
    // et on �xecute la logique de casse ici. Le serveur change les variables puis les envoie aux clients
    // Il effectue les traitements puis synchronise.
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void TakeDamageServerRpc(float damage)
    {
        if (broken.Value) return;
        health.Value -= damage;
        if (health.Value <= 0)
        {
            Break();
        }
    }

    private void Break()
    {
        broken.Value = true;
        behaviour.OnBreak(this);
        Debug.LogWarning("The module is broken " + gameObject.name);
        if (soundFXManager != null) {
            if (damagedSound != null)
                soundFXManager.PlaySoundFXClip(damagedSound, transform, damagedSoundVolume);
            if (damagedSound2 != null)
                soundFXManager.PlaySoundFXClip(damagedSound2, transform, damagedSound2Volume);
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void RepairServerRpc()
    {
        if (!broken.Value) return;
        Debug.Log("Module repaired ! " + gameObject.name);
        broken.Value = false;
        health.Value = stats.health;
        behaviour.OnRepair(this);
        if (repairSound != null && soundFXManager != null)
        {
            soundFXManager.PlaySoundFXClip(repairedSound, transform, repairedSoundVolume);
        }
    }

    // Appeler cette méthode lors d'une activation de module (pas utilisée pour le moment)
    public void PlayActivationSound()
    {
        if (activationSound != null && soundFXManager != null)
        {
            soundFXManager.PlaySoundFXClip(activationSound, transform, activationSoundVolume);
        }
    }

        public void PlayRepairStartSound()
        {
            if (repairSound != null)
            {
                if (repairAudioSource == null)
                {
                    repairAudioSource = gameObject.AddComponent<AudioSource>();
                    repairAudioSource.playOnAwake = false;
                    repairAudioSource.loop = true;
                    repairAudioSource.spatialBlend = 1f;
                }
                repairAudioSource.clip = repairSound;
                repairAudioSource.volume = repairSoundVolume;
                if (!repairAudioSource.isPlaying)
                    repairAudioSource.Play();
            }
        }

        public void StopRepairSound()
        {
            if (repairAudioSource != null && repairAudioSource.isPlaying)
            {
                repairAudioSource.Stop();
            }
        }
}
// Note, ici les clients n'ex�cute aucun code dans ce script. Ils font des demandes au serveur. La logique module est purement SERVER SIDE