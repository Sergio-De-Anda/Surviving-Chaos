using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    PlayerInput playerInput;
    public Transform target;

    public LayerMask wallLayers;

    [System.Serializable]
    public class CameraSettings
    {
        [Header("-Positioning-")]
        public Vector3 camPositionOffsetRight;

        [Header("-Camera Options-")]
        public Camera UICamera;
        public float mouseXSensitivity = 5.0f;
        public float mouseYSensitivity = 5.0f;
        public float minAngle = -30.0f;
        public float maxAngle = 70.0f;
        public float rotationSpeed = 5.0f;
        public float maxCheckDist = 0.1f;

        [Header("-Zoom-")]
        public float fieldOfView = 70.0f;
        public float zoomFieldOfView = 30.0f;
        public float zoomSpeed = 3.0f;

        [Header("-Visual Options-")]
        public float hideMeshWhenDistance = 0.5f;
    }
    [SerializeField]
    public CameraSettings cameraSettings;


    [System.Serializable]
    public class MovementSettings
    {
        public float movementLerpSpeed = 5.0f;
    }
    [SerializeField]
    public MovementSettings movement;

    float newX = 0.0f;
    float newY = 0.0f;

    public Camera mainCamera { get; protected set; }
    public Transform pivot { get; set; }

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
        pivot = transform.GetChild(0);
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        CheckWall();
        CheckMeshRenderer();
        Zoom(playerInput.Aim);
        RotatePlayerWithCamera();
    }

    void LateUpdate()
    {
        Vector3 targetPostion = target.position;
        Quaternion targetRotation = target.rotation;

        FollowTarget(targetPostion, targetRotation);
    }

    //Following the target with Time.deltaTime smoothly
    void FollowTarget(Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movement.movementLerpSpeed);
        transform.position = newPos;

    }

    //Rotates the camera with input
    void RotateCamera()
    {
        newX += cameraSettings.mouseXSensitivity * playerInput.mouseX;
        newY += cameraSettings.mouseYSensitivity * -playerInput.mouseY;

        Vector3 eulerAngleAxis = new Vector3();
        eulerAngleAxis.x = newY;
        eulerAngleAxis.y = newX;

        newX = Mathf.Repeat(newX, 360);
        newY = Mathf.Clamp(newY, cameraSettings.minAngle, cameraSettings.maxAngle);

        Quaternion newRotation = Quaternion.Slerp(pivot.localRotation, Quaternion.Euler(eulerAngleAxis), Time.deltaTime * cameraSettings.rotationSpeed);

        pivot.localRotation = newRotation;
    }

    //Checks the wall and moves the camera up if we hit
    void CheckWall()
    {

        RaycastHit hit;

        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 pivotPos = pivot.position;

        Vector3 start = pivotPos;
        Vector3 dir = mainCamPos - pivotPos;

        float dist = Mathf.Abs(cameraSettings.camPositionOffsetRight.z);

        if (Physics.SphereCast(start, cameraSettings.maxCheckDist, dir, out hit, dist, wallLayers))
        {
            MoveCamUp(hit, pivotPos, dir, mainCamT);
        }
        else
        {
            PostionCamera(cameraSettings.camPositionOffsetRight);
        }
    }

    //This moves the camera forward when we hit a wall
    void MoveCamUp(RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform cameraT)
    {
        float hitDist = hit.distance;
        Vector3 sphereCastCenter = pivotPos + (dir.normalized * hitDist);
        cameraT.position = sphereCastCenter;
    }

    //Postions the cameras localPosition to a given location
    void PostionCamera(Vector3 cameraPos)
    {
        if (!mainCamera)
            return;

        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.localPosition;
        Vector3 newPos = Vector3.Lerp(mainCamPos, cameraPos, Time.deltaTime * movement.movementLerpSpeed);
        mainCamT.localPosition = newPos;
    }

    //Hides the mesh targets mesh renderers when too close
    void CheckMeshRenderer()
    {

        SkinnedMeshRenderer[] meshes = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 targetPos = target.position;
        float dist = Vector3.Distance(mainCamPos, (targetPos + target.up));

        if (meshes.Length > 0)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                if (dist <= cameraSettings.hideMeshWhenDistance)
                {
                    meshes[i].enabled = false;
                }
                else
                {
                    meshes[i].enabled = true;
                }
            }
        }
    }

    //Zooms the camera in and out
    void Zoom(bool isZooming)
    {

        if (isZooming)
        {
            float newFieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.zoomFieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
            mainCamera.fieldOfView = newFieldOfView;
        }
        else
        {
            float originalFieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.fieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
            mainCamera.fieldOfView = originalFieldOfView;
        }
    }

    void RotatePlayerWithCamera()
    {
        if (playerInput.Vertical != 0.0f || playerInput.Horizontal != 0.0f || playerInput.Aim)
        {
            Vector3 pivotPos = pivot.position;
            Vector3 lookTarget = pivotPos + (pivot.forward * 120.0f);
            Vector3 thisPos = target.transform.position;
            Vector3 lookDir = lookTarget - thisPos;

            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            lookRot.x = 0;
            lookRot.z = 0;

            Quaternion newRotation = Quaternion.Lerp(target.transform.rotation, lookRot, Time.deltaTime * 10.0f);
            target.transform.rotation = newRotation;
        }
    }
}
