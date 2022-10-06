using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransform : GameBehaviour
{
    public GameObject thirdPersonPlayer;
    private GameObject CameraTarget;
    Vector3 yOffset = new Vector3 (0, 0.58f, 0);

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
        CameraTarget = GameObject.Find("CameraLook");
    }

    private void Start()
    {
        _cinemachineTargetYaw = CameraTarget.transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        if (UI.menu != Menus.None || UI.paused)
            return;
        transform.position = thirdPersonPlayer.transform.position + yOffset;
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
        CameraRotation();
    }
    private void CameraRotation()
    {
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
