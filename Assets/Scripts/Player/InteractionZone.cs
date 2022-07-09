using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteractionZone : GameBehaviour<InteractionZone>
{
    public bool canPickUp;
    public bool canClimb;
    public GameObject player;
    public GameObject objectToPickUp;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmallRock") || other.CompareTag("Stick") || 
            other.CompareTag("Mushroom") || other.CompareTag("LightPickUp"))
        {
            //Debug.Log("collide with small rock");
            other.GetComponent<Outline>().enabled = true;
            canPickUp = true;
            objectToPickUp = other.gameObject;
        }

        if (other.CompareTag("Ladder"))
        {
            Debug.Log("Enter Ladder");
            canClimb = true;
        }

        if (other.CompareTag("LadderTop") || other.CompareTag("LadderBottom"))
        {
            Debug.Log("Can Climb");
            canClimb = true;
            LadderEntry = other.gameObject;
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
            Debug.Log("Exit Ladder");
            PM.isClimbing = false;
            canClimb = false;
        }
        if (other.CompareTag("LadderTop") || other.CompareTag("LadderBottom"))
        {
            Debug.Log("Can't Climb");
            canClimb = false;
            LadderEntry = null;
        }
    }

    public void DisableOutline()
    {
        objectToPickUp.GetComponent<Outline>().enabled = false;
        objectToPickUp = null;
        canPickUp = false;
    }
    public void Toggle(bool isEnabled)
    {
        
        this.gameObject.SetActive(isEnabled);
    }
}
