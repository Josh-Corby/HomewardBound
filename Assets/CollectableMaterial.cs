using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMaterial : GameBehaviour
{
    [SerializeField]
    private bool isMovingTowardsPlayer;
    private Collider col;
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
            gameObject.SetActive(false);
        }
    }
    public void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, PM.pickUpSpot.transform.position, 0.1f);
    }

    public void StartMovingTowardsPlayer()
    {
        isMovingTowardsPlayer = true;
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUpSpot"))
        {
            
            //gameObject.SetActive(false);
        }
    }
}
