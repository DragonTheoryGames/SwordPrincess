using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityDestroyAfterTime : MonoBehaviour {
    [SerializeField] float timeUntilDestroyed = 5;

    void Awake() {
        Destroy(this, timeUntilDestroyed);
    }
}
