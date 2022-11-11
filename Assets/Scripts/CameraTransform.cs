using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransform : GameBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private CinemachineVirtualCamera vcam;
    private Cinemachine3rdPersonFollow camfollow;


    private float cameraDistance;
    private const float _threshold = 0.01f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [SerializeField]
    private bool isColliding;

    [SerializeField]
    private LayerMask mask;



    [SerializeField]
    private Transform cameraLookRight;
    [SerializeField]
    private Transform cameraLookLeft;
    private void Awake()
    {
        camfollow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void Start()
    {
        _cinemachineTargetYaw = gameObject.transform.rotation.eulerAngles.y;
        cam = Camera.main;
    }

    private void Update()
    {
        cameraDistance = 10f;
        camfollow.CameraDistance = cameraDistance;
        if (vcam.Follow == null) return;
        if (vcam.LookAt == null) return;


        //vcam.Follow = gameObject.transform;
       //vcam.LookAt = gameObject.transform;

        if (UI.menu != Menus.None || UI.paused)
            return;
       // CameraPosition();
    }
    private void LateUpdate()
    {
        if (!UI.paused)
        {
            CameraRotation();
            
        }
    }

    public void SetCameraTarget(Transform target)
    {
        vcam.Follow = target;
        vcam.LookAt =target;
    }
    private void CameraRotation()
    {
        //CameraTarget = UI.menu == Menus.Radial? null: cameraLook;
        //vcam.LookAt = UI.menu == Menus.Radial ? null : gameObject.transform;
       // vcam.Follow = UI.menu == Menus.Radial ? null : gameObject.transform;

        if (gameObject == null) return;
        // if there is an input and camera position is not fixed
        if (IM.cameraInput.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = 0.1f;

            _cinemachineTargetYaw += IM.cameraInput.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += -IM.cameraInput.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        gameObject.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void CameraPosition()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 1, mask))
        {
            Debug.Log(hit.collider.gameObject);
            transform.position = hit.transform.position;
        }
    }



    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("camera right");
    //    SetCameraTarget(cameraLookLeft);
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("Camera left");
    //    SetCameraTarget(cameraLookRight);
    //}
}
