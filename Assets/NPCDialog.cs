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
        UIAboveHead = GUI.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            DM.GetNPCInformation(this);
            DM.StartDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            DM.EndDialogue();
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

    public void EnableHoverUI()
    {
        UIAboveHead.SetActive(true);
    }

    public void DisableHoverUI()
    {
        UIAboveHead.SetActive(false);
    }
}
