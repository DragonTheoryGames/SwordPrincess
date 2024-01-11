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
        damageCollider.heavyDamage = weapon.heavyDamage;

        damageCollider.swiftAttack01Modifier = weapon.swiftAttack01DamageModifier;
    }
}
