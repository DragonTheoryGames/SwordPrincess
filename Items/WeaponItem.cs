using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons")]
public class WeaponItem : Item {
    
    // ANIMATOR CONTROLLER MAYBE

    [Header("Weapon Info")]
    public GameObject weaponModel;
    
    [Header("Base Damage")]
    public int swiftDamage = 0;
    public int strongDamage = 0;
    public int magicDamage = 0;
    
    public int lightningDamage = 0;
    public int iceDamage = 0;
    public int gravityDamage = 0;
    public int fireDamage = 0;

    [Header("Poise Damage")]
    public float poiseDamage = 10;

    [Header("Attack Modifiers")]
    public float swiftAttack01DamageModifier = 1;
    public float strongAttack01DamageModifier = 1.5f;
    // WEAPON MODIFIER
    // CRITICAL MOD

    [Header("Stamina Cost")]
    public int baseStaminaCost = 20;
    public float swiftAttack01StaminaMultiplier = 1;
    public float strongAttack01StaminaMultiplier = 1.5f;
    // RUNNING MOD
    // AIR MOD

    [Header("Actions")]
    public WeaponItemAction swiftAttack;
    public WeaponItemAction strongAttack;
    // WEAPON DEFLECTION
    // CAN BE BUFFED

    // SOUNDS
}
