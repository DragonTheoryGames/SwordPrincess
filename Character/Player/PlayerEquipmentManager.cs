using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager {

    PlayerManager player;
    
    [SerializeField] WeaponManager weaponManager;
    public WeaponInstantiationSlot rightHandSlot;
    public GameObject weaponModel;

    protected override void Awake() {
        base.Awake();
        player = GetComponent<PlayerManager>();

        InitializeWeapons();
    }

    protected override void Start(){
        base.Start();
        LoadWeapon();
    }

    void InitializeWeapons() {
        WeaponInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots) {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand) {
                rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeapon() {
        if(player.playerInventoryManager.currentWeapon != null) {
            //CLEARWEAPON
            weaponModel = Instantiate(player.playerInventoryManager.currentWeapon.weaponModel);
            rightHandSlot.LoadWeapon(weaponModel);
            weaponManager = weaponModel.GetComponent<WeaponManager>();
            weaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentWeapon);
            //Assign Weapon Damage to Collider
        }
    }

    public void SwitchWeapon() {
        if(player.IsOwner) {
            //WeaponItem selectedWeapon = null;
            // CALL SELECTED WEAPON FROM INDEX ARRAY EP20
            // PLAY WEAPON ANIMATION
            // ENABLE WEAPON EFFECT
            // RETURN TO PRIMARY WEAPON
        }
    }

    //DAMAGE COLLIDERS
    public void EnableDamageCollider() {
        weaponManager.damageCollider.EnableDamageCollider();
        //PLAY WHOOSH
    }

    public void DisableDamageCollider() {
        weaponManager.damageCollider.DisableDamageCollider();
    }
}
