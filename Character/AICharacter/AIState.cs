using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ScriptableObject {
    
    public virtual AIState Tick(AICharacterManager aiCharacter) {
        return this;
    }

    protected virtual AIState SwitchState(AICharacterManager aICharacter, AIState newState) {
        ResetStateFlag(aICharacter);
        return newState;
    }

    protected virtual void ResetStateFlag(AICharacterManager aICharacter) {
        // CLEAR STATE FLAGS
    }
}
