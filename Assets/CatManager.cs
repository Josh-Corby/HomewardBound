using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public CatDetection catDetection;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catDetection.raycasting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catDetection.raycasting = false;
        }
    }
}
