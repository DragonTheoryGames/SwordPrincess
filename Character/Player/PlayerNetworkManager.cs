using UnityEngine;
using Unity.Collections;
using Unity.Netcode;

public class PlayerNetworkManager : CharacterNetworkManager {
    
    PlayerManager player;
    PlayerCombatManager playerCombatManager;

    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Equipment")]
    public NetworkVariable<int> currentWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake() {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void SetNewMaxHealthValue(int oldVitality, int newVitality) {
        maxHealth.Value = player.playerStatsManager.CalcualteHealthBasedOnVitalityLevel(newVitality);
        PlayerUIManager.singleton.playerUIHUDManager.SetMaxHealthValue(newVitality);
        currentHealth.Value = maxHealth.Value;
    }
        
    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance) {
        maxStamina.Value = player.playerStatsManager.CalcualteStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.singleton.playerUIHUDManager.SetMaxStaminaValue(newEndurance);
        currentStamina.Value = maxStamina.Value;
    }

    public void OnCurrentWeaponIDChange(int oldID, int newID) {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Singleton.GetWeaponByID(newID));
        //EQUIP WEAPON AND PLAY ANIMATION EP20;
        player.playerInventoryManager.currentWeapon = newWeapon;
        player.playerEquipmentManager.LoadWeapon();
        player.playerCombatManager.currentWeapon = newWeapon;
    }

    [ServerRpc]
    public void NotifyServerofWeaponActionServerRpc(ulong clientID, int actionID, int weaponID) {
        if (IsServer) {
            NotifyServerofWeaponActionClientRpc(clientID, actionID, weaponID);
        }
    }

    [ClientRpc]
    public void NotifyServerofWeaponActionClientRpc(ulong clientID, int actionID, int weaponID) {
        if (clientID != NetworkManager.Singleton.LocalClientId) {
            PerformWeaponAction(actionID, weaponID);
        }
    }

    void PerformWeaponAction(int actionID, int weaponID) {
    
        WeaponItemAction weaponAction = WorldActionManager.singleton.GetWeaponItemAction(actionID);
        if (weaponAction != null) {
            weaponAction.AttemptToPerformAction(player, WorldItemDatabase.Singleton.GetWeaponByID(weaponID));
        }
        else Debug.LogError("ACTION IS NULL, CANNOT BE PERFORMED");
    }
}
