using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CameraSideChangeCollision : MonoBehaviour
{

    private float _cameraSideEnterValue = 0;
    private float _cameraSideExitValue = 1;

    [SerializeField]
    private CameraTransform _camera;

    public bool triggerState;
    public bool canLerp;

    private float waitTimer;
    private float waitTimerMax = 0.5f;
    private float lerpAmount = 0.005f;


    private void Start()
    {
        waitTimer = waitTimerMax;
    }

    private void Update()
    {
        if (!triggerState)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                _camera.camfollow.CameraSide = Mathf.Lerp(_camera.camfollow.CameraSide, _cameraSideExitValue, lerpAmount);
                return;
            }
        }

        if (triggerState)
        {

            waitTimer = waitTimerMax;
            _camera.camfollow.CameraSide = Mathf.Lerp(_camera.camfollow.CameraSide, _cameraSideEnterValue, lerpAmount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            triggerState = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            triggerState = false;
        }
    }
}
