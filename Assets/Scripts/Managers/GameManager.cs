using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class GameManager : GameBehaviour<GameManager>
{
    public static event Action OnMaterialsUpdated = null;
    public static event Action OnPlayerRespawn = null;

    [Header("Resources Collected")]
    public int rocksCollected;
    public int sticksCollected;
    public int stringCollected;
    public int pebblesCollected;


    public GameObject Player;
    public Transform spawnPoint;

    [Header("Tools bools")]
    public bool havePickaxe;
    public bool haveSlingshot;
    public bool haveBuilding;
    //public bool haveGlider;
    public bool haveGrappleHook;

    private void Start()
    {
        RespawnPlayer();
        haveGrappleHook = false;
        //haveGlider = false;
        haveBuilding = false;
        haveSlingshot = false;
        havePickaxe = false;

        InteractionZone.OnItemPickUp += IncreaseResources;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            haveGrappleHook = true;
            //haveGlider = true;
            haveBuilding = true;
            haveSlingshot = true;
            havePickaxe = true;

            pebblesCollected += 1000;
            AddMaterials(1000, 1000, 1000);
            SS.UpdateAmmo();
        }

    }

    public void AddMaterials(int sticks, int rocks, int strings)
    {
        sticksCollected += sticks;
        rocksCollected += rocks;
        stringCollected += strings;
        OnMaterialsUpdated?.Invoke();
    }

   
    public void IncreaseResources(GameObject resourceCollected)
    {
        //Debug.Log(resourceCollected);

        if (resourceCollected.CompareTag("Rock"))
        { 
            rocksCollected += 1;
            UI.UpdateMaterials(UI.smallRocksCollected,rocksCollected);
            OnMaterialsUpdated?.Invoke();
            return;
        }
        if (resourceCollected.CompareTag("Stick"))
        { 
            sticksCollected += 1;
            UI.UpdateMaterials(UI.sticksCollected,sticksCollected);
            OnMaterialsUpdated?.Invoke();
            return;
        }
        if (resourceCollected.CompareTag("String"))
        { 
            stringCollected += 1;
            UI.UpdateMaterials(UI.stringCollected,stringCollected);
            OnMaterialsUpdated?.Invoke();
            return;
        }
        if (resourceCollected.CompareTag("Pebble"))
        { 
            pebblesCollected += 1;
            //UI.UpdateMaterials(UI.pebblesCollected, pebblesCollected);
            OnMaterialsUpdated?.Invoke();
            SS.UpdateAmmo();
            
            return;
        }       
    }

    /// <summary>
    /// Respawn the player at current spawn point position and rotation
    /// </summary>
    public void RespawnPlayer()
    {
        Player = TPM.gameObject;
        Player.GetComponent<CharacterController>().enabled = false;
        Player.transform.position = spawnPoint.transform.position;
        Player.transform.rotation = spawnPoint.transform.rotation;

        Player.GetComponent<CharacterController>().enabled = true;

        TPM.fallTimer = TPM.fallTimerMax;
        TPM.enabled = true;


        PM.isClimbing = false;
        LC.inside = false;
        //Debug.Log("Player Respawned");
        BM.CancelBuilding();


        OnPlayerRespawn?.Invoke();
    }



    /// <summary>
    /// Set spawn point of player
    /// </summary>
    /// <param name="SP"> Transform of spawn point to be changed to</param>
    public void SetSpawnPoint(Transform SP)
    {
        spawnPoint = SP;
    }

}
