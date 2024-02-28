using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldItemDatabase : MonoBehaviour {
    
    public static WorldItemDatabase Singleton;

    List<Item> items = new List<Item>();

    [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();
    //LIST ALCHEMY
    //LIST TALISMANS
    //LIST SPELLS
    

    void Awake() {
        if (Singleton == null) {Singleton = this;}
        else Destroy(this);

        foreach (WeaponItem weapon in weapons) {
            items.Add(weapon);
        }

        for (int i = 0; i < items.Count; i++) {
            items[i].itemID = i;
        }
    }

    public WeaponItem GetWeaponByID(int ID) {
        return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
    }

}
