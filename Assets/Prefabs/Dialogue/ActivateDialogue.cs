using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDialogue : MonoBehaviour
{
    public GameObject dialogueManager;
    public GameObject dialogueStart;
    public GameObject dialogueContinue;
    public bool playerFound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerFound = true;
            dialogueManager.SetActive(true);

            dialogueContinue.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetMouseButton(0))
        {
            dialogueStart.SetActive(false);
            dialogueContinue.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerFound = false;

            dialogueStart.SetActive(true);
            dialogueManager.SetActive(false);
        }
    }

}
