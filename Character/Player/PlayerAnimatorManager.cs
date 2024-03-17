using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager {
    
    PlayerManager player;

    protected override void Awake() {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public override void EnableCombo() {
        player.playerCombatManager.canCombo = true;
    }

    public override void DisableCombo() {
        player.playerCombatManager.canCombo = false;
    }
}
