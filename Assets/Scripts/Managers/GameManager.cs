using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : GameBehaviour<GameManager>
{
    public int smallRocksCollected = 0;
    public GameObject SquarePrefab;

    public void BuildSquare()
    {
        if(smallRocksCollected >= 4)
        {

        }
    }
}
