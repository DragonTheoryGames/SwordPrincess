using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AICharacterManager : CharacterManager {

    public AICharacterCombatManager aiCharacterCombatManager;
    [Header("Current State")]
    [SerializeField] AIState currentState;

    protected override void Awake() {
        base.Awake();

        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        ProcessStateMachine();
    }

    private void ProcessStateMachine() {
        AIState nextState = currentState?.Tick(this);

        if (nextState != null) {
            currentState = nextState;
        }
    }
}
