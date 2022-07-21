using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatDetection : GameBehaviour
{
    public GameObject player;
    public LayerMask mask;
    public bool raycasting;
    public CatManager catManager;

    public float detectionTimer;
    private float detectionTimerMax = 2f;

    private void Start()
    {
        detectionTimer = detectionTimerMax;
    }

    private void Update()
    {
        

        detectionTimer = Mathf.Clamp(detectionTimer, 0, detectionTimerMax);

        if (raycasting)
        {
            transform.LookAt(player.transform);
            //Ray catRay = new Ray(transform.position, player.transform.position);

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 15f, mask))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (catManager.aiState == AIStates.Aggro)
                    {
                        return;
                    }

                    if (detectionTimer >= 0)
                    {
                        catManager.aiState = AIStates.Detecting;
                        Debug.DrawLine(transform.position, hit.point, Color.green);
                        detectionTimer -= Time.deltaTime;
                    }
                    if (detectionTimer <= 0)
                    {
                        Debug.Log("PlayerDetected");
                        Debug.DrawLine(transform.position, hit.point, Color.red);
                        catManager.aiState = AIStates.Aggro;
                    }
                }
                else
                {
                    catManager.aiState = AIStates.Walk;
                    detectionTimer = detectionTimerMax;
                }
                    
            }
        }
        if (!raycasting)
        {
            detectionTimer = detectionTimerMax;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GM.RespawnPlayer();
        }
    }
}
