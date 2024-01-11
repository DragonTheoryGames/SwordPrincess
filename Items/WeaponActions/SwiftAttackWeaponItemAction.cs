using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Swift Attack")]
public class SwiftAttackWeaponItemAction : WeaponItemAction {

    [SerializeField] string swiftAttack01 = "SwiftAttack01";

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
        if (!playerPerformingAction.IsOwner) {return;}
        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0) {return;}
        if (!playerPerformingAction.isGrounded) {return;}

        PerformSwiftAttack(playerPerformingAction, weaponPerformingAction);
    }

    void PerformSwiftAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack01, swiftAttack01, true);
    }
}
