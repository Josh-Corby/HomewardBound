using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat
{

    public class CatDetection : GameBehaviour
    {
        private GameObject Player;
        public LayerMask mask;
        public bool raycasting;
        public CatManager catManager;
        [SerializeField]
        private BoxCollider col;

        [SerializeField]
        private float detectionTimer;
        [SerializeField]
        private float detectionTimerMax = 2f;
        private float detectionRange;

        [SerializeField]
        private bool isNotDetecting;

        static readonly string playerLayerMaskName = "Player";
        static readonly string tallGrassLayerMaskName = "TallGrass";
        static readonly string wallLayerMaskName = "Default";

        private void Awake()
        {
            Player = TPM.gameObject;
            mask = LayerMask.GetMask(playerLayerMaskName);

            detectionRange = catManager.GetComponent<SphereCollider>().radius;

            PlayerTrigger.OnPlayerStealth += AddTallGrassLayer;
            PlayerTrigger.OnPlayerUnstealth += RemoveTallGrassLayer;

            mask |= (1 << LayerMask.NameToLayer(wallLayerMaskName));
        }
        private void Start()
        {
            detectionTimer = detectionTimerMax;
        }
        private void Update()
        {
            detectionTimer = Mathf.Clamp(detectionTimer, 0, detectionTimerMax);

            if (!catManager.isDistracted)
            {
                if (raycasting)
                {
                    

                    if (Physics.Raycast(transform.position, (Player.transform.position - transform.position), out RaycastHit hit, detectionRange, mask))
                    {
                        
                        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            catManager.aiState = AIStates.Detecting;
                            isNotDetecting = false;
                            col.enabled = true;
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
                                //Debug.Log("PlayerDetected");
                                Debug.DrawLine(transform.position, hit.point, Color.red);
                                catManager.aiState = AIStates.Aggro;
                            }
                        }
                        else
                        {
                            if (!isNotDetecting)
                            {
                                catManager.StartWalking();

                                detectionTimer = detectionTimerMax;
                                col.enabled = false;
                                isNotDetecting = true;
                            }
                            
                        }
                    }
                }
                if (!raycasting)
                {
                    detectionTimer = detectionTimerMax;
                }
            }

            
        }
        private void ResetCatDetection()
        {
            raycasting = false;
            detectionTimer = detectionTimerMax;
        }
        public void AddTallGrassLayer()
        {
            //Debug.Log("Grass is being detected");
            mask |= (1 << LayerMask.NameToLayer(tallGrassLayerMaskName));
        }
        public void RemoveTallGrassLayer()
        {
            //Debug.Log("Grass is no longer being detected");
            mask &= ~(1 << LayerMask.NameToLayer(tallGrassLayerMaskName));
        }




        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                //Debug.Log("Hit");
                catManager.Distract();
                catManager.ResetAggro();
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Player"))
            {
                
                ResetCatDetection();
                GM.RespawnPlayer();
                catManager.RestartPath();
            }

            if(other.CompareTag("Ladder") || other.CompareTag("Bridge") || other.CompareTag("Bonfire"))
            {
                
                //Debug.Log(other.gameObject);
                Destroy(other.gameObject.GetComponentInParent<ObjectBuild>().gameObject);
            }
        }
    }
}