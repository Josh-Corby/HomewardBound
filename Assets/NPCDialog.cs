using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCDialog : GameBehaviour
{
    public string NPC_Name;
    public string[] NPC_Dialogue;
   
    [SerializeField]
    private Outline outline;

    [SerializeField]
    private GameObject UIAboveHead;
    public GUITween GUI;


    private void Awake()
    {
        outline = transform.parent.GetComponentInChildren<Outline>();
        GUI = transform.parent.GetComponentInChildren<GUITween>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            //Debug.Log("player in radius");
            DM.GetNPCInformation(this);
            DM.StartDialogue();
            //EnableOutline();
            //DisableHoverUI();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            DM.EndDialogue();
            DM.ClearNPCInformation();
            DisableOutline();
            //EnableHoverUI();
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

    public void EnableHoverUI()
    {
        UIAboveHead.SetActive(true);
    }

    public void DisableHoverUI()
    {
        UIAboveHead.SetActive(false);
    }
}
