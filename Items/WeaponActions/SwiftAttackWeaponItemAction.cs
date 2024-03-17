using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Swift Attack")]
public class SwiftAttackWeaponItemAction : WeaponItemAction {

    [SerializeField] string swiftAttack01 = "SwiftAttack01";
    [SerializeField] string swiftAttack02 = "SwiftAttack02";
    [SerializeField] string swiftAttack03 = "SwiftAttack03";
    [SerializeField] string swiftAttack04 = "SwiftAttack04";

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
        if (!playerPerformingAction.IsOwner) {return;}
        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0) {return;}
        if (!playerPerformingAction.isGrounded) {return;}

        PerformSwiftAttack(playerPerformingAction, weaponPerformingAction);
    }

    void PerformSwiftAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        if (playerPerformingAction.playerCombatManager.canCombo && playerPerformingAction.isPerformingAction) {
            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerfromed == swiftAttack01) {
                playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack02, swiftAttack02, true);
            }
            else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerfromed == swiftAttack02) {
                playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack03, swiftAttack03, true);
            }
            else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerfromed == swiftAttack03) {
                playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack04, swiftAttack04, true);
            }
            else {
                playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack01, swiftAttack01, true);
            }
            
        }
        else if (!playerPerformingAction.isPerformingAction){
            playerPerformingAction.playerAnimatorManager.PlayAttackAnimation(AttackType.SwiftAttack01, swiftAttack01, true);
        }
        
    }
}
