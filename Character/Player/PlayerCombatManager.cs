using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager {

    PlayerManager player;

    public WeaponItem currentWeapon;
    

    protected override void Awake() {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponAction(WeaponItemAction weaponAction, WeaponItem weapon) {
        if (player.IsOwner) {
            weaponAction.AttemptToPerformAction(player, currentWeapon);
            player.playerNetworkManager.NotifyServerofWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weapon.itemID);
        }
    }

    public void DrainAttackStamina() {
        if (!player.IsOwner) {return;}
        if (currentWeapon == null) {return;}

        float staminaDeducted = 0;

        switch (currentAttackType)
        {
            case AttackType.SwiftAttack01:
                staminaDeducted = currentWeapon.baseStaminaCost * currentWeapon.swiftAttack01StaminaMultiplier;
                break;
            default:
                break;
        }

        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        if(player.IsOwner) {
            PlayerCamera.singleton.SetLockCameraHeight();
        }
    }
}