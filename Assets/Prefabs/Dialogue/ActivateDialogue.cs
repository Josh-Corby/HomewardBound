using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDialogue : MonoBehaviour
{
    public GameObject DialogueManager;
    public GameObject DialogueStart;
    public GameObject DialogueContinue;
    public bool PlayerFound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFound = true;
            DialogueManager.SetActive(true);

            DialogueContinue.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetMouseButton(0))
        {
            DialogueStart.SetActive(false);
            DialogueContinue.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFound = false;

            DialogueStart.SetActive(true);
            DialogueManager.SetActive(false);
        }
    }

}
