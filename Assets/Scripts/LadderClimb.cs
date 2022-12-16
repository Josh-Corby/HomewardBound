using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LadderClimb : GameBehaviour<LadderClimb>
{
    public static event Action<bool> OnLadderStateChange = null;
    public static event Action<bool> OnClimbingStateChange = null;

    public Transform PlayerController;
    public bool Inside;
    public float SpeedUpDown = 3.2f;
    public ThirdPlayerMovement TPMMovement;
    void Start()
    {
        Inside = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "LadderClimb")
        {
            OnLadderStateChange?.Invoke(true);
            TPM.groundState = GroundStates.Grounded;
            TPM.enabled = false;
            Inside = true;
            TPM.StopSprinting();
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "LadderClimb")
        {
            AM.SetClimbing(false);
            Debug.Log("Not On Ladder");
            OnLadderStateChange?.Invoke(false);
            TPM.enabled = true;
            TPM.groundState = GroundStates.Grounded;
            Inside = false;
        }
    }

    private void LateUpdate()
    {
        if (Inside == true && Input.GetKey("w"))
        {
            PlayerController.transform.position += Vector3.up / SpeedUpDown;
            OnClimbingStateChange?.Invoke(true);
            return;
        }

        if (Inside == true && Input.GetKey("s"))
        {
            PlayerController.transform.position += Vector3.down / SpeedUpDown;
            OnClimbingStateChange?.Invoke(true);
            return;
        }

        else
        {
            OnClimbingStateChange?.Invoke(false);
        }
    }
}