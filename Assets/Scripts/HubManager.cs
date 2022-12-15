using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : GameBehaviour
{
    public GameObject[] EntryTriggers;
    public GameObject[] ExitTriggers;
    //public GameObject[] ObjectsToSpawn;
    //public List<GameObject> spawnedObjectsList = new List<GameObject>();

    [Header("Spawn Prefabs")]

    [SerializeField]
    private GameObject rockPrefab;
    [SerializeField]
    private GameObject stickPrefab;
    [SerializeField]
    private GameObject stringPrefab;

    [Header("Pools")]
    [SerializeField]
    private GameObject[] rocksPool;
    [SerializeField]
    private GameObject[] sticksPool;
    [SerializeField]
    private GameObject[] stringPool;

    private void Start()
    {
        SpawnAllItemsToPools();
    }

    private void SpawnAllItemsToPools()
    {
        SpawnItemsToPool(rocksPool, rockPrefab);
        SpawnItemsToPool(sticksPool, stickPrefab);
        SpawnItemsToPool(stringPool, stringPrefab);
    }

    private void SpawnItemsToPool(GameObject[] poolToSpawnTo, GameObject prefabToSpawn)
    {
        for (int i = 0; i < poolToSpawnTo.Length; i++)
        {
            GameObject pickup = Instantiate(prefabToSpawn);
            poolToSpawnTo[i] = pickup;
            pickup.transform.parent = this.transform;
            pickup.transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.01f, Random.Range(-3f, 3f));
        }
    }
    public void EnableAllObjects()
    {
        EnableObjects(rocksPool);
        EnableObjects(sticksPool);
        EnableObjects(stringPool);
    }

    public void EnableObjects(GameObject[] poolToEnable)
    {
        for (int i = 0; i < poolToEnable.Length; i++)
        {
            poolToEnable[i].SetActive(true);
        }
    }

    public void DisableAllObjects()
    {
        DisableObjects(rocksPool);
        DisableObjects(sticksPool);
        DisableObjects(stringPool);
    }

    public void DisableObjects(GameObject[] poolToDisable)
    {
        for (int i = 0; i < poolToDisable.Length; i++)
        {
            poolToDisable[i].SetActive(false);
        }
    }
}
