using UnityEngine;
using System;

public class PlayerManager : GameBehaviour<PlayerManager>
{

    public static event Action<int> OnToolSelected;


    public bool isInteracting;
    public bool isClimbing;
    public bool playerIsStealthed;



    private void Awake()
    {
        PlayerTrigger.OnPlayerStealth += PlayerStealth;
        PlayerTrigger.OnPlayerUnstealth += PlayerUnstealth;

        isClimbing = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { OnToolSelected(0); }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        { OnToolSelected(1); }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        { OnToolSelected(2); }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        { OnToolSelected(3); }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        { OnToolSelected(4); }

        if(Input.GetKeyDown(KeyCode.Alpha6))
        { OnToolSelected(5); }
    }

    private void PlayerStealth()
    { playerIsStealthed = true; }
    private void PlayerUnstealth()
    { playerIsStealthed = false; }
}
