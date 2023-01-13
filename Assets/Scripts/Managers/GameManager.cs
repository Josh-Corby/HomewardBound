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
    public int RocksCollected;
    public int SticksCollected;
    public int StringCollected;
    public int PebblesCollected;


    public GameObject Player;
    public Transform SpawnPoint;

    [Header("Tools bools")]
    public bool HaveSlingshot;
    public bool HaveBuilding;

    [SerializeField]
    private bool CheatCodes;
    private void Start()
    {
        Player = TPM.gameObject;
        RespawnPlayer();
        HaveBuilding = false;
        HaveSlingshot = false;
        InteractionZone.OnItemPickUp += IncreaseResources;
    }

    private void Update()
    {
        if (CheatCodes)
        {

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                HaveBuilding = true;
                HaveSlingshot = true;

                PebblesCollected += 1000;
                AddMaterials(1000, 1000, 1000);
                SS.UpdateAmmo();
            }
        }

    }

    public void AddMaterials(int sticks, int rocks, int strings)
    {
        SticksCollected += sticks;
        RocksCollected += rocks;
        StringCollected += strings;
        OnMaterialsUpdated?.Invoke();
    }


    public void IncreaseResources(GameObject resourceCollected)
    {
        if (resourceCollected.CompareTag("Rock"))
        {
            RocksCollected += 1;
            UI.UpdateMaterials(UI.smallRocksCollected, RocksCollected);
            OnMaterialsUpdated?.Invoke();
            return;
        }
        if (resourceCollected.CompareTag("Stick"))
        {
            SticksCollected += 1;
            UI.UpdateMaterials(UI.sticksCollected, SticksCollected);
            OnMaterialsUpdated?.Invoke();
            return;
        }
        if (resourceCollected.CompareTag("String"))
        {
            StringCollected += 1;
            UI.UpdateMaterials(UI.stringCollected, StringCollected);
            OnMaterialsUpdated?.Invoke();
            return;
        }
        if (resourceCollected.CompareTag("Pebble"))
        {
            PebblesCollected += 1;
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
        if (SpawnPoint != null)
        {
            Player.GetComponent<CharacterController>().enabled = false;
            Player.transform.position = SpawnPoint.transform.position;
            Player.transform.rotation = SpawnPoint.transform.rotation;
            Player.GetComponent<CharacterController>().enabled = true;
            TPM.fallTimer = TPM.fallTimerMax;
            TPM.enabled = true;
            PM.isClimbing = false;
            LC.Inside = false;
            BM.CancelBuilding();
            OnPlayerRespawn?.Invoke();
        }

    }


    /// <summary>
    /// Set spawn point of player
    /// </summary>
    /// <param name="SP"> Transform of spawn point to be changed to</param>
    public void SetSpawnPoint(Transform SP)
    {
        SpawnPoint = SP;
    }

}
