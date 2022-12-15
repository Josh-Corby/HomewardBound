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
    private Pickups _pickups;
    private Outline _outline;

    private void Awake()
    {
        _outline = gameObject.GetComponent<Outline>();
        _outline.enabled = false;  
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
            UnlockTool(_pickups);
            StartCoroutine(WaitToDestroy());
        }          
    }
}
