using UnityEngine;
using System;

public class PlayerManager : GameBehaviour<PlayerManager>
{
    public bool isInteracting;
    public bool isClimbing;

    public bool playerIsStealthed;

    private void Awake()
    {
        PlayerTrigger.OnPlayerStealth += PlayerStealth;
        PlayerTrigger.OnPlayerUnstealth += PlayerUnstealth;

        isClimbing = false;
    }

    private void PlayerStealth()
    {
        playerIsStealthed = true;
        Debug.Log(playerIsStealthed);
    }
    private void PlayerUnstealth()
    {
        playerIsStealthed = false;
        Debug.Log(playerIsStealthed);
    }
}
