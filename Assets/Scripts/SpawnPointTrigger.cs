using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointTrigger : GameBehaviour
{
    private GameObject _spawnPoint;

    private void Awake()
    {
        _spawnPoint = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GM.SetSpawnPoint(_spawnPoint.transform);
            transform.parent.gameObject.SetActive(false);
        }       
    }
}
