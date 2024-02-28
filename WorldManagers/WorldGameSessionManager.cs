using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameSessionManager : MonoBehaviour {

    public static WorldGameSessionManager singleton;
    
    [Header("Active Players in Session")]
    public List<PlayerManager> players = new List<PlayerManager>();

    void Awake() {
        if(singleton == null) {
            singleton = this;
        }
        else Destroy(this);
        DontDestroyOnLoad(this);
    }

    public void AddPlayer(PlayerManager player) {
        if (!players.Contains(player)) {
            players.Add(player);
        }

        // CHECK LIST FOR NULLS
        for (int i = players.Count - 1; i > -1; i--) {
            if (players[i] == null) {
                players.RemoveAt(i);
            }
        }
    }

    public void RemovePlayer(PlayerManager player) {
        if (!players.Contains(player)) {
            players.Remove(player);
        }

        // CHECK LIST FOR NULLS
        for (int i = players.Count - 1; i > -1; i--) {
            if (players[i] == null) {
                players.RemoveAt(i);
            }
        }
    }
}
