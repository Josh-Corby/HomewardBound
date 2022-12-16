using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransform : GameBehaviour
{
    [SerializeField]
    private Camera _cam;
    [SerializeField]
    private CinemachineVirtualCamera _vcam;
    [HideInInspector]
    public Cinemachine3rdPersonFollow CamFollow;


    private float _cameraDistance;
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
    private LayerMask _mask;

    private void Awake()
    {
        _cam = Camera.main;
        CamFollow = _vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void Start()
    {
        _cinemachineTargetYaw = gameObject.transform.rotation.eulerAngles.y;
        _cam = Camera.main;
    }

    private void Update()
    {
        _cameraDistance = 10f;
        CamFollow.CameraDistance = _cameraDistance;
        if (_vcam.Follow == null) return;
        if (_vcam.LookAt == null) return;

        if (UI.menu != Menus.None || UI.paused)
            return;
        // CameraPosition();


    }
    private void LateUpdate()
    {
        CameraRotation();     
    }
    public void SetCameraTarget(Transform target)
    {
        _vcam.Follow = target;
        _vcam.LookAt = target;
    }

    public void LookAtPlayer()
    {
        _vcam.Follow = gameObject.transform;
        _vcam.LookAt = gameObject.transform;
    }

    public IEnumerator LerpCameraSide(float value)
    {
        float timeElapsed = 0.1f;
        float lerpDuration = 3f;
        while (timeElapsed < lerpDuration)
        {
            CamFollow.CameraSide = Mathf.Lerp(CamFollow.CameraSide, value, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        CamFollow.CameraSide = value;
    }

    public void ChangeCameraSide(float value)
    {
        CamFollow.CameraSide = Mathf.Lerp(CamFollow.CameraSide, value, 0.1f);

    }

    private void CameraRotation()
    {

        if(!UI.paused)
        {

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

        }
        // Cinemachine will follow this target
        gameObject.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void CameraPosition()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 1, _mask))
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
}
