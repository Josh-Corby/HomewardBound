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

        private float detectionTimer;
        [SerializeField]
        private float detectionTimerMax;
        private float detectionRange;

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

            if (raycasting)
            {
                catManager.aiState = AIStates.Detecting;
                if (Physics.Raycast(transform.position, (Player.transform.position - transform.position), out RaycastHit hit, detectionRange, mask))
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
                            //Debug.Log("PlayerDetected");
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
            if (other.CompareTag("Player"))
            {
                ResetCatDetection();
                GM.RespawnPlayer();
                catManager.RestartPath();
            }
        }
    }
}