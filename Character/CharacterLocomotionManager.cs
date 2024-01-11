using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLocomotionManager : MonoBehaviour {

    CharacterManager character;

    [Header("Ground & Jump")]
    [SerializeField] protected float gravity = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = .1f;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedVelocity = -5;
    [SerializeField] protected float fallStartYVelocity = -5;
    protected bool isFalling = false;
    protected float inAirTimer = 0;
    protected bool isPressedJumped = false;

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update() {
        GroundCheck();
        GravityCheck();
    }

    protected void GroundCheck() {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckRadius, groundLayer);
    }

    protected void GravityCheck() {
        if (character.isGrounded) {
            if(yVelocity.y < 0) {
                inAirTimer = 0;
                isFalling = false;
                yVelocity.y = groundedVelocity;
            }
            character.characterNetworkManager.isJumping.Value = false;
            inAirTimer = 0;
        }
        else {
            if (!character.isJumping && !isFalling) {
                isFalling = true;
                yVelocity.y = gravity;
            }

            inAirTimer += Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);
            yVelocity.y += gravity * Time.deltaTime;
            character.characterController.Move(yVelocity * Time.deltaTime);
        }
    }

}
