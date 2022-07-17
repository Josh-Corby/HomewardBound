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
    public LayerMask viewMask;

    float viewAngle;
    public float playerVisibleTimer;
    float timeToSpotPlayer = 5f;

    GameObject player;
    Color originalSpotlightColor;

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;


    private void Start()
    {
        player = GameObject.Find("ThirdPersonPlayer");
        viewAngle = spotlight.spotAngle;
        originalSpotlightColor = spotlight.color;


        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        GotoNextPoint();
    }

    private void Update()
    {
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        if (CanSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
        }
        
        spotlight.color = Color.Lerp(originalSpotlightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);

        if (playerVisibleTimer >= timeToSpotPlayer)
        {
            GM.RespawnPlayer();
        }

        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
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

    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
        {
            Debug.Log("Can see player");
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.transform.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }





    private void OnDrawGizmos()
    {


        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
