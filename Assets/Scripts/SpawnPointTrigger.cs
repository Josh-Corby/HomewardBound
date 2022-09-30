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


    public void CheckSpawnPoint()
    {
        if (GM.spawnPoint == transform.parent.gameObject)
        {
            spawnTrigger.enabled = false;
        }

        else spawnTrigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GM.SetSpawnPoint(spawnPoint.transform);
        }       
    }
}
