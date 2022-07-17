using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : GameBehaviour<GameManager>
{
    public int pebblesCollected = 0;
    public int sticksCollected = 0;
    public int mushroomsCollected = 0;
    public GameObject Player;
    public Transform spawnPoint;

    
    private void Start()
    {
        RespawnPlayer();
        
    }

    public void RespawnPlayer()
    {
        Player.transform.position = spawnPoint.transform.position;
        Debug.Log("Player Respawned");
    }
}
