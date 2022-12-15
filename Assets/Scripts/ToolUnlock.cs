using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum Pickups 
{ 
    Builder,
    Slingshot,
}

public class ToolUnlock : GameBehaviour
{
    [SerializeField]
    private Pickups pickups;
    private bool canPickUp;
    private Outline outline;

    private void Awake()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;  
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    private void UnlockTool(Pickups pickups)
    {
        switch (pickups) 
        {
            case Pickups.Builder:
                GM.haveBuilding = !GM.haveBuilding;
                break;

            case Pickups.Slingshot:
                GM.haveSlingshot = !GM.haveSlingshot;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnlockTool(pickups);
            StartCoroutine(WaitToDestroy());
        }          
    }
}
