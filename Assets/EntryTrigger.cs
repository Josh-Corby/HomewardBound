using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryTrigger : MonoBehaviour
{
    public HubManager hM;


    private void Awake()
    {
        hM = gameObject.GetComponentInParent<HubManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < hM.EntryTriggers.Length; i++)
            {
                hM.EntryTriggers[i].GetComponent<BoxCollider>().enabled = false;

            }

            for (int i = 0; i < hM.ExitTriggers.Length; i++)
            {
                hM.ExitTriggers[i].GetComponent<BoxCollider>().enabled = true;
            }

            for (int i = 0; i < hM.ObjectsToSpawn.Length; i++)
            {
                GameObject pickup = Instantiate(hM.ObjectsToSpawn[i]);
                
                pickup.transform.parent = hM.transform;
                pickup.transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
                hM.spawnedObjectsList.Add(pickup);
            }
        }
    }
}