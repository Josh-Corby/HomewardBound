using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpZone : GameBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Rock") || other.CompareTag("Stick") || other.CompareTag("String") || other.CompareTag("Pebble"))
        {
            other.gameObject.GetComponent<CollectableMaterial>().StartMovingTowardsPlayer();
        }
    }
}
