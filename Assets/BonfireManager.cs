using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonfireManager : GameBehaviour
{
    [SerializeField]
    private Transform SpawnPoint;
    [SerializeField]
    private GameObject particles;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == IZ.gameObject)
        {
            Debug.Log(other.gameObject);
            //Debug.Log("Player at bonfire");
            //InteractionZone.OnRespawnSet += SetRespawnPoint;   
            SetRespawnPoint();
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.gameObject == IZ.gameObject)
    //    {
    //        Debug.Log("Player no longer at bonfire");
    //        //InteractionZone.OnRespawnSet -= SetRespawnPoint;
    //    }
    //}
    private void SetRespawnPoint()
    {
        Debug.Log("Respawn set");
        GM.spawnPoint = SpawnPoint;
        particles.SetActive(true);
    }
}
