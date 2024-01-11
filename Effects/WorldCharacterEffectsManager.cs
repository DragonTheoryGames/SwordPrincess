using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour {
    
    public static WorldCharacterEffectsManager singleton;

    [Header("VFX")]
    public GameObject bloodSplatterVFX;

    [Header("Damage")]
    public HealthDamage healthDamage;

    [SerializeField] List<InstantCharacterEffects> instantEffects;

    void Awake() {
        if (singleton == null) {singleton = this;}
        else {Destroy(this);}

        GenerateEffectIDs();
    }

    void GenerateEffectIDs() {
        for (int i = 0; i < instantEffects.Count; i++) {
            instantEffects[i].instantEffectId = i;
        }
    }
}
