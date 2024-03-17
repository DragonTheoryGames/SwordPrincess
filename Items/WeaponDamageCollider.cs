using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageCollider : DamageCollider {
    
    [Header("Attacking Character")]
    public CharacterManager characterAttacking;

    [Header("Weapon Attack Modifier")]
    public float swiftAttack01Modifier;
    public float swiftAttack02Modifier;
    public float swiftAttack03Modifier;
    public float swiftAttack04Modifier;
    public float strongAttack01Modifier;
    public float strongAttack02Modifier;
    public float strongAttack03Modifier;
    public float strongAttack04Modifier;

    protected override void Awake() {
        base.Awake();

        if (damageCollider == null) {
            damageCollider = GetComponent<Collider>();
        }
        damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider collider) {
        base.OnTriggerEnter(collider);

        CharacterManager damageTarget = collider.GetComponentInParent<CharacterManager>();
        
        if (damageTarget == characterAttacking) {return;} //STOP HITTING YOURSELF

        if(damageTarget != null) {
            Vector3 contactPoint = collider.ClosestPointOnBounds(transform.position);

            //CHECK FOR FREINDLY FIRE
            //CHECK FOR BLOCKING
            //CHECK FOR INVULNERABLITY

            DamageTarget(damageTarget);
        }
    }

    protected override void DamageTarget(CharacterManager damageTarget) {
        base.DamageTarget(damageTarget);

        if (charactersDamaged.Contains(damageTarget)){return;} // DONT DAMAGE TARGET MULTIPLE TIMES WITH SINGLE ATTACK

        charactersDamaged.Add(damageTarget);

        HealthDamage healthDamage = Instantiate(WorldCharacterEffectsManager.singleton.healthDamage);
        healthDamage.swiftDamage = swiftDamage;
        healthDamage.strongDamage = strongDamage;
        healthDamage.contactPoint = contactPoint;
        healthDamage.angleHitFrom = Vector3.SignedAngle(characterAttacking.transform.forward, damageTarget.transform.forward, Vector3.up);

        switch (characterAttacking.characterCombatManager.currentAttackType)
        {
            case AttackType.SwiftAttack01:
                ApplyAttackDamageModifiers(swiftAttack01Modifier, healthDamage);
                break;
            case AttackType.SwiftAttack02:
                ApplyAttackDamageModifiers(swiftAttack02Modifier, healthDamage);
                break;
            case AttackType.SwiftAttack03:
                ApplyAttackDamageModifiers(swiftAttack03Modifier, healthDamage);
                break;
            case AttackType.SwiftAttack04:
                ApplyAttackDamageModifiers(swiftAttack04Modifier, healthDamage);
                break;
            case AttackType.StrongAttack01:
                ApplyAttackDamageModifiers(strongAttack01Modifier, healthDamage);
                break;
            case AttackType.StrongAttack02:
                ApplyAttackDamageModifiers(strongAttack02Modifier, healthDamage);
                break;
            case AttackType.StrongAttack03:
                ApplyAttackDamageModifiers(strongAttack03Modifier, healthDamage);
                break;
            case AttackType.StrongAttack04:
                ApplyAttackDamageModifiers(strongAttack04Modifier, healthDamage);
                break;
            default:
                break;
        }

        if (characterAttacking.IsOwner) {
            damageTarget.characterNetworkManager.NotifyServerOfDamageServerRpc( 
                damageTarget.NetworkObjectId,
                characterAttacking.NetworkObjectId,
                healthDamage.swiftDamage,
                healthDamage.strongDamage,
                healthDamage.poiseDamage,
                healthDamage.angleHitFrom,
                healthDamage.contactPoint.x,
                healthDamage.contactPoint.y,
                healthDamage.contactPoint.z
            );
        }

        //damageTarget.characterEffectsManager.ProcessInstantEffect(healthDamage);
    }

    private void ApplyAttackDamageModifiers(float modifier, HealthDamage healthDamage) {
        healthDamage.swiftDamage *= modifier;
    }
}