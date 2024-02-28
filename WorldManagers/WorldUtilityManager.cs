using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUtilityManager : MonoBehaviour {

    public static WorldUtilityManager singleton;

    [SerializeField] LayerMask characterLayers;
    [SerializeField] LayerMask environmentLayers;

    void Awake() {
    if(singleton == null) {singleton = this;}
    else Destroy(gameObject);
    }

    public LayerMask GetCharacterLayers() {
        return characterLayers;
    }

    public LayerMask GetEnvironmentLayers() {
        return environmentLayers;
    }

}
