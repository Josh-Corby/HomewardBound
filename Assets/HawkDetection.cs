using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkDetection : MonoBehaviour
{
    public GameObject detectionCircle;
    bool isGrowing;
    public Vector3 minScale;
    public Vector3 maxScale;
    float lerpTime = 0.3f;

    private void Start()
    {
        isGrowing = false;
    }
    private void Update()
    {
        if (isGrowing)
        {
            detectionCircle.transform.localScale = Vector3.Lerp(detectionCircle.transform.localScale, maxScale,
                lerpTime * Time.deltaTime);
        }
        if (!isGrowing)
        {
            detectionCircle.transform.localScale = Vector3.Lerp(detectionCircle.transform.localScale, minScale,
                lerpTime * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            isGrowing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player not detected");
            isGrowing = false;
        }
    }
}
