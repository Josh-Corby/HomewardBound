using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType { Ant, Spider}
public class NPCDialog : GameBehaviour
{
    public NPCType Type;
    public string NPC_Name;
    public string[] NPC_Dialogue;
    [HideInInspector] public GUITween GUITween;
    private GameObject _gui;
    private Outline _outline;
    private readonly int _distanceToPlayer = 200;

    private void Awake()
    {
        _outline = transform.parent.GetComponentInChildren<Outline>();
        GUITween = transform.parent.GetComponentInChildren<GUITween>();
        _gui = GUITween.gameObject;
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

    private void Update()
    {
        _gui.SetActive(Vector3.Distance(gameObject.transform.position, TPM.gameObject.transform.position) <= _distanceToPlayer);
    }

    public void EnableOutline()
    {
        _outline.enabled = true;
    }

    public void DisableOutline()
    {
        _outline.enabled = false;
    }
}
