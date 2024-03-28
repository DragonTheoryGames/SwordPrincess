using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager {

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    [SerializeField] float minDetectionAngle = -35;
    [SerializeField] float maxDetectionAngle = 35;

    public void FindATargetViaLineOfSight(AICharacterManager aiCharacter) {
        if(currentTarget != null) {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.singleton.GetCharacterLayers());
    
        for (int i = 0; i < colliders.Length; i++) {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
            if (targetCharacter == null ) {continue;}
            if (targetCharacter == aiCharacter) {continue;}
            if (targetCharacter.isDead) {continue;}
            
            if (WorldUtilityManager.singleton.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup)) {
                //CHECK FOR ANGLE
                Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                if (viewableAngle > minDetectionAngle && viewableAngle < maxDetectionAngle) {
                    //CECK FOR OBSTRUCTION
                    if (!Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, 
                                        targetCharacter.characterCombatManager.lockOnTransform.position, 
                                        WorldUtilityManager.singleton.GetEnvironmentLayers())) {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, 
                                        targetCharacter.characterCombatManager.lockOnTransform.position);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                    }
                }
            }
        }
    }
    
}
