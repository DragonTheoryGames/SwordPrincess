using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager {
    
    [Header("Debug")]
    [SerializeField] InstantCharacterEffects effectsTest;
    [SerializeField] bool processEffect = false;

    private void Update() {
        if (processEffect) {
            processEffect = false;
            InstantCharacterEffects effect = Instantiate(effectsTest) as InstantCharacterEffects;
            ProcessInstantEffect(effect);
        }
    }
}
