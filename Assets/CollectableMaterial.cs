using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMaterial : GameBehaviour
{
    [SerializeField]
    private bool isMovingTowardsPlayer;
    private Collider col;

    private float moveSpeed = 0.4f;

    private void Awake()
    {
        isMovingTowardsPlayer = false;
        col = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {

        if (isMovingTowardsPlayer)
        {
            MoveTowardsPlayer();
        }

        if (Vector3.Distance(transform.position, PM.pickUpSpot.transform.position) <= 0.1f)
        {
            GM.IncreaseResources(gameObject);
            if (SM.SFX != null)
            {
                SM.PlayClip(SM.pickupClip);
            }

            gameObject.SetActive(false);
        }
    }
    public void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, PM.pickUpSpot.transform.position, moveSpeed);
    }

    public void StartMovingTowardsPlayer()
    {
        isMovingTowardsPlayer = true;
        col.isTrigger = true;
    }
}
