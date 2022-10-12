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
        public Transform[] points;
        private int destinationIndex = 0;
        private NavMeshAgent agent;
        private readonly float agentWalkSpeed = 5f;

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
        private void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destinationIndex].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destinationIndex = (destinationIndex + 1) % points.Length;
        }

        private void ResumePath()
        {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destinationIndex].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destinationIndex = (destinationIndex) % points.Length;
        }

        public void RestartPath()
        {
            transform.position = points[0].position;

            if(points.Length >= 2)
            {
                transform.LookAt(points[1].position);
            }

            destinationIndex = 0;
            aiState = AIStates.Walk;
            
            //agent.destination = points[destinationIndex].position;
            //transform.position = points[destinationIndex].position;
            //GotoNextPoint();
        }


        private void Update()
        {
            switch (aiState)
            {
                case AIStates.Walk:
                    {
                        agent.speed = agentWalkSpeed;
                        if (resuming)
                        {
                            ResumePath();
                            resuming = false;
                            return;
                        }
                        if (!resuming)
                        {
                            // Choose the next destination point when the agent gets
                            // close to the current one.
                            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                                GotoNextPoint();
                        }
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
                        agent.speed = 15;
                        break;
                    }
                case AIStates.Distracted:
                    if(distractionTransform == null)
                    {
                        aiState = AIStates.Walk;
                        return;
                    }
                    Distract(distractionTransform);
                    break;
            }

            
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
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name + " is in range");
                catDetection.raycasting = true;
            }
            if (other.CompareTag("BerryBomb"))
            {
                Debug.Log("Distraction");
                distractionTransform = other.gameObject.transform;
                aiState = AIStates.Distracted;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name + " is no longer in range");
                catDetection.raycasting = false;
                aiState = AIStates.Walk;
            }

            if (other.CompareTag("BerryBomb"))
            {
                distractionTransform = null;
                aiState = AIStates.Walk;
            }
        }
    }
}

