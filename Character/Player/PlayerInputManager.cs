using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour {

    public static PlayerInputManager singleton;
    public PlayerManager player;
    PlayerControls playerControls;

    [Header("Movement Controls")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveInput;
    public float moveAmount;

    [Header("Camera Controls")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("Lock On Input")]
    [SerializeField] bool lockOnInput;
    [SerializeField] bool lockOnLeftInput;
    [SerializeField] bool lockOnRightInput;
    Coroutine lockOnCoroutine;

    [Header("Action Controls")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool swiftAttackInput = false;

    void Awake() {
        if(singleton == null) {singleton = this;}
        else Destroy(gameObject);
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChange;
        singleton.enabled = false;
        if (playerControls != null) { playerControls.Disable(); }
        
    }

    void OnEnable() {
        if(playerControls == null) { 
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.CameraControls.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.QuickAttack.performed += i => swiftAttackInput = true;
            playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
            playerControls.PlayerActions.LockOnLeft.performed += i => lockOnLeftInput = true;
            playerControls.PlayerActions.LockOnRight.performed += i => lockOnRightInput = true;
        }

        playerControls.Enable();
    }

    void Update() {
        HandleAllInputActions();
    }

    void HandleAllInputActions(){
        MovementInput();
        CameraInput();
        DodgeInput();
        SprintInput();
        JumpInput();
        SwiftAttackInput();
        LockOnInput();
        LockOnSwitchInput();
    }

    void OnApplicationFocus(bool focus) {
        if (enabled) {
            if (focus) {
                playerControls.Enable();
            }
            else playerControls.Disable();
        }

    }

    void OnSceneChange(Scene oldScene, Scene newScene) {
        singleton.enabled = newScene.buildIndex == WorldSaveGameManager.singleton.GetWorldSceneIndex() ? true : false;
        if (playerControls != null) {playerControls.Enable();}
    }

    void MovementInput() {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveInput = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
        moveAmount = moveInput;

        if (moveAmount <= .15f && moveAmount > 0) {moveAmount = .0f;}
        if (player.playerNetworkManager.isSprinting.Value) {moveAmount = 2;}
        //else if (moveAmount > .5f && moveAmount <= 1) {moveAmount = 1;} // remove movement walk/run animation blend

        //IF LOCKED ON
        if (player == null) {return;}

        if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value) {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }
        else {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput);
        }
        
        
    }

    void CameraInput(){
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    void DodgeInput() {
        if(dodgeInput) {
            dodgeInput = false;
            player.playerLocomotionManager.PerformDodge();
        }
    }

    void SprintInput() {
            player.playerLocomotionManager.PerformSprint(sprintInput, moveInput);
    }

    void JumpInput() {
        if (jumpInput) {
            jumpInput = false;
            player.playerLocomotionManager.PerformJump();
        }
    }

    void SwiftAttackInput() {
        if (swiftAttackInput) { 
            swiftAttackInput = false; 
            player.playerCombatManager.PerformWeaponAction(player.playerInventoryManager.currentWeapon.swiftAttack, player.playerInventoryManager.currentWeapon);
        }
    }

    void LockOnInput(){
        if (player.playerNetworkManager.isLockedOn.Value) {
            if (player.playerCombatManager.currentTarget == null) {return;}
            
            if (player.playerCombatManager.currentTarget.isDead) {
                player.playerNetworkManager.isLockedOn.Value = false;
            }
            //Attempt to find new target or unlock completely
            //THIS ASSURES US THE COROUTINE ONLY RUNES ONCE AT A TIME
            if (lockOnCoroutine != null) {
                StopCoroutine(lockOnCoroutine);
                lockOnCoroutine = StartCoroutine(PlayerCamera.singleton.WaitThenFIndNewTargets());
            }
        }
        if (lockOnInput && player.playerNetworkManager.isLockedOn.Value) {
            lockOnInput = false;
            PlayerCamera.singleton.ClearLockOnTargets();
            player.playerNetworkManager.isLockedOn.Value = false;
            //Disable Lock On.
            return;
        }
        if (lockOnInput && !player.playerNetworkManager.isLockedOn.Value) {
            lockOnInput = false; 
            //Enable Lock On
            PlayerCamera.singleton.TargetLockOn();

            if(PlayerCamera.singleton.nearestTarget != null) {
                //TODO Set as target
                player.playerCombatManager.SetTarget(PlayerCamera.singleton.nearestTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }
            return;
        }
    }

    void LockOnSwitchInput() {
        if (lockOnLeftInput) {
            lockOnLeftInput = false;
            if (player.playerNetworkManager.isLockedOn.Value) {
                PlayerCamera.singleton.TargetLockOn();
                if(PlayerCamera.singleton.leftNearestTarget != null) {
                    player.playerCombatManager.SetTarget(PlayerCamera.singleton.leftNearestTarget);
                }
            }
        }

        if (lockOnRightInput) {
            lockOnRightInput = false;
            if (player.playerNetworkManager.isLockedOn.Value) {
                PlayerCamera.singleton.TargetLockOn();
                if(PlayerCamera.singleton.rightNearestTarget != null) {
                    player.playerCombatManager.SetTarget(PlayerCamera.singleton.rightNearestTarget);
                }
            }
        }
    }

    void OnDestroy(){
        SceneManager.activeSceneChanged += OnSceneChange;
    }



}
