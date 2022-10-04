using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Dialogue
{
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;

//    public GameObject dialoguePanel;
//    public TextMeshProUGUI dialogueText;
//    public string[] Lines;
//    private int index;

//    public GameObject contButton;
//    public float wordSpeed;
//    public bool playerIsClose;

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
//        {
//            if (dialoguePanel.activeInHierarchy)
//            {
//                StartDialouge();
//                NextLine();
//                dialoguePanel.SetActive(false);
//                contButton.SetActive(false);

//                zeroText();
//            }
//            else
//            {
//                contButton.SetActive(true);
//                dialoguePanel.SetActive(true);
//            }
//        }

//        if (dialogueText.text == Lines[index])
//        {
//            contButton.SetActive(true);
//        }
//        else
//        {
//            StopAllCoroutines();
//            dialogueText.text = Lines[index];
//        }
//    }

//    public void zeroText()
//    {
//        StopAllCoroutines();
//        dialogueText.text = " ";
//        index = 0;
//        dialoguePanel.SetActive(false);
//        contButton.SetActive(false);

//    }

//    IEnumerator Typing()
//    {
//        foreach (char letter in dialogue[index].ToCharArray())
//        {
//            dialogueText.text += letter;
//            yield return new WaitForSeconds(wordSpeed);
//        }
//    }

//    public void NextLine()
//    {
//        if (index < dialogue.Length - 1)
//        {
//            index++;
//            dialogueText.text = string.Empty;
//            StartCoroutine(Typing());
//        }
//        else
//        {
//            zeroText();
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            playerIsClose = true;
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            playerIsClose = false;
//            zeroText();
//        }
//    }

//    public TextMeshProUGUI textComponent;
//    public string[] lines;
//    public float textSpeed;

//    private int index;

//    void Start()
//    {
//        textComponent.text = string.Empty;
//        StartDialouge();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKey(KeyCode.E))
//        {
//            if (textComponent.text == lines[index])
//            {
//                NextLine();
//            }
//            else
//            {
//                StopAllCoroutines();
//                textComponent.text = lines[index];
//            }
//        }
//    }

//    void StartDialouge()
//    {
//        index = 0;
//        StartCoroutine(TypeLine());

//    }

//    IEnumerator TypeLine()
//    {
//        foreach (char c in Lines[index].ToCharArray())
//        {
//            dialogueText.text += c;
//            yield return new WaitForSeconds(wordSpeed);
//        }
//    }

//    void NextLine()
//    {
//        if (index < Lines.Length - 1)
//        {
//            index++;
//            dialogueText.text = string.Empty;
//            StartCoroutine(TypeLine());
//        }
//        else
//        {
//            gameObject.SetActive(false);
//        }
//    }
}