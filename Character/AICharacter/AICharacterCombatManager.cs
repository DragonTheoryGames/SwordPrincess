using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager {

    [Header("Target Information")]
    public float viewableAngle;
    public Vector3 targetDirection;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    [SerializeField] public float minFOV = -55;
    [SerializeField] public float maxFOV = 55;

    public void FindATargetViaLineOfSight(AICharacterManager aiCharacter) {
        if(currentTarget != null) { return; }

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.singleton.GetCharacterLayers());
    
        for (int i = 0; i < colliders.Length; i++) {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
            if (targetCharacter == null ) {continue;}
            if (targetCharacter == aiCharacter) {continue;}
            if (targetCharacter.isDead) {continue;}
            
            if (WorldUtilityManager.singleton.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup)) {
                //CHECK FOR ANGLE
                Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                if (angleOfPotentialTarget > minFOV && angleOfPotentialTarget < maxFOV) {
                    //CHECK FOR OBSTRUCTION
                    if (!Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, 
                                        targetCharacter.characterCombatManager.lockOnTransform.position, 
                                        WorldUtilityManager.singleton.GetEnvironmentLayers())) {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, 
                                        targetCharacter.characterCombatManager.lockOnTransform.position);
                        targetsDirection = targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.singleton.GetAngleOfTarget(transform, targetsDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                        PivotTowardsTarget(aiCharacter);
                    }
                }
            }
        }
    }

    public void PivotTowardsTarget(AICharacterManager aiCharacter) {
        //PLAY A PIVOT ANIMATION DEPENDING ONVIEWABLE ANGLE OF TARGET
        if(aiCharacter.isPerformingAction) return;
        
        if(viewableAngle >= 20 && viewableAngle <= 60) { aiCharacter.characterAnimatorManager.PlayAnimation("TurnRight", true);}
        else if(viewableAngle <= -20 && viewableAngle >= -60) { aiCharacter.characterAnimatorManager.PlayAnimation("TurnLeft", true); }
        else if(viewableAngle >= 61 && viewableAngle <= 110) { aiCharacter.characterAnimatorManager.PlayAnimation("TurnRight", true); }
        else if(viewableAngle <= -61 && viewableAngle >= 110) { aiCharacter.characterAnimatorManager.PlayAnimation("TurnLeft", true); }
        else if(viewableAngle >= 111 && viewableAngle <= 145) { aiCharacter.characterAnimatorManager.PlayAnimation("TurnRight", true); }
        else if(viewableAngle <= -111 && viewableAngle >= 60) { aiCharacter.characterAnimatorManager.PlayAnimation("TurnLeft", true); }
        else if(viewableAngle >= 146 && viewableAngle <= 180) { aiCharacter.characterAnimatorManager.PlayAnimation("TurnRight", true); }
    }   
}
