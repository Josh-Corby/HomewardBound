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

    public List<GameObject> outlineObjectsList = new List<GameObject>();

    private void Update()
    {
        if (outlineObjectsList.Count > 0)
            OutlineCheck();

        if (outlineObjectsList.Count > 0)
            canPickUp = true;
        if (outlineObjectsList.Count < 0)
            canPickUp = false;

        if (OM.outfit == Outfits.Miner)
        {
            //Break Items

            if (UI.radialMenuStatus || UI.paused == true)
            {
                //canBreak = false;
                return;
            }


            if (IM.lClick_Input)
            {
                IM.lClick_Input = false;


                if (objectToInteract.CompareTag("Rock"))
                {
                    //canBreak = true;
                    outlineObjectsList.Remove(objectToInteract);
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
        if (OM.outfit == Outfits.Builder)
        {
            if (IM.interact_Input)
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
                //if (!canDestroy)
                //    IM.interact_Input = false;
            }
        }
        #region Item Pickups
        if (IM.interact_Input)
        {
            if (canClimb)
            {
                if (LadderEntry != null)
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
                outlineObjectsList.Remove(objectToInteract);
                if (objectToInteract.CompareTag("LightPickUp"))
                {
                    FL.ChangeIntensity(lightPickUpValue);
                }

                if (objectToInteract.CompareTag("Rock"))
                {
                    GM.rocksCollected += 1;
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
    }

    private void OutlineObjects()
    {
        if (outlineObjectsList.Count <= 0) return;
        float closestDistanceSqr = 4f;
        Vector3 playerPosition = player.transform.position;

        foreach (GameObject objectToOutline in outlineObjectsList)
        {
            Vector3 directionToTarget = objectToOutline.transform.position - playerPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                objectToInteract = objectToOutline;
                closestDistanceSqr = dSqrToTarget;
                objectToOutline.GetComponent<Outline>().enabled = true;
            }
            if (dSqrToTarget > closestDistanceSqr)
            {

                Debug.Log("Object not highlighted");
                objectToOutline.GetComponent<Outline>().enabled = false;
                //outlineObjectsList.Remove(objectToOutline);

            }
        }
    }

    private void OutlineCheck()
    {
        OutlineObjects();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Pickups
        if (other.CompareTag("Rock") || other.CompareTag("Stick") ||
            other.CompareTag("Mushroom") || other.CompareTag("LightPickUp") || other.CompareTag("Pebble"))
        {
            //other.GetComponent<Outline>().enabled = true;
            canPickUp = true;


            outlineObjectsList.Add(other.gameObject);
            OutlineCheck();
        }

        //Breakable Objects
        if (other.CompareTag("Rock") || other.CompareTag("BreakableWall"))
        {
            objectToInteract = other.gameObject;
            //other.GetComponent<Outline>().enabled = true;

            //outlineObjectsList.Add(objectToInteract);

        }
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

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock") || other.CompareTag("Stick") || other.CompareTag("Mushroom") || other.CompareTag("LightPickUp") || other.CompareTag("Pebble"))
        {
            DisableInteractions();
            other.GetComponent<Outline>().enabled = false;
            outlineObjectsList.Remove(other.gameObject);
            OutlineCheck();
        }

        if (other.CompareTag("Rock") || other.CompareTag("BreakableWall"))
        {
            //outlineObjectsList.Remove(other.gameObject);
            //canBreak = false;
            //objectToInteract.GetComponent<Outline>().enabled = false;

        }

        if (other.CompareTag("Ladder"))
        {
            PM.isClimbing = false;
            canClimb = false;
            objectToDestroy = null;
            canDestroy = false;
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

        TogglePickUpBools();
    }

    public void TogglePickUpBools()
    {
        objectToInteract = null;
        canDestroy = false;
        //canBreak = false;
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
