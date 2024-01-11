using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager {

    PlayerManager player;

    protected override void Awake() {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Start() {
        base.Start();

        // This is calculated here for when the character class is setup at start of game.
        CalcualteHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
        CalcualteStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
    }
}
