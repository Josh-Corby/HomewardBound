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
    public GameObject objectToPickUp;
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

                if (objectToPickUp.CompareTag("Rock"))
                {
                    GM.rocksCollected +=1;
                    UI.UpdateRocksCollected();
                    Debug.Log(GM.rocksCollected);
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
                if (objectToPickUp.CompareTag("Pebble"))
                {
                    GM.pebblesCollected += 1;
                    UI.UpdatePebblesCollected();
                    Debug.Log(GM.pebblesCollected);
                }

                //Debug.Log("Picked up small object");
                Destroy(objectToPickUp);
                canPickUp = false;
                objectToPickUp = null;
                IM.interact_Input = false;
                
            }      
        }
        #endregion

        //Break Items{
        if (canBreak)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (objectToPickUp.CompareTag("Rock"))
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        GameObject pebble = Instantiate(GM.pebblePrefab);
                        pebble.transform.parent = objectToPickUp.transform;
                        pebble.transform.localPosition = new Vector3(Random.Range(0.0f, 1.0f), 0, Random.Range(0f, 1f));
                        pebbleCounter += 1;
                        pebble.transform.parent = null;
                        pebble.name = "Pebble_" + pebbleCounter;                                           
                    }          
                    Destroy(objectToPickUp);
                    TogglePickUpBools();
                    canBreak = false;
                }
                else return;
            }
        }
        //Destroy Items
        if (IM.destroy_Input)
        {
            if (canDestroy)
            {
                GM.rocksCollected += 1;
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
        if (other.CompareTag("Rock") || other.CompareTag("Stick") || 
            other.CompareTag("Mushroom") || other.CompareTag("LightPickUp") || other.CompareTag("Pebble"))
        {
            other.GetComponent<Outline>().enabled = true;
            canPickUp = true;
            objectToPickUp = other.gameObject;
        }

        if (other.CompareTag("Rock"))
        {
            canBreak = true;
        }
        
        if (other.CompareTag("LadderTop") || other.CompareTag("LadderBottom"))
        {
            Debug.Log("Can Climb");
            canClimb = true;
            LadderEntry = other.gameObject;
        }

        if (other.CompareTag("LadderBody"))
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
        if (other.CompareTag("Rock") || other.CompareTag("Stick") || other.CompareTag("Mushroom") || other.CompareTag("LightPickUp") || other.CompareTag("Pebble"))
        {
            StopPickUp();
        }

        if (other.CompareTag("Rock"))
        {
            canBreak = false;
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

    public void StopPickUp()
    {
        if (objectToPickUp == null)
            return;
        objectToPickUp.GetComponent<Outline>().enabled = false;
        TogglePickUpBools();
    }

    public void TogglePickUpBools()
    {
        objectToPickUp = null;
        canPickUp = false;
    }
    public void Toggle(bool isEnabled)
    {
        
        this.gameObject.SetActive(isEnabled);
    }
}
