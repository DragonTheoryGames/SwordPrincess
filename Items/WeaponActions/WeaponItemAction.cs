using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemAction : ScriptableObject {
    public int actionID;

    public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        if(playerPerformingAction.IsOwner) {
            playerPerformingAction.playerNetworkManager.currentWeaponID.Value = weaponPerformingAction.itemID;
        }
    }
}
