using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Pursue Target")]
public class PursueTargetState : AIState {
    
    public override AIState Tick(AICharacterManager aiCharacter) {

        if(aiCharacter.isPerformingAction) {return this;}
        if(aiCharacter.aiCharacterCombatManager.currentTarget == null) {
            return SwitchState(aiCharacter, aiCharacter.idle);
        }
        if (!aiCharacter.navMeshAgent.enabled) {
            aiCharacter.navMeshAgent.enabled = true;
        }

        // if target is outside FOV : pivot
        if (aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minFOV - 30
            || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maxFOV + 30) {
            aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
        }

        aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

        // IF IN RANGE SWITCH TO COMBAT
        // IF TARGER IS UNREACHABLE, RETURN HOME
        // PURSUE TARGET
        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this;
    }
}
