using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : GameBehaviour<GameManager>
{
    [Header("Resources Collected")]
    public int rocksCollected;
    public int sticksCollected;
    public int mushroomsCollected;
    public int pebblesCollected;


    public GameObject Player;
    public Transform spawnPoint;
    public GameObject pebblePrefab;

    [Header("Bools for what tools the player has available")]
    public bool havePickaxe = false;
    public bool haveSlingshot = false;
    public bool haveBuilding = false;
    public bool haveGlider = false;
    public bool haveGrappleHook = false;
    

    private void Start()
    {
        RespawnPlayer();      
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RespawnPlayer();
    }

    /// <summary>
    /// Respawn the player at current spawn point position and rotation
    /// </summary>
    public void RespawnPlayer()
    {
        Player.GetComponent<CharacterController>().enabled = false;
        Player.transform.position = spawnPoint.transform.position;
        Player.transform.rotation = spawnPoint.transform.rotation;
        Player.GetComponent<CharacterController>().enabled = true;
        Debug.Log("Player Respawned");
        TPM.StopHookshot();
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
