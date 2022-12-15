using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CameraSideChangeCollision : MonoBehaviour
{

    private readonly float _cameraSideEnterValue = 0;
    private readonly float _cameraSideExitValue = 1;

    [SerializeField]
    private CameraTransform _camera;

    public bool TriggerState;
    public bool TanLerp;

    private float _waitTimer;
    private readonly float _waitTimerMax = 0.5f;
    private readonly float _lerpAmount = 0.005f;


    private void Start()
    {
        _waitTimer = _waitTimerMax;
    }

    private void Update()
    {
        if (!TriggerState)
        {
            _waitTimer -= Time.deltaTime;

            if (_waitTimer <= 0f)
            {
                _camera.CamFollow.CameraSide = Mathf.Lerp(_camera.CamFollow.CameraSide, _cameraSideExitValue, _lerpAmount);
                return;
            }
        }

        if (TriggerState)
        {

            _waitTimer = _waitTimerMax;
            _camera.CamFollow.CameraSide = Mathf.Lerp(_camera.CamFollow.CameraSide, _cameraSideEnterValue, _lerpAmount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            TriggerState = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            TriggerState = false;
        }
    }
}
