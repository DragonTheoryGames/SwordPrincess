using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
     
    GameManager singleton;

    void Awake() {
        if(singleton = null) singleton = this;
        else Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        Application.targetFrameRate = 120;
    }
}
