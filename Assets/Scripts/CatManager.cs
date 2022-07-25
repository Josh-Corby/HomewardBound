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
        private int destPoint = 0;
        private NavMeshAgent agent;

        public AIStates aiState;
        public GameObject player;
        private bool resuming;

        private readonly float moveSpeed = 1f;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;

            aiState = AIStates.Walk;
        }


        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;
        }

        void ResumePath()
        {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint) % points.Length;
        }

        void Update()
        {
            switch (aiState)
            {
                case AIStates.Walk:
                    {
                        agent.speed = 3.5f;
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
                        //agent.ResetPath();
                        resuming = true;
                        break;
                    }
                case AIStates.Aggro:
                    {
                        agent.SetDestination(player.transform.position);
                        agent.speed = 15;
                        //transform.position =  Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
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

        private void Distract(Transform distraction)
        {
            agent.SetDestination(distraction.position);

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
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