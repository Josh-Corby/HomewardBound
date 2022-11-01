using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class InteractionZone : GameBehaviour<InteractionZone>
{
    public static event Action OnRespawnSet;
    public static event Action<GameObject> OnItemPickUp;

    public bool canPickUp;
    public bool canDestroy;
    public bool canClimb;
    public bool canBreak;
    public bool isRolling;
    [SerializeField]
    private bool atBonfire;
    public GameObject Player;
    public GameObject objectToInteract;
    public GameObject objectToDestroy;
    private GameObject LadderEntry;
    public int lightPickUpValue = 8;

    private int pebbleCounter = 0;

    public List<GameObject> outlineObjectsList = new List<GameObject>();

    private readonly float interactRange = 3f;

    private Collider col;
    private void Awake()
    {
        Player = TPM.gameObject;
        col = GetComponent<BoxCollider>();
    }
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


            if (IM.rClick_Input)
            {
                if (objectToInteract == null)
                {
                    IM.lClick_Input = false;
                    return;
                }

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

                if (objectToInteract.CompareTag("MinableObject"))
                {
                    StartCoroutine(objectToInteract.GetComponent<MinableWall>().Break());
                }
                else
                {
                    return;
                }
            }
        }
        if (OM.outfit == Outfits.Builder)
        {
            if (Input.GetKeyDown(KeyCode.E))
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
                //    Input.GetKeyDown(KeyCode.E) = false;
            }
        }
        #region Item Interactions
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (objectToInteract == null)
            {

                return;
            }
            if (objectToInteract != null)
            {
                if (isRolling)
                {
                    Debug.Log("Stop rolling");
                    objectToInteract.transform.parent.parent = null;

                    col.enabled = true;
                    isRolling = false;

                    return;
                }


                if (!isRolling)
                {
                    if (atBonfire)
                    {
                        Debug.Log("Respawnset");
                        OnRespawnSet();

                        
                        return;
                    }

                    if (objectToInteract.CompareTag("RollingRock"))
                    {
                        if (TPM.groundState == GroundStates.Grounded)
                        {

                            objectToInteract.transform.parent.SetParent(TPM.transform);
                            col.enabled = false;
                            isRolling = true;

                            return;
                        }
                    }
                    if (canClimb)
                    {
                        if (LadderEntry != null)
                        {
                            Player.transform.position = LadderEntry.transform.position;
                            Player.transform.rotation = LadderEntry.transform.rotation;
                            PM.isClimbing = true;
                        }
                        PM.isClimbing = true;
                        Debug.Log("ClimbingLadder");
                    }
                    if (canPickUp)
                    {
                        PickUpObjects();          
                    }
                }
            }
        }
        #endregion


    }
    private void PickUpObjects()
    {
        foreach (GameObject pickUpObject in outlineObjectsList)
        {
            OnItemPickUp(pickUpObject);       
            pickUpObject.SetActive(false);
        }
        outlineObjectsList.Clear();
        canPickUp = false;
        objectToInteract = null;

    }

    private void OutlineObjects()
    {
        if (outlineObjectsList.Count <= 0) return;
        float closestDistanceSqr = interactRange;
        Vector3 playerPosition = Player.transform.position;

        foreach (GameObject objectToOutline in outlineObjectsList)
        {
            Vector3 directionToTarget = objectToOutline.transform.position - playerPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                objectToInteract = objectToOutline;
                closestDistanceSqr = dSqrToTarget;
                OutlineObject(objectToOutline);
            }
            if (dSqrToTarget > closestDistanceSqr)
            {
                //Debug.Log("Object not highlighted");
                StopOutliningObject(objectToOutline);
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
        //if (other.CompareTag("Rock") || other.CompareTag("Stick") ||
        //    other.CompareTag("Mushroom") || other.CompareTag("Pebble"))
        //{
        //    other.GetComponent<Outline>().enabled = true;
        //    canPickUp = true;
        //    AddOutline(other.gameObject);
        //}
        //Breakable Objects
        if (other.CompareTag("Rock") || other.CompareTag("BreakableWall") || other.CompareTag("MinableObject"))
        {
            objectToInteract = other.gameObject;
            //other.GetComponent<Outline>().enabled = true;
            //outlineObjectsList.Add(objectToInteract);
        }

        if(other.CompareTag("RollingRock"))
        {
            objectToInteract = other.gameObject;
        }

        if(other.CompareTag("Ladder") || other.CompareTag("Bridge") || other.CompareTag("Bonfire"))
        {
            OutlineObject(other.gameObject.GetComponentInParent<ObjectBuild>().gameObject);
        }

        if (other.CompareTag("Ladder") || other.CompareTag("Bridge"))
        {
            objectToDestroy = other.gameObject.GetComponent<BuildObjectTrigger>().ObjectMain.gameObject;
            canDestroy = true;
        }

        if (other.CompareTag("Bonfire"))
        {
            atBonfire = true;
            objectToInteract = other.gameObject;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock") || other.CompareTag("Stick") || other.CompareTag("Mushroom") || other.CompareTag("Pebble"))
        {
            DisableInteractions();
            RemoveOutline(other.gameObject);
        }
        if (other.CompareTag("Rock") || other.CompareTag("BreakableWall") || other.CompareTag("MinableObject") || other.CompareTag("RollingRock"))
        {
            //outlineObjectsList.Remove(other.gameObject);
            //canBreak = false;
            //objectToInteract.GetComponent<Outline>().enabled = false;
        }

        if(other.CompareTag("RollingRock"))
        {
            DisableInteractions();
        }



        if (other.CompareTag("Ladder") || other.CompareTag("Bridge") || other.CompareTag("Bonfire"))
        {
            StopOutliningObject(other.gameObject.GetComponentInParent<ObjectBuild>().gameObject);
        }
        if (other.CompareTag("Ladder") || other.CompareTag("Bridge"))
        {
            objectToDestroy = null;
        }

        if (other.CompareTag("Ladder"))
        {
            PM.isClimbing = false;
            canClimb = false;
            canDestroy = false;
        }
        if (other.CompareTag("Bridge"))
        {
            canDestroy = false;
        }
        if (other.CompareTag("Bonfire"))
        {
            atBonfire = false;
            //StopOutliningObject(other.gameObject);
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

    public void ClearObject()
    {
        Destroy(objectToInteract);
        objectToInteract = null;
    }
    public void DestroyObject()
    {
        if(objectToDestroy != null)
        {

        Debug.Log(objectToDestroy);
        Destroy(objectToDestroy);
        objectToDestroy = null;
        canDestroy = false;
        canClimb = false;
        UI.UpdateMaterialsCollected();
        }
    }

    private void OutlineObject(GameObject objectToOutline)
    {
        objectToOutline.GetComponent<Outline>().enabled = true;
    }
    private void StopOutliningObject(GameObject objectToStopOutlining)
    {
        objectToStopOutlining.GetComponent<Outline>().enabled = false;
    }
    private void AddOutline(GameObject objectToOutline)
    {
        outlineObjectsList.Add(objectToOutline.gameObject);
        OutlineCheck();
    }
    private void RemoveOutline(GameObject objectToOutline)
    {
        objectToOutline.GetComponent<Outline>().enabled = false;
        outlineObjectsList.Remove(objectToOutline.gameObject);
        OutlineCheck();
    }
}
