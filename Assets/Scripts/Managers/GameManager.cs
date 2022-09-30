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

    [Header("Tools bools")]
    public bool havePickaxe;
    public bool haveSlingshot;
    public bool haveBuilding;
    public bool haveGlider;
    public bool haveGrappleHook;

    private void Start()
    {
        RespawnPlayer();
        haveGrappleHook = false;
        haveGlider = false;
        haveBuilding = false;
        haveSlingshot = false;
        havePickaxe = false;
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
