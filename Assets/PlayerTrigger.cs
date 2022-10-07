using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerTrigger : GameBehaviour
{
    [SerializeField]
    private GameObject Player;

    public static event Action OnPlayerStealth;
    public static event Action OnPlayerUnstealth;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TallGrass"))
        {
            Debug.Log("Player is stealthed");
            OnPlayerStealth();
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TallGrass"))
        {
            Debug.Log("Player is unstealthed");
            OnPlayerUnstealth();
        }

    }
}
