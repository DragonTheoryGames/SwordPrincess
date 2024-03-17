using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Strong Attack")]
public class StrongAttackWeaponItemAction : WeaponItemAction {

    [SerializeField] string strongAttack01 = "StrongAttack01";
    [SerializeField] string strongAttack02 = "StrongAttack02";
    [SerializeField] string strongAttack03 = "StrongAttack03";
    [SerializeField] string strongAttack04 = "StrongAttack04";

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
        if (!playerPerformingAction.IsOwner) {return;}
        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0) {return;}
        if (!playerPerformingAction.isGrounded) {return;}

        PerformSwiftAttack(playerPerformingAction, weaponPerformingAction);
    }

    void PerformSwiftAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        if (playerPerformingAction.playerCombatManager.canCombo && playerPerformingAction.isPerformingAction) {
            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerfromed == strongAttack01) {
                playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack02, strongAttack02, true);
            }
            else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerfromed == strongAttack02) {
                playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack03, strongAttack03, true);
            }
            else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerfromed == strongAttack03) {
                playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack04, strongAttack04, true);
            }
            else {
                playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack01, strongAttack01, true);
            }
            
        }
        else if (!playerPerformingAction.isPerformingAction){
            playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack01, strongAttack01, true);
        }
    }
}
