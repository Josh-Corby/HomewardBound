using UnityEngine;
using System;

public class PlayerManager : GameBehaviour<PlayerManager>
{

    public static event Action<int> OnToolSelected = null;

    public bool IsInteracting;
    public bool isClimbing;
    public bool playerIsStealthed;


    public GameObject pickUpSpot;
    private void Awake()
    {

        isClimbing = false;
    }


    private void Update()
    {
 

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (GM.haveBuilding)
            { OnToolSelected(1); }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (GM.haveBuilding)
            { OnToolSelected(2); }
        }

    

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (GM.haveSlingshot)
            { OnToolSelected(3); }
        }

    
    }

    private void PlayerStealth()
    { playerIsStealthed = true; }
    private void PlayerUnstealth()
    { playerIsStealthed = false; }
}
