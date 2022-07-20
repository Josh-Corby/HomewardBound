using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerManager : GameBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pebble"))
        {

        }
    }
}
