using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum Side
{
    Left,
    Right
}
public class CameraSideChangeCollision : MonoBehaviour
{
    [SerializeField]
    private Side _side;
    private float _cameraSideEnterValue = 0;
    private float _cameraSideExitValue = 1;

    [SerializeField]
    private CameraTransform _camera;

    public bool triggerState;
    public bool canLerp;
    private CapsuleCollider col;
    private float waitTimer;
    private float waitTimerMax = 0.5f;

    private void Awake()
    {
        col = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        //if (_side == Side.Left)
        //{
        //    _CameraSideValue = 1;

        //}

        //if (_side == Side.Right)
        //{
        //    _CameraSideValue = 0;
        //}
        waitTimer = waitTimerMax;
    }

    private void Update()
    {
        if (!triggerState)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                _camera.camfollow.CameraSide = Mathf.Lerp(_camera.camfollow.CameraSide, _cameraSideExitValue, 0.03f);
                return;
            }
        }
        if (triggerState)
        {

            waitTimer = waitTimerMax;
            _camera.camfollow.CameraSide = Mathf.Lerp(_camera.camfollow.CameraSide, _cameraSideEnterValue, 0.03f);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            triggerState = true;
            Debug.Log("Camera side changing");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            triggerState = false;
            Debug.Log("Camera side changing");

        }
    }

    private IEnumerator CanLerp()
    {
        yield return new WaitForSeconds(2f);
        canLerp = true;
    }


    private IEnumerator DisableCollider()
    {
        col.enabled = false;
        yield return new WaitForSeconds(1.5f);
        //triggerState = false;
        col.enabled = true;
        triggerState = false;
    }
}
