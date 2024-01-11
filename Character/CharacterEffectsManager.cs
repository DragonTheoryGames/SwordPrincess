using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour {

    //Process Instant Effects (Damage/Healing)
    //Process Timed Effects (Poison/Bleed)
    //Process Static Effects (Buffs/Debuffs)

    CharacterManager character;

    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterVFX;

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
    }

    public void ProcessInstantEffect(InstantCharacterEffects effect) {
        effect.ProcessEffect(character);
        //Take and Effect
        //Process It
    }

    public void PlayBloodSplatterVFX(Vector3 contactPoint) {
        if(bloodSplatterVFX != null) { //USE UNIQUE EFFECT
            GameObject bloodSpatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        else { //USE GENERIC EFFECT
            GameObject bloodSpatter = Instantiate(WorldCharacterEffectsManager.singleton.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }
    
}
