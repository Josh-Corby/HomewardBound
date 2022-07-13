using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteractionZone : GameBehaviour<InteractionZone>
{
    public bool canPickUp;
    public bool canDestroy;
    public bool canClimb;
    public GameObject player;
    public GameObject objectToPickUp;
    public GameObject objectToDestroy;
    private GameObject LadderEntry;
    public int lightPickUpValue = 8;

    private void Update()
    {
        #region World Interactions
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
            if (objectToPickUp == null && !BM.isBuilding)
            {
                IM.interact_Input = false;
                return;
            }
            
            if (canPickUp)
            {
                if (objectToPickUp.CompareTag("LightPickUp"))
                {
                    FL.ChangeIntensity(lightPickUpValue);
                }

                if (objectToPickUp.CompareTag("SmallRock"))
                {
                    GM.pebblesCollected +=1;
                    UI.UpdatePebblesCollected();
                    Debug.Log(GM.pebblesCollected);
                }

                if (objectToPickUp.CompareTag("Stick"))
                {
                    GM.sticksCollected += 1;
                    UI.UpdateSticksCollected();
                    Debug.Log(GM.sticksCollected);
                }
                if (objectToPickUp.CompareTag("Mushroom"))
                {
                    GM.mushroomsCollected += 1;
                    UI.UpdateMushroomsCollected();
                    Debug.Log(GM.mushroomsCollected);
                }

                //Debug.Log("Picked up small object");
                Destroy(objectToPickUp);
                canPickUp = false;
                objectToPickUp = null;
                IM.interact_Input = false;
                
            }

            
        }
        #endregion

        //Destroy Items
        if (IM.destroy_Input)
        {
            if (canDestroy)
            {
                GM.pebblesCollected += 1;
                GM.sticksCollected += 1;
                GM.mushroomsCollected += 1;
                Destroy(objectToDestroy);
                objectToDestroy = null;
                canDestroy = false;
                canClimb = false;
                UI.UpdateMaterialsCollected();
            }
            if(!canDestroy)
                IM.destroy_Input = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Pickups
        if (other.CompareTag("SmallRock") || other.CompareTag("Stick") || 
            other.CompareTag("Mushroom") || other.CompareTag("LightPickUp"))
        {
            other.GetComponent<Outline>().enabled = true;
            canPickUp = true;
            objectToPickUp = other.gameObject;
        }
        
        if (other.CompareTag("LadderTop") || other.CompareTag("LadderBottom"))
        {
            Debug.Log("Can Climb");
            canClimb = true;
            LadderEntry = other.gameObject;
        }

        if (other.CompareTag("Ladder"))
        {

            Debug.Log("Can Destroy Ladder");
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
        if (other.CompareTag("SmallRock") || other.CompareTag("Stick") || other.CompareTag("Mushroom"))
        {
            other.GetComponent<Outline>().enabled = false;
            canPickUp = false;
            objectToPickUp = null;
        }

        if (other.CompareTag("Ladder"))
        {
            PM.isClimbing = false;
            canClimb = false;
            objectToDestroy = null;
        }
        if (other.CompareTag("LadderTop") || other.CompareTag("LadderBottom"))
        {
            Debug.Log("Can't Climb");
            canClimb = false;
            LadderEntry = null;

            objectToDestroy = null;
        }

        if (other.CompareTag("Bridge"))
        {
            objectToDestroy = null;
            canDestroy = false; 
        }
    }

    public void DisableOutline()
    {
        if (objectToPickUp == null)
            return;
        objectToPickUp.GetComponent<Outline>().enabled = false;
        objectToPickUp = null;
        canPickUp = false;
    }
    public void Toggle(bool isEnabled)
    {
        
        this.gameObject.SetActive(isEnabled);
    }
}
