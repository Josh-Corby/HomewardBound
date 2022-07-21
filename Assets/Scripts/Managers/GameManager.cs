using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : GameBehaviour<GameManager>
{
    public int rocksCollected;
    public int sticksCollected;
    public int mushroomsCollected;
    public int pebblesCollected;
    public GameObject Player;
    public Transform spawnPoint;

    public GameObject pebblePrefab;

    public bool havePickaxe = false;
    public bool haveSlingshot = false;
    public bool haveGlider = false;
    public bool haveGrappleHook = false;


    private void Start()
    {
        RespawnPlayer();
        
    }

    public void RespawnPlayer()
    {
        Player.GetComponent<CharacterController>().enabled = false;
        Player.transform.position = spawnPoint.transform.position;
        Player.GetComponent<CharacterController>().enabled = true;
        Debug.Log("Player Respawned");
    }
}
