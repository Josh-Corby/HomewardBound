using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatDetection : GameBehaviour
{
    public GameObject player;
    public LayerMask mask;
    public bool raycasting;


    private void Update()
    {
        if (raycasting)
        {
            transform.LookAt(player.transform);
            RaycastHit hit;
            //Ray catRay = new Ray(transform.position, player.transform.position);

            if (Physics.Raycast(transform.position, transform.forward, out hit, 15f, mask))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    Debug.Log("Can see player");
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                }
            }       
        }
    }
}
