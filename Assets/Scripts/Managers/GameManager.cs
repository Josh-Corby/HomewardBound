using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : GameBehaviour<GameManager>
{
    public int smallRocksCollected = 0;
    public GameObject Player;
    public Transform spawnPoint;
    private void Start()
    {
        //Player.transform.position = spawnPoint.transform.position;
    }
}
