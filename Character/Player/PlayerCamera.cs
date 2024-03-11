using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.TextCore.Text;

public class PlayerCamera : MonoBehaviour {

    public static PlayerCamera singleton;
    public Camera cameraMain;
    public PlayerManager player;
    [SerializeField] Transform cameraPivotTransform;

    [Header("Camera Settings")]
    float cameraSmothSpeed = 1;
    [SerializeField] float horizontalRotationSpeed = 220;
    [SerializeField] float verticalRotationSpeed = 220;
    [SerializeField] float minPivot = -30;
    [SerializeField] float maxPivot = 60;
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collisionLayers;

    [Header("Camera Values")]
    Vector3 cameraVelocity;
    Vector3 cameraMainPosition;
    [SerializeField] float horizontalAngle;
    [SerializeField] float verticalAngle;
    float CameraZPosition;
    float targetCameraZPosition;

    [Header("Lock On")]
    [SerializeField] float lockOnRadius = 20f;
    [SerializeField] float minimumViewableAngle = -50f;
    [SerializeField] float maximumViewableAngle = 50f;
    [SerializeField] float lockOnFollowSpeed = 0.2f;
    [SerializeField] float unlockedCameraHeight = 1.65f;
    [SerializeField] float lockedCameraHeight = 2.0f;
    [SerializeField] float setCameraHeightSpeed = 1;
    Coroutine cameraLockOnHeightCoroutine;
    List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestTarget;
    public CharacterManager leftNearestTarget;
    public CharacterManager rightNearestTarget;
    

    void Awake() {
        if(singleton == null) {singleton = this;}
        else Destroy(gameObject);
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
        CameraZPosition = cameraMain.transform.localPosition.z;
    }

    public void CameraActions(){
        if (player != null) {
            FollowTarget();
            Rotation();
            Collisions();
        }
        
    }

    void FollowTarget() {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    void Rotation() {
        if (player.playerNetworkManager.isLockedOn.Value) { //LOCKED ON ROTATION
            Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
            rotationDirection.Normalize();
            rotationDirection.y = 0;

            //Y ROTATION
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnFollowSpeed);

            //Y ROTATION
            rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
            rotationDirection.Normalize();
            cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnFollowSpeed);

            targetRotation = Quaternion.LookRotation(rotationDirection);

            //SAVE ROTATION TO LOOK ANGLE
            horizontalAngle = transform.eulerAngles.y;
            verticalAngle = transform.eulerAngles.x;
        }
        else { //NORMAL ROTATION
        horizontalAngle += PlayerInputManager.singleton.cameraHorizontalInput * horizontalRotationSpeed * Time.deltaTime;
        verticalAngle += PlayerInputManager.singleton.cameraVerticalInput * verticalRotationSpeed * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle, minPivot, maxPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        cameraRotation.y = horizontalAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.x = -verticalAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
        }
    }

    void Collisions() {
        targetCameraZPosition = CameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraMain.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collisionLayers)){
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius) {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraMainPosition.z = Mathf.Lerp(cameraMain.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraMain.transform.localPosition = cameraMainPosition;
    }

    public void TargetLockOn() {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceLeft = -Mathf.Infinity;
        float shortestDistanceRight = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 
                                                    lockOnRadius, 
                                                    WorldUtilityManager.singleton.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++) {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();
            if (lockOnTarget != null) {
                //Check if within feild of view
                Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - player.transform.position;
                float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetsDirection, cameraMain.transform.forward);

                if (lockOnTarget.isDead) { continue; }
                if (lockOnTarget.transform.root == player.transform.root) { continue; }

                if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle) {
                    RaycastHit hit;
                    if(Physics.Linecast(player.playerCombatManager.lockOnTransform.position, 
                                        lockOnTarget.characterCombatManager.lockOnTransform.position, 
                                        out hit, 
                                        WorldUtilityManager.singleton.GetEnvironmentLayers())) {
                        continue;
                    }
                    else {
                        availableTargets.Add(lockOnTarget);
                    }
                }
            }
        }

        //Sort through potential targets
        for (int i = 0; i < availableTargets.Count; i++) {
            if (availableTargets[i] != null) {
                float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[i].transform.position);
                Vector3 targetDirection = availableTargets[i].transform.position - player.transform.position;

                if (distanceFromTarget < shortestDistance) {
                    shortestDistance = distanceFromTarget;
                    nearestTarget = availableTargets[i];
                }

                if (player.playerNetworkManager.isLockedOn.Value) {
                    Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[i].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    if (availableTargets[i] == player.playerCombatManager.currentTarget) {continue;}
                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceLeft) {
                        shortestDistanceLeft = distanceFromLeftTarget;
                        leftNearestTarget = availableTargets[i];
                    }
                    else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceRight) {
                        shortestDistanceRight = distanceFromRightTarget;
                        rightNearestTarget = availableTargets[i];
                    }
                }
            }
            else {
                ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;
            }
        }
    }

    public void SetLockCameraHeight() {
        if (cameraLockOnHeightCoroutine != null) {
            StopCoroutine(cameraLockOnHeightCoroutine);
        }

        cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
    }

    public void ClearLockOnTargets() {
        nearestTarget = null;
        leftNearestTarget = null;
        rightNearestTarget = null;
        availableTargets.Clear();
    }

    public IEnumerator WaitThenFIndNewTargets() {
        while (player.isPerformingAction) {
            yield return null;
        }
        
        ClearLockOnTargets();
        TargetLockOn();

        if (nearestTarget != null) {
            player.playerCombatManager.SetTarget(nearestTarget);
            player.playerNetworkManager.isLockedOn.Value = true;
        }

        yield return null;
    }

    IEnumerator SetCameraHeight() {
        float duration = 1;
        float timer = 0;

        Vector3 velocity = Vector3.zero;
        Vector3 newLockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight);
        Vector3 newUnlockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

        while (timer < duration) {
            timer += Time.deltaTime;

            if (player != null) {
                if (player.playerCombatManager.currentTarget != null) {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    cameraPivotTransform.transform.localRotation = Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0,0,0), lockOnFollowSpeed);
                }
                else {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed); 
                }
            }
            yield return null;
        }

        if (player != null) {
            if (player.playerCombatManager.currentTarget != null) {
                    cameraPivotTransform.transform.localPosition = newLockedCameraHeight;
                    cameraPivotTransform.transform.localRotation = Quaternion.Euler(0,0,0);
            }
            else {
                cameraPivotTransform.transform.localPosition = newUnlockedCameraHeight;
            }
        }

        yield return null;
    }
}