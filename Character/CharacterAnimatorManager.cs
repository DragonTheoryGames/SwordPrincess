using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour {
    
    CharacterManager character;

    int vertical;
    int horizontal;

    // String Formatting

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue) {
        if (Mathf.Abs(verticalValue) < .2) {verticalValue = 0;}
        if (Mathf.Abs(horizontalValue) < .2) {horizontalValue = 0;}
        character.animator.SetFloat(horizontal, horizontalValue, .1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalValue, .1f, Time.deltaTime);
    }

    public virtual void PlayAnimation(string animation, bool isPerformingAction, bool canRotate = false, bool canMove = false) {
        character.animator.CrossFade(animation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
        character.canRotate = canRotate;

        character.characterNetworkManager.NotifyServerofActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, animation);
    }

    public virtual void PlayAttackAnimation(AttackType attackType, string animation, bool isPerformingAction, bool canRotate = false, bool canMove = false) {
        character.characterCombatManager.currentAttackType = attackType;
        character.animator.CrossFade(animation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
        character.canRotate = canRotate;

        //KEEP TRACK OF LAST ATTACK
        //KEEP TRACK OF CURRENT ATTACK TYPE
        //UPDATE ANIMATION TO CURRENT WEAPON
        //CAN ATTACK BE PARRIED
        //TELL NETWORK WE ARE ATTACKING

        character.characterNetworkManager.NotifyServerofAttackAnimationServerRpc(NetworkManager.Singleton.LocalClientId, animation);
    }
}
