using System;
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
        SwiftAttack();
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

        //IF NOT LOCKED ON
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        //APPLY MOVEMENT WITH STRAFE
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

    void SwiftAttack() {
        if (swiftAttackInput) { 
            swiftAttackInput = false; 
            player.playerCombatManager.PerformWeaponAction(player.playerInventoryManager.currentWeapon.swiftAttack, player.playerInventoryManager.currentWeapon);
        }
    }

    void OnDestroy(){
        SceneManager.activeSceneChanged += OnSceneChange;
    }



}
