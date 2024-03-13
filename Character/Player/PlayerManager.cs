using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerManager : CharacterManager {

    [Header("DEBUG MENU")]
    [SerializeField] bool respawnCharacter = false;

    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;

    [Header("Strings")]
    string deathBackwards = "DeathBackward";
    string empty = "Empty";

    protected override void Awake() {
        base.Awake();
        
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
    }

    protected override void Update() {
        base.Update();
        if (!IsOwner) {return;}
        
        playerLocomotionManager.Movement();
        playerStatsManager.RegenerateStamina();

        DebugMenu();
    }

    protected override void LateUpdate() {
        if (!IsOwner) {return;}
        base.LateUpdate();
        PlayerCamera.singleton.CameraActions();
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.OnClientConnectedCallback +=  OnClientConnectedCallback;

        if (IsOwner) {
            PlayerCamera.singleton.player = this;
            PlayerInputManager.singleton.player = this;
            WorldSaveGameManager.singleton.player = this;

            //Update MaxResources on Stat change.
            playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
            //Updates UI Resource Bars
            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.singleton.playerUIHUDManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.singleton.playerUIHUDManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
            //EQUIPMENT
            playerNetworkManager.currentWeaponID.OnValueChanged += playerNetworkManager.OnCurrentWeaponIDChange;
        }

        //Flags
        playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;
        playerNetworkManager.isChargingAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackChanged;

        //STATS
        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

        // UPON CONNECTING AS MULTIPLAYER, GRAB STATS
        if(IsOwner && !IsServer) {
            LoadPlayerGame(ref WorldSaveGameManager.singleton.currentCharacterData);
        }
    }

    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();
         NetworkManager.Singleton.OnClientConnectedCallback -=  OnClientConnectedCallback;

        if (IsOwner) {
            //Update MaxResources on Stat change.
            playerNetworkManager.vitality.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;
            //Updates UI Resource Bars
            playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.singleton.playerUIHUDManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.singleton.playerUIHUDManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenTimer;
            //EQUIPMENT
            playerNetworkManager.currentWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentWeaponIDChange;
        }

        //Flags
        playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;
        playerNetworkManager.isChargingAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackChanged;

        //STATS
        playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHP;
    }

    void OnClientConnectedCallback(ulong clientID) {
        // KEEP LIST OF ACTIVE PLAYERS
        if (!IsServer && IsOwner) { //SERVER IS THE HOST SO NO NEED TO LOAD OTHER PLAYERS
            foreach(var player in WorldGameSessionManager.singleton.players) {
                if(player != this) {
                    player.LoadOtherPlayers(player);
                }
            }
        }
    }

    void DebugMenu() {
        if (respawnCharacter) {
            respawnCharacter = false;
            playerNetworkManager.isDead.Value = false;
            ReviveCharacter();
        }
    }

    public override void DeathEvent(bool isManuallySelectDeathAnimation = false) {
        base.DeathEvent(isManuallySelectDeathAnimation);

        if(IsOwner) {
            playerNetworkManager.currentHealth.Value = 0;
            playerNetworkManager.isDead.Value = true;
            PlayerUIManager.singleton.playerUIPopUpManager.SendPopUp();
            // RESET FLAGS
            // IF WE ARE NOT GROUNDED, DIE IN THE AIR

            if (!isManuallySelectDeathAnimation) {
                characterAnimatorManager.PlayAnimation(deathBackwards, true);
            }
        }
        // PLAY DEATH SFX

        // IF ALL PLAYERS DEAD RESPAWN

        // DROP RUNES
        // DISABLE CHARACTER
    }

    public override void ReviveCharacter() {
        base.ReviveCharacter();
        if (IsOwner) {
            isDead = false;
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            // RESTORE FOCUS
            // PLAY REZ EFFECTS
            playerAnimatorManager.PlayAnimation(empty, false);
        }
    }

    public void SavePlayerGame(ref CharacterSaveData currentCharacterData) {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

        currentCharacterData.vitality = playerNetworkManager.vitality.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;
    }

    public void LoadPlayerGame(ref CharacterSaveData currentCharacterData) {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3 (currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;

        playerNetworkManager.vitality.Value = currentCharacterData.vitality;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;

        //Stats and Attributes
        playerNetworkManager.maxHealth.Value = playerStatsManager.CalcualteHealthBasedOnVitalityLevel(currentCharacterData.vitality);
        playerNetworkManager.maxStamina.Value = playerStatsManager.CalcualteStaminaBasedOnEnduranceLevel(currentCharacterData.endurance);
        playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
        playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
        //PlayerUIManager.singleton.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
    }

    public void LoadOtherPlayers(PlayerManager otherPlayer) {
        // SYNC WEAPONS
        // SYNC PLAYERS
        // SYNC LOCKON
        if (playerNetworkManager.isLockedOn.Value) {
            playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
        }
    }
}
