using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLilypadTrigger : GameBehaviour
{
    
    public GameObject playerFollowObject;

    [SerializeField]
    private GameObject Player;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("Player on lilypad");
            playerFollowObject.transform.position = new Vector3(TPM.transform.position.x, transform.position.y, TPM.transform.position.z);
            TPM.LilypadOffset = playerFollowObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            TPM.LilypadOffset = null;
        }
    }



  
}