using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCDialog : GameBehaviour
{
    public string NPC_Name;
    public string[] NPC_Dialogue;


    private bool isPlayerInRange;
    //[SerializeField]
    //private GameObject UIAboveHead;

    private bool informationSent;

    private void Update()
    {
        if (!isPlayerInRange) return;

        if (!informationSent)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                
                informationSent = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            Debug.Log("player in radius");
            DM.GetNPCInformation(this);
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            DM.EndDialogue();
            isPlayerInRange = false;
        }
    }

    //private void EnableHoverUI()
    //{
    //    UIAboveHead.SetActive(true);
    //}

    //private void DisableHoverUI()
    //{
    //    UIAboveHead.SetActive(false);
    //}
}
