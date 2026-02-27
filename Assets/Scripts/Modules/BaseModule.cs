using Unity.Netcode;
using UnityEngine;

public class BaseModule : NetworkBehaviour
{
    public ModuleBehavior behaviour;
    public ModuleStats stats;

    // NetworkVariables, wrapper de variables natives, synchronisées automatiquement par NetCode
    // On accède à leur valeur par variable.Value pour toutes les opérations.
    // Peuvent être initialisée à la création par le constructeur (recommandé)
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
    // /!\ Ne s'exécute qu'a l'apparition de l'objet sur le Network.
    // Potentiellement, les joueurs qui arrivent en cours d epartie ne sont pas encore là.
    public override void OnNetworkSpawn()
    {
        // Chaque client et le serveur exécute tous les codes
        // Ainsi, tous les clients vont essayer de run cette fonction aussi. Il n'est pas utile et même dangereux de laisser tout le faire
        // health.Value = stats.health; peut potentiellement créer des désynchro et ne doit être écrite que par le serveur.
        // Alors, on ignore tous ceux qui ne sont pas le serveur. Ainsi, la suite du code ne se fait que sur Server.
        //
        // /!\ A ne pas simplement désactiver le script avec enabled = false, car les clients ont besoin des ServerRpc
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
        // Meme logique que OnNetworkSpawn. Le code ne doit petre exécuter que sur Serveur.
        if (!IsServer) return;

        float chancePerFrame = Time.deltaTime / stats.breakChance;
        if (!broken.Value && Random.value < chancePerFrame)
        {
            Break();
            Debug.LogError($"The module IS BROKEN");
        }
    }

    // Une ServerRPC est du code APPELÉ par un CLIENT et EXÉCUTÉE sur le SERVEUR
    // C'est à voir comme une demande auprès du client pour exécuter du code
    // C'est ici que le serveur doit analyser la demande et réagir en fonction. 
    // Souvent le serveur doit syncro cette demande envers tous les clients.
    // Ainsi, on verra souvent des ClientRPC appelées ici et des NetworkVariable màj.
    //
    // /!\ LES CLIENTS NE DOIVENT PAS (ou quasiment jamais) EXECUTER DE LOGIQUE QUI PEUT ETRE SYNC
    // par exemple, ici, on met le module a broken uniquement sur le serveur 
    // et on éxecute la logique de casse ici. Le serveur change les variables puis les envoie aux clients
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
        Debug.LogWarning("The module is broken");
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void RepairServerRpc()
    {
        if (!broken.Value) return;
        Debug.Log("Module repaired !");
        broken.Value = false;
        health.Value = stats.health;
        behaviour.OnRepair(this);
    }
}

// Note, ici les clients n'exécute aucun code dans ce script. Ils font des demandes au serveur. La logique module est purement SERVER SIDE