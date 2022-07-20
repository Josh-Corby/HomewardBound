using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SnakeDetection : GameBehaviour
{


    //public float speed = 5f;
    //public float waitTime = .3f;
    //public float turnSpeed = 90;
    

    public Light spotlight;
    public float viewDistance;
    public LayerMask mask;
    public bool rayCasting;
    public bool canSeePlayer;

    float viewAngle;

    public float timer;
    readonly float maxTimer = 4f;

    GameObject player;
    Color originalSpotlightColor;

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;




    private void Start()
    {
        timer = maxTimer;
        canSeePlayer = false;
        player = GameObject.Find("ThirdPersonPlayer");
        //viewAngle = spotlight.spotAngle;
        //originalSpotlightColor = spotlight.color;
        rayCasting = false;

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        //GotoNextPoint();
    }

    private void Update()
    {
        timer = Mathf.Clamp(timer, 0, maxTimer);
        if (canSeePlayer)
        {
            transform.position = transform.position;
            
            if(timer <= 0)
            {
                GM.RespawnPlayer();
                timer = maxTimer;
                canSeePlayer = false;
                rayCasting = false;
            }
            return;
        }

        



        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (canSeePlayer)
        {
            agent.ResetPath();
            return;
        }
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            rayCasting = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.LookAt(other.gameObject.transform);
            RaycastHit hit;
            Ray snakeRay = new Ray(transform.position, player.transform.position);

            if (rayCasting)
            {
                
                if (Physics.Raycast(snakeRay, out hit, 100, mask))
                {
                    if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        Debug.DrawLine(snakeRay.origin, hit.point, Color.red);
                        canSeePlayer = true;
                        Debug.Log(hit.collider.gameObject.name);
                        timer -= Time.deltaTime;
                    }
                    else
                    {
                        canSeePlayer = false;
                        Debug.DrawLine(snakeRay.origin, hit.point, Color.green);                 
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rayCasting = false;
            canSeePlayer = false;
        }
    }
}
