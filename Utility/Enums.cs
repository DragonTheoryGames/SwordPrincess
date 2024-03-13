using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour {
   
}

public enum CharacterSlot {
    CharacterSlot0,
    CharacterSlot1,
    CharacterSlot2,
    CharacterSlot3,
    CharacterSlot4,
    CharacterSlot5,
    CharacterSlot6,
    CharacterSlot7,
    CharacterSlot8,
    CharacterSlot9,
    NO_SLOT
}

public enum WeaponModelSlot {
    RightHand,
    LeftHand,
}

//CALCULATE DAMAGE BASED ON ATTACK TYPE
public enum AttackType {
    SwiftAttack01,
    SwiftAttack02,
    StrongAttack01,
}
