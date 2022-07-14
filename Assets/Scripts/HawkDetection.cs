using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkDetection : GameBehaviour
{
    public GameObject detectionCircle;
    public bool isGrowing;
    public Vector3 minScale;
    public Vector3 maxScale;
    float lerpTime = 0.3f;

    public float timer;
    float maxTimer = 10f;


    private void Start()
    {
        detectionCircle.transform.localPosition = new Vector3(0, 0, 0);
        isGrowing = false;
        timer = maxTimer;
    }
    private void Update()
    {
        timer = Mathf.Clamp(timer, 0, maxTimer);
        if (isGrowing)
        {
            detectionCircle.transform.localScale = Vector3.Lerp(detectionCircle.transform.localScale, maxScale,
                lerpTime * Time.deltaTime);
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                GM.RespawnPlayer();
                timer = maxTimer;
            }
        }
        if (!isGrowing)
        {
            timer += Time.deltaTime;
            detectionCircle.transform.localScale = Vector3.Lerp(detectionCircle.transform.localScale, minScale,
                lerpTime * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isGrowing = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectionCircle.transform.position = new Vector3( other.transform.position.x, other.transform.position.y+0.01f, other.transform.position.z);    
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isGrowing = false;
            detectionCircle.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
