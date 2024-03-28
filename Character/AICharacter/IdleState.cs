using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Idle")]
public class IdleState : AIState {

    public override AIState Tick(AICharacterManager aiCharacter) {

        if (aiCharacter.characterCombatManager.currentTarget != null) {
            // RETURN THE PURSUE TARGET STATE
        }
        else {
            // RETURN THIS STATE TO SEARCH FOR TARGET
            aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
        }

        return this;
    }
}
