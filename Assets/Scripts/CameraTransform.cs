using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransform : GameBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera vcam;


    private Cinemachine3rdPersonFollow camfollow;

    public GameObject thirdPersonPlayer;
    [SerializeField]
    private GameObject cameraLook;
    [SerializeField]
    private GameObject CameraTarget;
    [SerializeField]
    Vector3 Offset;

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

    private void Awake()
    {
        CameraTarget = cameraLook;
        camfollow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void Start()
    {
        _cinemachineTargetYaw = CameraTarget.transform.rotation.eulerAngles.y;
    }

    private void Update()
    {


        cameraDistance = Mathf.Clamp(cameraDistance,10f, 20f);
        camfollow.CameraDistance = cameraDistance;
        if (vcam.Follow == null) return;
        if (vcam.LookAt == null) return;


        vcam.Follow = CameraTarget.transform;
        vcam.LookAt = CameraTarget.transform;
  
        if (UI.menu != Menus.None || UI.paused)
            return;


        //if (Input.mouseScrollDelta.y < 0)
        //{
        //    cameraDistance += 2;
        //}

        //if(Input.mouseScrollDelta.y >0)
        //{
        //    cameraDistance -= 2;
        //}
    }
    private void LateUpdate()
    {
        if (!UI.paused)
            CameraRotation();
    }
    private void CameraRotation()
    {
        //CameraTarget = UI.menu == Menus.Radial? null: cameraLook;
        vcam.LookAt = UI.menu == Menus.Radial ? null : cameraLook.transform;
        vcam.Follow = UI.menu == Menus.Radial ? null : cameraLook.transform;

        if (CameraTarget == null) return;
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
        CameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
