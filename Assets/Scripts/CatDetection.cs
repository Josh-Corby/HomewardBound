using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatDetection : GameBehaviour
{
    Color startColor = Color.green;
    Color endColor = Color.red;
    float lerpTime = 1f;
    bool lerp = false;

    public float detectionTimer;
    private float timerMax = 3f;

    public MeshRenderer meshRenderer;
    [SerializeField] private GameObject player;

    private void Start()
    {
        detectionTimer = timerMax;
        meshRenderer.material.color = Color.green;
        
    }

    private void Update()
    {
        detectionTimer = Mathf.Clamp(detectionTimer, 0f, timerMax);

        if (lerp)
        {
            meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, endColor, Time.deltaTime * lerpTime);
            detectionTimer -= Time.deltaTime;
            if (detectionTimer <= 0)
            {
                Debug.Log("Player Caught!");
                GM.RespawnPlayer();
                detectionTimer = timerMax;
            }
        }
        if (!lerp)
        {
            meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, startColor, Time.deltaTime * lerpTime);
            detectionTimer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Detected");
            lerp = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player Not Detected");
            lerp = false;

        }
    }
}
