using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat
{

    public class CatDetection : GameBehaviour
    {
        public GameObject player;
        public LayerMask mask;
        public bool raycasting;
        public CatManager catManager;

        public float detectionTimer;
        private readonly float detectionTimerMax = 2f;
        private float detectionRange;

        private void Awake()
        {
            detectionRange = catManager.GetComponent<SphereCollider>().radius;
        }

        private void Start()
        {
            detectionTimer = detectionTimerMax;
        }

        private void Update()
        {
            detectionTimer = Mathf.Clamp(detectionTimer, 0, detectionTimerMax);

            if (raycasting)
            {
                catManager.LookAtPlayer();
                catManager.aiState = AIStates.Detecting;
                //transform.LookAt(player.transform);
                //Ray catRay = new Ray(transform.position, player.transform.position);

                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detectionRange, mask))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        if (catManager.aiState == AIStates.Aggro)
                        {
                            return;
                        }
                        if (detectionTimer >= 0)
                        {
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

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Player"))
        //    {
        //        GM.RespawnPlayer();
        //        catManager.RestartPath();
        //    }           
        //}
    }
}