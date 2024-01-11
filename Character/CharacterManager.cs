using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : NetworkBehaviour {
    
    [HideInInspector] public CharacterNetworkManager characterNetworkManager;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool canMove = true;
    public bool canRotate = true;
    public bool isGrounded = false;
    public bool isJumping = false;

    protected virtual void Awake(){
        DontDestroyOnLoad(this);

        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
    }

    protected virtual void Start() {
        IgnoreCharacterColliders();
    }

    protected virtual void Update() {
        SetFlags();
        if (IsOwner) {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        else {
            transform.position = Vector3.SmoothDamp(transform.position, characterNetworkManager.networkPosition.Value, 
                ref characterNetworkManager.networkPositionVelocity, characterNetworkManager.networkPositionSmoothTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, characterNetworkManager.networkRotation.Value, 
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate() {

    }

    void SetFlags() {
        animator.SetBool("isGrounded", isGrounded);
    }

    public virtual void DeathEvent(bool isManuallySelectDeathAnimation = false) {
        
    }

    public virtual void ReviveCharacter() {
        
    }

    protected virtual void IgnoreCharacterColliders() {
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
        List<Collider> ignoreColliders = new List<Collider>();

        ignoreColliders.Add(characterControllerCollider);
        foreach (Collider collider in damageableCharacterColliders) {
            ignoreColliders.Add(collider);
        }
        
        //GO THROUGH EVERY COLLIDER IN LIST AND MAKE THEM IGNORE EACH OTHER
        foreach (Collider collider in ignoreColliders) {
            foreach(Collider otherCollider in ignoreColliders) {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }
    }
}
