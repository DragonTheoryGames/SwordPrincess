using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Health Damage")]
public class HealthDamage : InstantCharacterEffects {

    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float swiftDamage = 0;
    public float heavyDamage = 0;

    public float lightningDamage = 0;
    public float iceDamage = 0;
    public float gravityDamage = 0;
    public float flameDamage = 0;

    // TODO BUILD UPS

    [Header("Final Damage")]
    int finalDamgeDealt = 0; 

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool isPoiseBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string newDamageAnimation;

    [Header("SFX")]
    public bool isPlaySFX = true;
    public AudioClip additionalDamgeSFX;

    [Header("Details")]
    public float angleHitFrom;          //DETERMINES ANIMATION TO PLAY
    public Vector3 contactPoint;        //DETERMINES BLOOD EFFECTS AND CRITICALS

    public override void ProcessEffect(CharacterManager character) {
        base.ProcessEffect(character);

        if (character.characterNetworkManager.isDead.Value) {return;}
        //CHECK FOR INVULNERABILITY
        CalculateDamage(character);
        //CHECK DIRECTION
        //PLAY ANIMATION
        //CHECK FOR BUILDUP
        PlayVFX(character);
        PlaySFX(character);

        //AI LOGIC
    }

    void CalculateDamage(CharacterManager character) {
        if(!character.IsOwner) {return;}
        if(characterCausingDamage != null) {
            // CHECK FOR DAMAGE MODS
        }

        //CHECK CHARACTER FOR DAMAGE REDUCTION
        //ADD ALL DAMAGE TOGETHER
        //APPLY DAMAGE
        finalDamgeDealt = Mathf.RoundToInt( swiftDamage +
                                            heavyDamage +
                                            lightningDamage +
                                            iceDamage +
                                            gravityDamage +
                                            flameDamage);
        finalDamgeDealt = finalDamgeDealt <= 0 ? 1 : finalDamgeDealt;

        character.characterNetworkManager.currentHealth.Value -= finalDamgeDealt;

        //CHECK FOR POISE DAMAGE
    }

    void PlayVFX(CharacterManager character) {
        
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    void PlaySFX(CharacterManager character) {
        AudioClip physicalDamageSFX = null;
        physicalDamageSFX = WorldSoundFXManager.singleton.RandomSFX(WorldSoundFXManager.singleton.physicalDamageSFX);

        character.characterSoundFXManager.PlaySFX(physicalDamageSFX);
    }

}
