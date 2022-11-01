using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpZone : GameBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Rock") || other.CompareTag("Stick") || other.CompareTag("Mushroom") || other.CompareTag("Pebble"))
        {
            //Debug.Log("start material pickup");
            other.gameObject.GetComponent<CollectableMaterial>().StartMovingTowardsPlayer();
        }
    }
}
