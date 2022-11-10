using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCDialog : GameBehaviour
{
    public string NPC_Name;
    public string[] NPC_Dialogue;
   
    [SerializeField]
    private Outline outline;

    //[SerializeField]
    //private GameObject UIAboveHead;

    private void Awake()
    {
        outline = GetComponentInParent<Outline>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            //Debug.Log("player in radius");
            DM.GetNPCInformation(this);
            DM.StartDialogue();
            //EnableOutline();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            DM.EndDialogue();
            DM.ClearNPCInformation();
            DisableOutline();
        }
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void DisableOutline()
    {
        outline.enabled = false;
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
