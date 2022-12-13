using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : GameBehaviour<LadderClimb>
{

    public Transform PlayerController;
    public bool inside = false;
    public float speedUpDown = 3.2f;
    public ThirdPlayerMovement TPMMovement;
    [SerializeField]
    private bool isClimbing;
    void Start()
    {
        inside = false;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "LadderClimb")
        {
            AM.isOnLadder = true;
            TPM.groundState = GroundStates.Grounded;
            TPM.enabled = false;
            inside = !inside;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "LadderClimb")
        {
            AM.isOnLadder = false;
            TPM.enabled = true;
            TPM.groundState = GroundStates.Grounded;
            inside = !inside;
        }
    }

    private void LateUpdate()
    {
        if (inside == true && Input.GetKey("w"))
        {
            PlayerController.transform.position += Vector3.up / speedUpDown;
            isClimbing = true;
            SendIsClimbing();
            return;
            //Debug.Log("Climbing Up");
        }

        if (inside == true && Input.GetKey("s"))
        {
            PlayerController.transform.position += Vector3.down / speedUpDown;
            //Debug.Log("Climbing Down");
            isClimbing = true;
            SendIsClimbing();
            return;
        }

        else
        {
            isClimbing = false;
            SendIsClimbing();
        }
    }

    private void SendIsClimbing()
    {
        AM.isClimbing = isClimbing;
        AM.SetAnimationBool("isClimbing", AM.isClimbing);
    }

}