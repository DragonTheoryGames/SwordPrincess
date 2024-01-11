using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour {

    [Header("Collider")]
    protected Collider damageCollider;

    [Header("Damage")]
    public float swiftDamage = 0;
    public float heavyDamage = 0;

    public float magicDamage = 0;
    public float windDamage = 0;
    public float waterDamage = 0;
    public float stoneDamage = 0;
    public float fireDamage = 0;

    [Header("Contact Point")]
    Vector3 contactPoint;

    [Header("Characters Damage")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    protected virtual void Awake() {

    }

    protected virtual void OnTriggerEnter(Collider collider) {
        
        CharacterManager damageTarget = collider.GetComponentInParent<CharacterManager>();

        if (damageTarget != null) {
            contactPoint = collider.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            DamageTarget(damageTarget);
        }
        
    }

    protected virtual void DamageTarget(CharacterManager damageTarget) {
        if(charactersDamaged.Contains(damageTarget)) {return;}
        charactersDamaged.Add(damageTarget);

        HealthDamage healthDamage = Instantiate(WorldCharacterEffectsManager.singleton.healthDamage);
        healthDamage.swiftDamage = swiftDamage;
        healthDamage.heavyDamage = heavyDamage;
        healthDamage.ProcessEffect(damageTarget);
    }

    public virtual void EnableDamageCollider() {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider() {
        damageCollider.enabled = false;
        charactersDamaged.Clear();
    }
}
