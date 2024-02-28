using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour {
    
    CharacterManager character;

    int vertical;
    int horizontal;

    [Header("Damage Animation")]
    [SerializeField] string hitFowardMedium01 = "Damage_Front_Big_ver_A";
    [SerializeField] string hitBackwardMedium01 = "Damage_Back_Small_ver_A";
    [SerializeField] string hitLeftMedium01 = "Damage_Left_Small_ver_A";
    [SerializeField] string hitRightMedium01 = "Damage_Right_Small_ver_A";

    public List<string> frontMediumDamage = new List<string>();
    public List<string> backMediumDamage = new List<string>();
    public List<string> leftMediumDamage = new List<string>();
    public List<string> rightMediumDamage = new List<string>();

    // String Formatting

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    protected virtual void Start() {
        frontMediumDamage.Add(hitFowardMedium01);
        backMediumDamage.Add(hitBackwardMedium01);
        leftMediumDamage.Add(hitLeftMedium01);
        rightMediumDamage.Add(hitRightMedium01);
    }

    public string GetAnimationFromList(List<string> animationList) {
        int randomValue = Random.Range(0, animationList.Count);
        return animationList[randomValue];
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
