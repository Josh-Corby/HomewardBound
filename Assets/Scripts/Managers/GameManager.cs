using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : GameBehaviour<GameManager>
{
    public int pebblesCollected;
    public int sticksCollected;
    public int mushroomsCollected;
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
