using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    /*waypoints tied to boat transform
     * fish move from point to boat to point to boat lerp
     * 
     * 
     *
     * 
     */

    public Transform pathHolder;

    public float speed = 5f;
    public float waitTime = 0.3f;
    public float turnSpeed = 90;


    private void Start()
    {
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }
        StartCoroutine(FollowPath(waypoints));
    }


    IEnumerator FollowPath(Vector3[] wayPoints)
    {
        transform.position = wayPoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = wayPoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if(transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % wayPoints.Length;
                targetWaypoint = wayPoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        transform.LookAt(lookTarget);
        yield return null;
        //Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        //float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x * Mathf.Rad2Deg);

        //while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)> 0.05)
        //{
        //    float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
        //    transform.eulerAngles = Vector3.up * angle;
        //    yield return null;
        //}
    }
    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach(Transform waypoint in pathHolder)
        {
            Gizmos.DrawLine(previousPosition, waypoint.position);
            Gizmos.DrawSphere(waypoint.position, .3f);
            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);
    }

}
