using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Stamina Damage")]
public class StaminaDamage : InstantCharacterEffects {

    public int staminaDamage;

    public override void ProcessEffect(CharacterManager character) {
        CalculateStaminaDamage(character);
    }

    void CalculateStaminaDamage(CharacterManager character) {
        if (!character.IsOwner) {return;}
        character.characterNetworkManager.currentStamina.Value -= staminaDamage;
    }
}
