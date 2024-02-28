using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldActionManager : MonoBehaviour {
    
    public static WorldActionManager singleton;

    [Header("Weapon Item Actions")]
    public WeaponItemAction[] weaponItemAction;

    void Awake() {
        if(singleton == null) singleton = this;
        else Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        for (int i =0; i < weaponItemAction.Length; i++) {
            weaponItemAction[i].actionID = i;
        }
    }

    public WeaponItemAction GetWeaponItemAction(int ID) {
        return weaponItemAction.FirstOrDefault(action => action.actionID == ID);
    }
}
