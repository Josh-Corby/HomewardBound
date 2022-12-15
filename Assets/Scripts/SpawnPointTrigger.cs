using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointTrigger : GameBehaviour
{
    private GameObject spawnPoint;
    private BoxCollider spawnTrigger;


    private void Awake()
    {
        spawnPoint = transform.parent.gameObject;
        spawnTrigger = gameObject.GetComponent<BoxCollider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GM.SetSpawnPoint(spawnPoint.transform);
            transform.parent.gameObject.SetActive(false);
            //spawnTrigger.enabled = false;
        }       
    }
}
