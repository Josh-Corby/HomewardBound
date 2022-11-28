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
    public int mushroomsCollected;
    public int pebblesCollected;


    public GameObject Player;
    public Transform spawnPoint;
    public GameObject pebblePrefab;

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

    public void AddMaterials(int sticks, int rocks, int mushrooms)
    {
        sticksCollected += sticks;
        rocksCollected += rocks;
        mushroomsCollected += mushrooms;
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
        if (resourceCollected.CompareTag("Mushroom"))
        { 
            mushroomsCollected += 1;
            UI.UpdateMaterials(UI.mushroomsCollected,mushroomsCollected);
            OnMaterialsUpdated?.Invoke();
            return;
        }
        if (resourceCollected.CompareTag("Pebble"))
        { 
            pebblesCollected += 1;
            UI.UpdateMaterials(UI.pebblesCollected, pebblesCollected);
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
        TPM.LilypadOffset = null;
        Player.GetComponent<CharacterController>().enabled = true;

        TPM.LilypadOffset = null;
        TPM.fallTimer = TPM.fallTimerMax;
        TPM.enabled = true;
        TPM.StopHookshot();

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
