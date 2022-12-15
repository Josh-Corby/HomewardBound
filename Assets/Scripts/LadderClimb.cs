using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LadderClimb : GameBehaviour<LadderClimb>
{
    public static event Action<bool> OnLadderStateChange = null;
    public static event Action<bool> OnClimbingStateChange = null;

    public Transform PlayerController;
    public bool inside = false;
    public float speedUpDown = 3.2f;
    public ThirdPlayerMovement TPMMovement;
    void Start()
    {
        inside = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "LadderClimb")
        {
            OnLadderStateChange?.Invoke(true);
            TPM.groundState = GroundStates.Grounded;
            TPM.enabled = false;
            inside = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "LadderClimb")
        {
            Debug.Log("Not On Ladder");
            OnLadderStateChange?.Invoke(false);
            TPM.enabled = true;
            TPM.groundState = GroundStates.Grounded;
            inside = false;
        }
    }

    private void LateUpdate()
    {
        if (inside == true && Input.GetKey("w"))
        {
            PlayerController.transform.position += Vector3.up / speedUpDown;
            OnClimbingStateChange?.Invoke(true);
            return;
        }

        if (inside == true && Input.GetKey("s"))
        {
            PlayerController.transform.position += Vector3.down / speedUpDown;
            OnClimbingStateChange?.Invoke(true);
            return;
        }

        else
        {
            OnClimbingStateChange?.Invoke(false);
        }
    }
}