using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUtilityManager : MonoBehaviour {

    public static WorldUtilityManager singleton;

    [SerializeField] LayerMask characterLayers;
    [SerializeField] LayerMask environmentLayers;

    void Awake() {
    if(singleton == null) {singleton = this;}
    else Destroy(gameObject);
    }

    public LayerMask GetCharacterLayers() {
        return characterLayers;
    }

    public LayerMask GetEnvironmentLayers() {
        return environmentLayers;
    }

    public bool CanIDamageThisTarget(CharacterGroup attackingcharacter, CharacterGroup targetCharacter) {
        if (attackingcharacter == CharacterGroup.Friendly) {
            switch (targetCharacter) {
                case CharacterGroup.Friendly: return false;
                case CharacterGroup.Hostile: return true;
                default: return false;
            }
        }
        else if (attackingcharacter == CharacterGroup.Hostile) {
            switch (targetCharacter) {
                case CharacterGroup.Friendly: return true;
                case CharacterGroup.Hostile: return false;
                default: return false;
            }
        }
        return false;
    }

    public float GetAngleOfTarget(Transform characterTransform, Vector3 targetDirection) {
        targetDirection.y = 0;
        float viewableAngle = Vector3.Angle(characterTransform.forward, targetDirection);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetDirection);

        if (cross.y < 0) viewableAngle = -viewableAngle;

        return viewableAngle;

    }

}
