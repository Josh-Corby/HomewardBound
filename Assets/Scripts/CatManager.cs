using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Cat
{

    public enum AIStates
    {
        Walk,
        Detecting,
        Aggro,
        Distracted
    }
    public class CatManager : GameBehaviour
    {
        public CatDetection catDetection;

        public Transform distractionTransform = null;
        public Transform[] patrolPoints;
        [SerializeField]
        private Transform closestPatrolPoint;
        //private int destinationIndex = 0;
        private NavMeshAgent agent;
        private readonly float agentWalkSpeed = 15f;
        private readonly float agentRunSpeed = 20;

        public AIStates aiState;
        private GameObject Player;
        private readonly float rotationSpeed = 1f;

        public bool isDistracted;
        private void Awake()
        {
            Player = TPM.gameObject;
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).


            StartWalking();
        }
        private void ResumePath()
        {
            // returns if no points have been set up
            if (patrolPoints.Length == 0)
                return;
            agent.destination = closestPatrolPoint.position;
        }
        public void RestartPath()
        {
            transform.position = patrolPoints[0].position;

            if (patrolPoints.Length >= 2)
            {
                transform.LookAt(patrolPoints[1].position);
            }


            StartWalking();
        }
        private void Update()
        {
            if(agent.remainingDistance < 0.5f)
            {
                FindNextPoint();
            }


            switch (aiState)
            {
                case AIStates.Walk:
                    {
                        agent.speed = agentWalkSpeed;
                        break;
                    }
                case AIStates.Detecting:
                    {
                        agent.SetDestination(transform.position);
                        break;
                    }
                case AIStates.Aggro:
                    {
                        agent.SetDestination(Player.transform.position);
                        LookAtPlayer();
                        agent.speed = agentRunSpeed;
                        break;
                    }
                case AIStates.Distracted:
                    {
                        Distract(transform);
                        
                    }
                    break;
            }
        }

        public void Distract()
        {
            isDistracted = true;
            aiState = AIStates.Distracted;

        }

        public void StartWalking()
        {
            agent.destination = transform.position;
            //Debug.Log("Start walking");
            aiState = AIStates.Walk;
            FindNextPoint();
        }
        public void ResetAggro()
        {
            StopCoroutine(ResetAggroTimer());
            StartCoroutine(ResetAggroTimer());
        }

        private IEnumerator ResetAggroTimer()
        {
            Debug.Log("Resetting aggro");
            yield return new WaitForSeconds(0.5f);
            isDistracted = false;
            FindNextPoint();
        }
        private void FindNextPoint()
        {
            agent.isStopped = true;
            //Debug.Log("finding next point");
            if (patrolPoints.Length <= 0) return;
            float closestpoint = Mathf.Infinity;
            Vector3 playerPosition = Player.transform.position;

            foreach (Transform patrolPoint in patrolPoints)
            {
                if (patrolPoint != closestPatrolPoint)
                {

                    Vector3 directionToTarget = patrolPoint.position - playerPosition;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestpoint)
                    {

                        closestpoint = dSqrToTarget;
                        closestPatrolPoint = patrolPoint;
                    }
                }
            }
            agent.isStopped = false;
            agent.SetDestination(closestPatrolPoint.position);
            //Debug.Log(closestPatrolPoint);
        }

        public void LookAtPlayer()
        {
            var lookPos = Player.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        private void Distract(Transform distraction)
        {
            agent.SetDestination(transform.position);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == TPM.gameObject)
            {
                //Debug.Log(other.name + " is in range");
                catDetection.raycasting = true;
            }
            //if (other.CompareTag("BerryBomb"))
            //{
            //    Debug.Log("Distraction");
            //    distractionTransform = other.gameObject.transform;
            //    aiState = AIStates.Distracted;
            //}
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == TPM.gameObject)
            {
                //Debug.Log(other.name + " is no longer in range");
                catDetection.raycasting = false;
                StartWalking();
            }

        }
    }
}

