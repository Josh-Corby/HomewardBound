using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class InteractionZone : GameBehaviour<InteractionZone>
{
    public static event Action OnRespawnSet;
    public static event Action<GameObject> OnItemPickUp;

    public bool CanPickUp;
    public bool CanDestroy;
    public bool CanClimb;
    public bool CanBreak;
    private bool AtBonfire;
    public GameObject Player;
    public GameObject objectToInteract;
    public GameObject objectToDestroy;
    private readonly GameObject _ladderEntry;
    public int LightPickUpValue = 8;

    private int _pebbleCounter = 0;

    public List<GameObject> OutlineObjectsList = new List<GameObject>();

    private readonly float _interactRange = 3f;

    private Collider _col;
    private void Awake()
    {
        Player = TPM.gameObject;
        _col = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        if (OutlineObjectsList.Count > 0)
            //OutlineCheck();

        if (OutlineObjectsList.Count > 0)
            CanPickUp = true;
        if (OutlineObjectsList.Count < 0)
            CanPickUp = false;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CanDestroy && UI.paused == false)
            {
                objectToDestroy.GetComponent<ObjectBuild>().RefundMaterials();


                if (objectToDestroy.CompareTag("Ladder"))
                {
                    LC.Inside = false;
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

                objectToInteract.transform.parent.parent = null;

                _col.enabled = true;






                if (AtBonfire)
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
                        _col.enabled = false;

                        return;
                    }
                }
                if (CanClimb)
                {
                    if (_ladderEntry != null)
                    {
                        Player.transform.position = _ladderEntry.transform.position;
                        Player.transform.rotation = _ladderEntry.transform.rotation;
                        PM.isClimbing = true;
                    }
                    PM.isClimbing = true;
                    Debug.Log("ClimbingLadder");
                }
                if (CanPickUp)
                {
                    PickUpObjects();
                }

            }
        }
        #endregion


    }
    private void PickUpObjects()
    {
        foreach (GameObject pickUpObject in OutlineObjectsList)
        {
            OnItemPickUp(pickUpObject);
            pickUpObject.SetActive(false);
        }
        OutlineObjectsList.Clear();
        CanPickUp = false;
        objectToInteract = null;

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
            CanDestroy = true;
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
            CanClimb = false;
            CanDestroy = false;

        }
        if (other.CompareTag("Bridge"))
        {
            CanDestroy = false;
        }
        if (other.CompareTag("Bonfire"))
        {
            AtBonfire = false;
        }
    }

    public void DisableInteractions()
    {
        TogglePickUpBools();
    }

    public void TogglePickUpBools()
    {
        objectToInteract = null;
        CanDestroy = false;
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
            CanDestroy = false;
            CanClimb = false;
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

    /*
    private void AddOutline(GameObject objectToOutline)
    {
        OutlineObjectsList.Add(objectToOutline.gameObject);
    }
    private void RemoveOutline(GameObject objectToOutline)
    {
        objectToOutline.GetComponent<Outline>().enabled = false;
        OutlineObjectsList.Remove(objectToOutline.gameObject);
    }

    private void OutlineObjects()
    {
        if (OutlineObjectsList.Count <= 0) return;
        float closestDistanceSqr = _interactRange;
        Vector3 playerPosition = Player.transform.position;

        foreach (GameObject objectToOutline in OutlineObjectsList)
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
                StopOutliningObject(objectToOutline);
            }
        }
    }
    */
}
