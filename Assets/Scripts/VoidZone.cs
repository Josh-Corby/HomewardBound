using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidZone : GameBehaviour
{
    // If player enters the voidzone they are respawned
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GM.RespawnPlayer();
        }
    }
}
