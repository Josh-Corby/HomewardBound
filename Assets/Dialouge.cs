using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dialouge : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;

    //public KeyCode key;
    //private Button button;

    //private void Awake()
    //{
    //    button = GetComponent<Button>();
    //}

    void Update()
    {
        //if (Input.GetKeyDown(key))
        //{
        //    button.onClick.Invoke();
        //}

        if(Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        if (dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }
    }

    public void zeroText()
    {
        dialogueText.text = " ";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);

        if(index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = " ";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }

    //public TextMeshProUGUI textComponent;
    //public string[] lines;
    //public float textSpeed;

    //private int index;

    //void Start()
    //{
    //    textComponent.text = string.Empty;
    //    StartDialouge();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKey(KeyCode.E))
    //    {
    //        if (textComponent.text == lines[index])
    //        {
    //            NextLine();
    //        }
    //        else
    //        {
    //            StopAllCoroutines();
    //            textComponent.text = lines[index];
    //        }
    //    }
    //}

    //void StartDialouge()
    //{
    //    index = 0;
    //    StartCoroutine(TypeLine());
    //}

    //IEnumerator TypeLine()
    //{
    //    foreach (char c in lines[index].ToCharArray())
    //    {
    //        textComponent.text += c;
    //        yield return new WaitForSeconds(textSpeed);
    //    }
    //}

    //void NextLine()
    //{
    //    if (index < lines.Length - 1)
    //    {
    //        index++;
    //        textComponent.text = string.Empty;
    //        StartCoroutine(TypeLine());
    //    }
    //    else
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}
}