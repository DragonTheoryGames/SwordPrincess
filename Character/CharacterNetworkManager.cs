using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class CharacterNetworkManager : NetworkBehaviour {

    CharacterManager character;

    [Header("Status")]
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Vector3 networkPositionVelocity;
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTime = 0.1f;

    [Header("Animator")]
    public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> networkMoveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
   
    [Header("Flags")]
    public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Stats")]
    public NetworkVariable<int> vitality = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> endurance = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> cunning = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Resources")]
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentMana = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxMana = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
    }

    public void CheckHP(int oldValue, int newValue) {
        if (currentHealth.Value <= 0) {
            character.DeathEvent();
        }

        if (character.IsOwner) {
            if (currentHealth.Value > maxHealth.Value) {
                currentHealth.Value = maxHealth.Value;
            }
        }
    }

    //ACTIONS
    [ServerRpc]
    public void NotifyServerofActionAnimationServerRpc(ulong clientID, string animationID) {
        if (IsServer) {
            PlayClientActionAnimationClientRpc(clientID, animationID);
        }
    }
    [ClientRpc]
    public void PlayClientActionAnimationClientRpc(ulong clientID, string animationID) {
        if (clientID != NetworkManager.Singleton.LocalClientId) {
            PerformActionAnimationFromServer(animationID);
        }
    }

    public void PerformActionAnimationFromServer(string animationID) {
        //character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(animationID, .2f);
    }

    //ATTACKS
    [ServerRpc]
    public void NotifyServerofAttackAnimationServerRpc(ulong clientID, string animationID) {
        if (IsServer) {
            PlayClientAttackAnimationClientRpc(clientID, animationID);
        }
    }
    [ClientRpc]
    public void PlayClientAttackAnimationClientRpc(ulong clientID, string animationID) {
        if (clientID != NetworkManager.Singleton.LocalClientId) {
            PerformAttackAnimationFromServer(animationID);
        }
    }

    public void PerformAttackAnimationFromServer(string animationID) {
        //character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(animationID, .2f);
    }

    //DAMAGE
    [ServerRpc]
    public void NotifyServerOfDamageServerRpc(
        ulong damagedCharacterID, 
        ulong attackingCharacterID,
        float swiftDamage, 
        float heavyDamage, 
        float poiseDamage,
        float hitAngle,
        float contactpointX,
        float contactpointY,
        float contactpointZ) {

        if (IsServer) {
            NotifyServerOfDamageClientRpc(
                damagedCharacterID, 
                attackingCharacterID,
                swiftDamage, 
                heavyDamage,
                poiseDamage,
                hitAngle,
                contactpointX,
                contactpointY,
                contactpointZ
            );
        }

    }
    [ClientRpc]
    public void NotifyServerOfDamageClientRpc(
        ulong damagedCharacterID, 
        ulong attackingCharacterID,
        float swiftDamage, 
        float heavyDamage, 
        float poiseDamage,
        float hitAngle,
        float contactpointX,
        float contactpointY,
        float contactpointZ) {

        ProcessCharacterDamageFromServer(
                damagedCharacterID, 
                attackingCharacterID,
                swiftDamage, 
                heavyDamage, 
                poiseDamage,
                hitAngle,
                contactpointX,
                contactpointY,
                contactpointZ
        );

    }

    public void ProcessCharacterDamageFromServer(
        ulong damagedCharacterID, 
        ulong attackingCharacterID,
        float swiftDamage, 
        float heavyDamage, 
        float poiseDamage,
        float hitAngle,
        float contactpointX,
        float contactpointY,
        float contactpointZ) {

        CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
        CharacterManager attackingCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[attackingCharacterID].gameObject.GetComponent<CharacterManager>();
    
        HealthDamage healthDamage = Instantiate(WorldCharacterEffectsManager.singleton.healthDamage);
        healthDamage.swiftDamage = swiftDamage;
        healthDamage.heavyDamage = heavyDamage;
        //ADD OTHER DAMAGE TYPES
        healthDamage.angleHitFrom = hitAngle;
        healthDamage.contactPoint = new Vector3(contactpointX, contactpointY, contactpointZ);

        damagedCharacter.characterEffectsManager.ProcessInstantEffect(healthDamage);

    }
}
