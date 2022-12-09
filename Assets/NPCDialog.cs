using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType { Ant, Spider}
public class NPCDialog : GameBehaviour
{
    public NPCType type;
    public string NPC_Name;
    public string[] NPC_Dialogue;

    [SerializeField]
    private Outline outline;

    [SerializeField]
    private GameObject GUI;
    public GUITween _GUITween;

    private int distanceToPlayer = 200;

    private void Awake()
    {
        outline = transform.parent.GetComponentInChildren<Outline>();
        _GUITween = transform.parent.GetComponentInChildren<GUITween>();
        GUI = _GUITween.gameObject;
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
        GUI.SetActive(Vector3.Distance(gameObject.transform.position, TPM.gameObject.transform.position) <= distanceToPlayer);
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
        GUI.SetActive(true);
    }

    public void DisableHoverUI()
    {
        GUI.SetActive(false);
    }
}
