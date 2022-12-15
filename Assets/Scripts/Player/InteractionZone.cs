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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canDestroy && UI.paused == false)
            {
                objectToDestroy.GetComponent<ObjectBuild>().RefundMaterials();


                if (objectToDestroy.CompareTag("Ladder"))
                {
                    LC.inside = false;
                    TPM.enabled = true;
                    AM.SetOnLadder(false);
                }
                DestroyObject();
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



        if (other.CompareTag("Ladder") || other.CompareTag("Bridge"))
        {
            OutlineObject(other.gameObject.GetComponentInParent<ObjectBuild>().gameObject);
        }

        if (other.CompareTag("Ladder") || other.CompareTag("Bridge"))
        {
            objectToDestroy = other.gameObject.GetComponent<BuildObjectTrigger>().ObjectMain.gameObject;
            canDestroy = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("Ladder") || other.CompareTag("Bridge"))
        {
            StopOutliningObject(other.gameObject.GetComponentInParent<ObjectBuild>().gameObject);
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
        if (objectToDestroy != null)
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
