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
                _camera.camfollow.CameraSide = Mathf.Lerp(_camera.camfollow.CameraSide, _cameraSideExitValue, 0.1f);
                return;
            }
        }
        if (triggerState)
        {

            waitTimer = waitTimerMax;
            _camera.camfollow.CameraSide = Mathf.Lerp(_camera.camfollow.CameraSide, _cameraSideEnterValue, 0.1f);
        }

    }

    //public IEnumerator LerpCameraSide(float value)
    //{
    //    float timeElapsed = 0;
    //    float lerpDuration = 1.5f;
    //    while (timeElapsed < lerpDuration)
    //    {
    //        camfollow.CameraSide = Mathf.Lerp(camfollow.CameraSide, value, timeElapsed / lerpDuration);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    camfollow.CameraSide = value;
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            triggerState = true;
            Debug.Log("Camera side changing");
            //StartCoroutine(CanLerp());
            //StartCoroutine(DisableCollider());
            //StopCoroutine(_camera.LerpCameraSide(_cameraSideEnterValue));
            //StartCoroutine(_camera.LerpCameraSide(_cameraSideEnterValue));
            //_camera.ChangeCameraSide(_CameraSideEnterValue);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            triggerState = false;
            Debug.Log("Camera side changing");
            ////StartCoroutine(DisableCollider());
            //StopCoroutine(_camera.LerpCameraSide(_cameraSideExitValue));
            //StartCoroutine(_camera.LerpCameraSide(_cameraSideExitValue));
            //_camera.ChangeCameraSide(_CameraSideEnterValue);
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
