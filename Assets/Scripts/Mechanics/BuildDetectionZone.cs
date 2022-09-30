using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDetectionZone : GameBehaviour
{
    public int buildCost;
    public bool canBuild;
    public GameObject buildPrefab;
    public Vector3 buildLocation;
    private void Start()
    {
        canBuild = false;
    }


    private void Update()
    {
        if (IM.interact_Input) 
        {
            
            if (canBuild)
            {
                Debug.Log("Interact");
                if (GM.rocksCollected >= buildCost)
                {
                    Instantiate(buildPrefab, buildLocation, Quaternion.identity);
                    GM.rocksCollected -= buildCost;
                    UI.UpdateRocksCollected();
                    this.gameObject.SetActive(false);
                    IM.interact_Input = false;
                    UI.UpdateCanBuildText(false);
                    Debug.Log("Prefab Built");
                    return;
                }
                if(GM.rocksCollected < buildCost)
                {
                    UI.UpdateBuildStatus("Not enough rocks!");
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Can Build");
            canBuild = true;
            UI.UpdateCanBuildText(true);
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Can't Build");
            canBuild = false;
            UI.UpdateCanBuildText(false);
        }
    }
}
