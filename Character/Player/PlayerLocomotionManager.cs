using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager {

    PlayerManager player;
    PlayerAnimatorManager playerAnimatorManager;

    // FROM INPUTMANAGER
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement")]
    Vector3 moveDirection;
    Vector3 targetRotation;
    float walkingSpeed = 2f;
    float runningSpeed = 2f;
    float rotationSpeed = 15;
    //float backflipSpeed = -2;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 2;
    [SerializeField] float jumpingSpeed = 3;
    [SerializeField] float freeFallMoveSpeed = 2;
    Vector3 jumpDirection;

    [Header("Dodge")]
    Vector3 rollDirection;

    [Header("Stamina")]
    [SerializeField] int sprintingStaminaCost = 10;
    float currentSprintExpense = 0;
    [SerializeField] int dodgeStaminaCost = 25;
    [SerializeField] int jumpStaminaCost = 15;

    [Header("Strings")]
    string roll = "DodgeFoward";
    string backflip = "DodgeBack";
    string jump = "JumpStart";

    protected override void Awake() {
        base.Awake();

        player = GetComponent<PlayerManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    protected override void Update() {
        base.Update();
        if (player.IsOwner) {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.networkMoveAmount.Value = moveAmount;
        }
        else {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.networkMoveAmount.Value;

            if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value) {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
            }
            else {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalMovement, verticalMovement);
            }
        }
    }

    public void Movement() {
        GroundedMovement();
        Rotation();
        JumpingMovement();
        FreeFallMovement();
    }

    void GroundedMovement() {
        if (!player.canMove) {return;}
        if (!player.isGrounded) {return;}
        
        GetGroundedInput();
        moveDirection = PlayerCamera.singleton.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.singleton.transform.right * horizontalMovement;
        moveDirection.y = 0;
        moveDirection.Normalize();

        if (PlayerInputManager.singleton.moveAmount > 0.75f) {
            player.characterController.Move(runningSpeed * Time.deltaTime * moveDirection) ;
        }
        else if (PlayerInputManager.singleton.moveAmount <= 0.25f) {
             player.characterController.Move(walkingSpeed * Time.deltaTime * moveDirection) ;
        }
    }

    void GetGroundedInput() {
        verticalMovement = PlayerInputManager.singleton.verticalInput;
        horizontalMovement = PlayerInputManager.singleton.horizontalInput;
        moveAmount = PlayerInputManager.singleton.moveAmount;
    }    

    void JumpingMovement() {
        if (!player.playerNetworkManager.isJumping.Value) {return;}
        player.characterController.Move(jumpDirection * jumpingSpeed * Time.deltaTime);
    }

    void FreeFallMovement() {
        if (player.isGrounded) {return;}

        Vector3 freeFallDirection;
        freeFallDirection = PlayerCamera.singleton.transform.forward * PlayerInputManager.singleton.verticalInput;
        freeFallDirection += PlayerCamera.singleton.transform.right * PlayerInputManager.singleton.horizontalInput;
        freeFallDirection.y = 0;

        player.characterController.Move(freeFallDirection * freeFallMoveSpeed * Time.deltaTime);
    }

    void Rotation() {
        if (player.isDead) {return;}
        if (!player.canRotate) {return;}

        if (player.playerNetworkManager.isLockedOn.Value) { //LOCKED ON TO A TARGET
            if (player.playerNetworkManager.isSprinting.Value || player.playerLocomotionManager.isRolling) {
                Vector3 targetDirection = Vector3.zero;
                targetDirection = PlayerCamera.singleton.cameraMain.transform.forward * verticalMovement;
                targetDirection += PlayerCamera.singleton.cameraMain.transform.right * horizontalMovement;
                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero) {
                    targetDirection = transform.forward;
                }
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = finalRotation;
            }
            else { //NO TARGET
                if (player.playerCombatManager.currentTarget == null) { return; }

                Vector3 targetDirection;
                targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                targetDirection.Normalize();
                targetDirection.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = finalRotation;
            }
        }
        else {
            targetRotation = Vector3.zero;
            targetRotation = PlayerCamera.singleton.cameraMain.transform.forward * verticalMovement;
            targetRotation = targetRotation + PlayerCamera.singleton.cameraMain.transform.right * horizontalMovement;
            targetRotation.y = 0;
            targetRotation.Normalize();

            if (targetRotation == Vector3.zero) {targetRotation = transform.forward;}

            Quaternion newRotation = Quaternion.LookRotation(targetRotation);
            Quaternion newTargetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = newTargetRotation;
        }
    }

    public void PerformDodge(){
        if (player.isPerformingAction) {return;}
        if (player.playerNetworkManager.currentStamina.Value <= 0) {return;}
        if (player.playerNetworkManager.isJumping.Value) {return;}
        if (!player.isGrounded) {return;}

        if (moveAmount > 0) {
            rollDirection = PlayerCamera.singleton.cameraMain.transform.forward * verticalMovement;
            rollDirection += PlayerCamera.singleton.cameraMain.transform.right * horizontalMovement;

            rollDirection.y = 0;
            rollDirection.Normalize();
            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayAnimation(roll, true);
            player.playerLocomotionManager.isRolling = true;
        }
        else {
            player.playerAnimatorManager.PlayAnimation(backflip, true);
        }
        player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost; 
    }

    public void PerformSprint(bool SprintInput, float moveInput) {
        if (player.isPerformingAction) {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }
        if (player.playerNetworkManager.currentStamina.Value <= 0) {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }

        if (moveInput >= 1) {
            player.playerNetworkManager.isSprinting.Value = SprintInput;
        }
        else player.playerNetworkManager.isSprinting.Value = false;
        
        if (player.playerNetworkManager.isSprinting.Value) {
            currentSprintExpense += sprintingStaminaCost * Time.deltaTime;
        }
        if (currentSprintExpense > 1) {
                currentSprintExpense--;
                player.playerNetworkManager.currentStamina.Value--;
            }
    }

    public void PerformJump() {
        if (player.isPerformingAction) {return;}
        if (player.playerNetworkManager.currentStamina.Value <= 0) {return;}
        if (player.playerNetworkManager.isJumping.Value) {return;}
        if (!player.isGrounded) {return;}

        player.playerAnimatorManager.PlayAnimation(jump, false);
        ApplyJumpingVelocity();
        player.playerNetworkManager.isJumping.Value = true;
        player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

        jumpDirection = PlayerCamera.singleton.cameraMain.transform.forward *
                        PlayerInputManager.singleton.verticalInput;
        jumpDirection += PlayerCamera.singleton.cameraMain.transform.right *
                         PlayerInputManager.singleton.horizontalInput;
        jumpDirection.y = 0;
        
        if (jumpDirection == Vector3.zero) {return;}
        if (player.playerNetworkManager.isSprinting.Value) {jumpDirection *= 1;}
        else if (PlayerInputManager.singleton.moveAmount > 0.5) {jumpDirection *= .5f;}
        else if (PlayerInputManager.singleton.moveAmount <= 0.5) {jumpDirection *= .25f;}
    }

    public void ApplyJumpingVelocity() {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -gravity);
    }

}
