using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum Pickups { 

    Pickaxe,
    Builder,
    Slingshot,
    GrappleHook,
    //Glider
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


    //private void Update()
    //{
    //    if (canPickUp)
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            UnlockTool(pickups);

    //            StartCoroutine(WaitToDestroy());
    //        }
    //    }
    //}

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    private void UnlockTool(Pickups pickups)
    {
        switch (pickups) 
        {
            case Pickups.Pickaxe:
                GM.havePickaxe = !GM.havePickaxe;
                break;
            case Pickups.Builder:
                GM.haveBuilding = !GM.haveBuilding;
                break;

            case Pickups.Slingshot:
                GM.haveSlingshot = !GM.haveSlingshot;
                break;
            case Pickups.GrappleHook:
                break;
            //case Pickups.Glider:
            //    GM.haveGlider = !GM.haveGlider;
            //    break;
        }
       // UI.UpdateToolsUnlockedUI(pickups.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //canPickUp = true;
            //outline.enabled = true;

            UnlockTool(pickups);
            StartCoroutine(WaitToDestroy());
        }          
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {         
    //        //canPickUp = false;
    //        //outline.enabled = false;
    //    }           
    //}
}
