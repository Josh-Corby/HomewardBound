using UnityEngine;
using System;

public class PlayerManager : GameBehaviour<PlayerManager>
{
    CameraManager cameraManager;

    public bool isInteracting;
    public bool isClimbing;

    public bool playerIsStealthed;

    private void Awake()
    {
        PlayerTrigger.OnPlayerStealth += PlayerStealth;
        PlayerTrigger.OnPlayerUnstealth += PlayerUnstealth;

        cameraManager = FindObjectOfType<CameraManager>();
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
