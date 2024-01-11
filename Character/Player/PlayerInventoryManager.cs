using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager {
    
    public WeaponItem currentWeapon;

    [Header("WeaponSlots")]
    public WeaponItem[] weapons = new WeaponItem[3];
    public int weaponIndex = 0;  //0=Sword, 1=Chakram, 2=Hammer, 3=Staff etc

    //[Header("Alchemy Slots")]
}
