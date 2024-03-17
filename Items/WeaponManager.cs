using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    
    public WeaponDamageCollider damageCollider;

    void Awake() {
        damageCollider = GetComponentInChildren<WeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager owner, WeaponItem weapon) {
        damageCollider.characterAttacking = owner;
        damageCollider.swiftDamage = weapon.swiftDamage;
        damageCollider.strongDamage = weapon.strongDamage;

        damageCollider.swiftAttack01Modifier = weapon.swiftAttack01DamageModifier;
        damageCollider.swiftAttack02Modifier = weapon.swiftAttack02DamageModifier;
        damageCollider.swiftAttack03Modifier = weapon.swiftAttack03DamageModifier;
        damageCollider.swiftAttack04Modifier = weapon.swiftAttack04DamageModifier;
        damageCollider.strongAttack01Modifier = weapon.strongAttack01DamageModifier;
        damageCollider.strongAttack02Modifier = weapon.strongAttack02DamageModifier;
        damageCollider.strongAttack03Modifier = weapon.strongAttack03DamageModifier;
        damageCollider.strongAttack04Modifier = weapon.strongAttack04DamageModifier;
    }
}
