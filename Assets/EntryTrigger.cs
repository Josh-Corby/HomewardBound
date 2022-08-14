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

            hM.EnableAllObjects();
            //for (int i = 0; i < 15; i++)
            //{
            //    for (int a = 0; a < hM.ObjectsToSpawn.Length; a++)
            //    {

            //        GameObject pickup = Instantiate(hM.ObjectsToSpawn[a]);

            //        pickup.transform.parent = hM.transform;
            //        pickup.transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.01f, Random.Range(-3f, 3f));
            //        hM.spawnedObjectsList.Add(pickup);
            //    }
            //}

           
        }
    }
}
