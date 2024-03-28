using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ScriptableObject {
    
    public virtual AIState Tick(AICharacterManager aiCharacter) {
        Debug.Log("WE ARE RUNNING THIS STATE");

        //DO SOME LOGIC TO FIIND THE PLAYER
        //RETURN PURSUE TARGET STATE

        return this;
    }
}
