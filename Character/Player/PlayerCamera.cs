using UnityEngine;

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
        //LOCK ON
        //ELSE ROTATE ON PLAYER

        //NORMAL ROTATION
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
}
