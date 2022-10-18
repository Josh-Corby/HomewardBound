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
    private GameObject zoomLook;
    [SerializeField]
    private GameObject CameraTarget;
    [SerializeField]
    Vector3 Offset;

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
        vcam.Follow = CameraTarget.transform;
        vcam.LookAt = CameraTarget.transform;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            //CameraTarget = zoomLook;
            camfollow.ShoulderOffset = new Vector3(1.37f, -0.4f, 0f);
            
        }

        if (!Input.GetKey(KeyCode.Mouse1))
        {
            //CameraTarget = cameraLook;
            camfollow.ShoulderOffset = new Vector3()
        }

        if (UI.menu != Menus.None || UI.paused)
            return;
        transform.position = thirdPersonPlayer.transform.position + Offset;
        //RotateCamera();
    }

    //public void RotateCamera()
    //{
    //    turn.x += Input.GetAxis("Mouse X");
    //    turn.y += Input.GetAxis("Mouse Y");
    //    transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    //}

    private void LateUpdate()
    {
        if (!UI.paused)
            CameraRotation();
    }
    private void CameraRotation()
    {
        //CameraTarget = UI.menu == Menus.Radial? null: cameraLook;

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
