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
        private readonly float agentWalkSpeed = 10f;
        private readonly float agentRunSpeed = 15;

        public AIStates aiState;
        private GameObject Player;
        private bool resuming;

        private int rotationX;
        private int rotationZ;
        private readonly float rotationSpeed = 1f;

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
            agent.autoBraking = false;

            aiState = AIStates.Walk;
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

            //destinationIndex = 0;
            aiState = AIStates.Walk;

        }
        private void Update()
        {
            switch (aiState)
            {
                case AIStates.Walk:
                    {
                        agent.speed = agentWalkSpeed;
                        StartCoroutine(ResetAggro());
                        //if (resuming)
                        //{
                        //    ResumePath();
                        //    resuming = false;
                        //    return;
                        //}
                        //if (!resuming)
                        //{
                            // Choose the next destination point when the agent gets
                            // close to the current one.
                            //if (!agent.pathPending && agent.remainingDistance < 0.5f)

                                

                        //}
                        break;
                    }
                case AIStates.Detecting:
                    {
                        agent.SetDestination(transform.position);
                        resuming = true;
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
                    if (distractionTransform == null)
                    {
                        aiState = AIStates.Walk;
                        return;
                    }
                    Distract(distractionTransform);
                    break;
            }
        }

        private IEnumerator ResetAggro()
        {
            yield return new WaitForSeconds(1f);
            FindNextPoint();
        }
        private void FindNextPoint()
        {
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
            agent.SetDestination(distraction.position);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == TPM.gameObject)
            {
                Debug.Log(other.name + " is in range");
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
                aiState = AIStates.Walk;
            }

            //if (other.CompareTag("BerryBomb"))
            //{
            //    distractionTransform = null;
            //    aiState = AIStates.Walk;
            //}
        }
    }
}

