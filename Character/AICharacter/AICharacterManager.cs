using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class AICharacterManager : CharacterManager {

    [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
    [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;

    [Header("Navmesh Agent")]
    public NavMeshAgent navMeshAgent;

    [Header("States")]
    [SerializeField] AIState currentState;
    public IdleState idle;
    public PursueTargetState pursueTarget;
    // COMBAT STATE
    // ATTACK STATE

    protected override void Awake() {
        base.Awake();

        aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        idle = Instantiate(idle);
        pursueTarget = Instantiate(pursueTarget);

        currentState = idle;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (IsOwner) {
            ProcessStateMachine();
        }
    }

    void ProcessStateMachine() {
        AIState nextState = currentState?.Tick(this);

        if (nextState != null) {
            currentState = nextState;
        }

        //RESETPOSITION?ROTATION AFTER THE STATE MACHINE HAS PROCESSED ITS TICK
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if (aiCharacterCombatManager.currentTarget != null) {
            aiCharacterCombatManager.targetDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
            aiCharacterCombatManager.viewableAngle = WorldUtilityManager.singleton.GetAngleOfTarget(transform, aiCharacterCombatManager.targetDirection);
        }

        if (navMeshAgent.enabled) {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navMeshAgent.stoppingDistance) {
                aiCharacterNetworkManager.isMoving.Value = true;
            }
            else {
                aiCharacterNetworkManager.isMoving.Value = false;
            }
        }
        else aiCharacterNetworkManager.isMoving.Value = false;
    }
}
