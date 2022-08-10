using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public HubManager hM;
    GameObject curentObject;

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
                hM.EntryTriggers[i].GetComponent<BoxCollider>().enabled = true;
            }

            for (int i = 0; i < hM.ExitTriggers.Length; i++)
            {
                hM.ExitTriggers[i].GetComponent<BoxCollider>().enabled = false;
            }

            for (int i = 0; i < hM.spawnedObjectsList.Count; i++)
            {
                curentObject = hM.spawnedObjectsList[i];
                hM.spawnedObjectsList.Remove(hM.spawnedObjectsList[i]);
                Destroy(curentObject);
            }
        }
    }
}

