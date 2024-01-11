using UnityEngine;

public class CharacterStatsManager : MonoBehaviour {

    CharacterManager character;

    [Header("Stats")]
    [SerializeField] int staminaRegenerationAmount = 2;
    [SerializeField] float staminaRegenerationDelay = 1;
    float staminaRegenerationTimer = 0;
    float staminaTickTimer = 0;

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start() {
        
    }

    public int CalcualteHealthBasedOnVitalityLevel(int vitality) {
        int health = 0;
        health = vitality * 10;
        return health;
    }

    public int CalcualteStaminaBasedOnEnduranceLevel(int endurance) {
        int stamina;
        stamina = endurance * 10;
        return stamina;
    }

    public virtual void RegenerateStamina() {
        if(!character.IsOwner) {return;}
        if(character.characterNetworkManager.isSprinting.Value) {return;}
        if (character.isPerformingAction) {return;}

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay) {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value) {
                staminaTickTimer += Time.deltaTime;
            }

            if (staminaTickTimer >= .1f) {
                staminaTickTimer = 0;
                character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(int oldStaminaValue, int newStaminaValue) {
        if(newStaminaValue < oldStaminaValue) {
            staminaRegenerationTimer = 0;
        }
    }
}
