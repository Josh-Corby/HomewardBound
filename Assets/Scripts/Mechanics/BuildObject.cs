using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : GameBehaviour
{
    public List<GameObject> collisionObjects = new List<GameObject>();


    private void Update()
    {
        BM.canBuild = collisionObjects.Count <= 0 ? true : false; 
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ladder") || other.CompareTag("Hawk"))
        {
            return;
        }

        Debug.Log(other.gameObject.name);

        collisionObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        collisionObjects.Remove(other.gameObject);
    }
}
