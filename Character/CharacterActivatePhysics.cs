using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActivatePhysics : MonoBehaviour {
    
    public CharacterManager character;
    public CharacterController controller;


    void Awake() {
        character = GetComponent<PlayerManager>();
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate() {
        if (character.isGrounded) {
            controller.enabled = true;
            GetComponent<CharacterActivatePhysics>().enabled = false;
        }
    }
}
