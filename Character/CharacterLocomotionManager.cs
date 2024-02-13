using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLocomotionManager : MonoBehaviour {

    CharacterManager character;

    [Header("Ground & Jump")]
    [SerializeField] Transform groundCheckTransform; 
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = .15f;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float fallStartYVelocity = -5;
    protected bool isFalling = false;
    protected float inAirTimer = 0;
    protected bool isPressedJumped = false;

    [Header("Gravity")]
    [SerializeField] protected float gravity = -9.8f;
    [SerializeField] protected float groundedGravity = -9.8f;

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update() {
        GroundCheck();
        GravityCheck();
    }

    protected void GroundCheck() {
        RaycastHit hit;
        character.isGrounded = Physics.Raycast(groundCheckTransform.position, Vector2.down, out hit, groundCheckDistance, groundLayer);
        Debug.DrawRay(groundCheckTransform.position, Vector3.down * groundCheckDistance, character.isGrounded ? Color.green : Color.red);
    }

    protected void GravityCheck() {
        if (character.isGrounded) {
            if(yVelocity.y < 0) {
                isFalling = false;
                yVelocity.y = gravity;
                character.characterNetworkManager.isJumping.Value = false;
            }
        }
        else {
            if (!character.characterNetworkManager.isJumping.Value && !isFalling) {
                isFalling = true;
                yVelocity.y = gravity;
            }
            yVelocity.y += gravity * Time.deltaTime;
        }
        character.characterController.Move(yVelocity * Time.deltaTime);
    }

}
