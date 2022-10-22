using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonfireManager : GameBehaviour
{
    [SerializeField]
    private Transform SpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == IZ.gameObject)
        {
            Debug.Log("Player at bonfire");
            InteractionZone.OnRespawnSet += SetRespawnPoint;        
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == IZ.gameObject)
        {
            Debug.Log("Player no longer at bonfire");
            InteractionZone.OnRespawnSet -= SetRespawnPoint;
        }
    }
    private void SetRespawnPoint()
    {
        GM.spawnPoint = SpawnPoint;
    }
}
