using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDialogue : MonoBehaviour
{
    public GameObject dialogueManager;
    public bool playerFound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerFound = true;
            dialogueManager.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerFound = false;
            dialogueManager.SetActive(false);
        }
    }

}
