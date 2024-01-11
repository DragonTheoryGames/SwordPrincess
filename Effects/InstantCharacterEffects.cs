using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCharacterEffects : ScriptableObject {
    
    [Header("Effect ID")]
    public int instantEffectId;

    public virtual void ProcessEffect(CharacterManager character) {

    }
}
