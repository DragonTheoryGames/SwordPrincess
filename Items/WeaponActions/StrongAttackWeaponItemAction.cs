using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Strong Attack")]
public class StrongAttackWeaponItemAction : WeaponItemAction {

    [SerializeField] string HeavyCharge = "StrongAttack01";

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
        if (!playerPerformingAction.IsOwner) {return;}
        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0) {return;}
        if (!playerPerformingAction.isGrounded) {return;}

        PerformSwiftAttack(playerPerformingAction, weaponPerformingAction);
    }

    void PerformSwiftAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.StrongAttack01, HeavyCharge, true);
    }
}
