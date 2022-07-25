using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteractionZone : GameBehaviour<InteractionZone>
{
    public bool canPickUp;
    public bool canDestroy;
    public bool canClimb;
    public bool canBreak;
    public GameObject player;
    public GameObject objectToInteract;
    public GameObject objectToDestroy;
    private GameObject LadderEntry;
    public int lightPickUpValue = 8;

    private int pebbleCounter = 0;

    private void Update()
    {
        #region Item Pickups
        if (IM.interact_Input)
        {
            if (canClimb)
            {
                if(LadderEntry != null)
                {
                    player.transform.position = LadderEntry.transform.position;
                    player.transform.rotation = LadderEntry.transform.rotation;
                    PM.isClimbing = true;
                }
                PM.isClimbing = true;
                Debug.Log("ClimbingLadder");
            }
            if (objectToInteract == null && !BM.isBuilding)
            {
                IM.interact_Input = false;
                return;
            }
            
            if (canPickUp)
            {
                if (objectToInteract.CompareTag("LightPickUp"))
                {
                    FL.ChangeIntensity(lightPickUpValue);
                }

                if (objectToInteract.CompareTag("Rock"))
                {
                    GM.rocksCollected +=1;
                    UI.UpdateRocksCollected();
                    Debug.Log(GM.rocksCollected);
                }

                if (objectToInteract.CompareTag("Stick"))
                {
                    GM.sticksCollected += 1;
                    UI.UpdateSticksCollected();
                    Debug.Log(GM.sticksCollected);
                }
                if (objectToInteract.CompareTag("Mushroom"))
                {
                    GM.mushroomsCollected += 1;
                    UI.UpdateMushroomsCollected();
                    Debug.Log(GM.mushroomsCollected);
                }
                if (objectToInteract.CompareTag("Pebble"))
                {
                    GM.pebblesCollected += 1;
                    UI.UpdatePebblesCollected();
                    Debug.Log(GM.pebblesCollected);
                }

                //Debug.Log("Picked up small object");
                Destroy(objectToInteract);
                canPickUp = false;
                objectToInteract = null;
                IM.interact_Input = false;
                
            }      
        }
        #endregion

        
        //Destroy Items
        if(OM.outfit == Outfits.Miner && GM.havePickaxe)
        {
            //Break Items{
            if (canBreak)
            {
                if (IM.interact_Input)
                {
                    IM.interact_Input = false;
                    if (objectToInteract.CompareTag("Rock"))
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            GameObject pebble = Instantiate(GM.pebblePrefab);
                            pebble.transform.parent = objectToInteract.transform;
                            pebble.transform.localPosition = new Vector3(Random.Range(0.0f, 1.0f), 0, Random.Range(0f, 1f));
                            pebbleCounter += 1;
                            pebble.transform.parent = null;
                            pebble.name = "Pebble_" + pebbleCounter;
                        }
                        Destroy(objectToInteract);
                        TogglePickUpBools();
                        canBreak = false;
                        return;
                    }

                    if (objectToInteract.CompareTag("BreakableWall"))
                    {
                        Destroy(objectToInteract);

                    }
                    
                    else 
                    {
                        return;
                    }

                }
            }
            if (IM.destroy_Input)
            {
                if (canDestroy)
                {
                    if (objectToDestroy.CompareTag("Ladder"))
                    {
                        DestroyObject();
                        LC.inside = false;
                        TPM.enabled = true;
                        
                    }
                    DestroyObject();
                }
                if (!canDestroy)
                    IM.destroy_Input = false;
            }
        }
    }
        

    private void OnTriggerEnter(Collider other)
    {
        //Pickups
        if (other.CompareTag("Rock") || other.CompareTag("Stick") || 
            other.CompareTag("Mushroom") || other.CompareTag("LightPickUp") || other.CompareTag("Pebble"))
        {
            other.GetComponent<Outline>().enabled = true;
            canPickUp = true;
            objectToInteract = other.gameObject;
        }

        //Breakable Objects
        if (other.CompareTag("Rock") || other.CompareTag("BreakableWall"))
        {
            objectToInteract = other.gameObject;
            other.GetComponent<Outline>().enabled = true;
            canBreak = true;
        }

        //if (other.CompareTag("LadderTop") || other.CompareTag("LadderBottom"))
        //{
        //    Debug.Log("Can Climb");
        //    canClimb = true;
        //    LadderEntry = other.gameObject;
        //}

        //if (other.CompareTag("LadderBody"))
        //{

        //    Debug.Log("Can Destroy Ladder");
        //    objectToDestroy = other.gameObject;
        //    canDestroy = true;
        //}
        if (other.CompareTag("Ladder"))
        {
            objectToDestroy = other.gameObject;
            canDestroy = true;
        }

        if (other.CompareTag("Bridge"))
        {
            Debug.Log("Can Destroy Bridge");
            objectToDestroy = other.gameObject;
            canDestroy = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stick") || other.CompareTag("Mushroom") || other.CompareTag("LightPickUp") || other.CompareTag("Pebble"))
        {
            DisableInteractions();
        }

        if (other.CompareTag("Rock") || other.CompareTag("BreakableWall"))
        {
            canBreak = false;
            objectToInteract.GetComponent<Outline>().enabled = false;

        }

        if (other.CompareTag("Ladder"))
        {
            PM.isClimbing = false;
            canClimb = false;
            objectToDestroy = null;
        }
        //if (other.CompareTag("LadderTop") || other.CompareTag("LadderBottom"))
        //{
        //    Debug.Log("Can't Climb");
        //    canClimb = false;
        //    LadderEntry = null;

        //    objectToDestroy = null;
        //}

        if (other.CompareTag("Bridge"))
        {
            objectToDestroy = null;
            canDestroy = false; 
        }
    }

    public void DisableInteractions()
    {
        if (objectToInteract == null)
            return;
        objectToInteract.GetComponent<Outline>().enabled = false;
        TogglePickUpBools();
    }

    public void TogglePickUpBools()
    {
        objectToInteract = null;
        canPickUp = false;
        canDestroy = false;
        canBreak = false;
    }
    public void Toggle(bool isEnabled)
    {
        
        this.gameObject.SetActive(isEnabled);
    }

    public void DestroyObject()
    {
        Destroy(objectToDestroy);
        objectToDestroy = null;
        canDestroy = false;
        canClimb = false;
        UI.UpdateMaterialsCollected();
    }
}
