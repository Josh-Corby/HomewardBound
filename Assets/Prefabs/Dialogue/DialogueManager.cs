using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : GameBehaviour<DialogueManager>
{
    public static event Action OnStartTalkingToPlayer;
    public static event Action OnEndTalkingToPlayer;

    [SerializeField]
    private GameObject dialogPanel;
    [SerializeField]
    private TMP_Text currentNPC_Name_Text;
    [SerializeField]
    private TMP_Text current_NPC_Dialogue_Text;

    private NPCDialog currentNPC;
    [SerializeField]
    private string currentNPC_Name;
    [SerializeField]
    private string[] currentNPC_Dialogue;
    [SerializeField]
    private int currentSentence_Index;
    private string currentSentence;
    private bool isSentenceOver;
    [SerializeField]
    private float wordSpeed;

    private bool isInDialogue;
    
    public bool isConversationStarted;

    private void Start()
    {
        isSentenceOver = false;
    }
    private void Update()
    {
        if (currentNPC != null)
        {

            if (!isConversationStarted)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentNPC.DisableOutline();
                    StartDialogue();
                    return;
                }
            }

            if (!isSentenceOver)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    wordSpeed = 0.005f;
                }

            if (isSentenceOver)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentSentence_Index += 1;
                    if (currentSentence_Index > currentNPC_Dialogue.Length - 1)
                    {
                        
                        EndDialogue();
                        
                        return;
                    }
                    StartNextSentence();
                }
            }
        }
    }
    public void GetNPCInformation(NPCDialog NPC)
    {
        currentNPC = NPC;
        currentNPC_Name = currentNPC.NPC_Name;
        currentNPC_Name_Text.text = currentNPC_Name;
        currentNPC_Dialogue = currentNPC.NPC_Dialogue;
        currentSentence_Index = 0;
    }

    private void StartNextSentence()
    {
        currentSentence = currentNPC_Dialogue[currentSentence_Index];
        StartCoroutine(Typing());
    }

    public void ClearNPCInformation()
    {
        currentNPC = null;
        currentNPC_Name = null;
        currentNPC_Dialogue = null;
    }

    private void EnablePanel()
    {
        dialogPanel.SetActive(true);
        isConversationStarted = true;
    }

    private void StartDialogue()
    {
        isInDialogue = true;
        EnablePanel();
        StartNextSentence();
    }

    public void EndDialogue()
    {
        if (isInDialogue)
        {
            StopCoroutine(Typing());
            current_NPC_Dialogue_Text.text = "";
            dialogPanel.SetActive(false);
            isConversationStarted = false;
            isInDialogue = false;
            currentSentence_Index = 0;
            currentNPC.EnableOutline();
        }
    }

    IEnumerator Typing()
    {
        isSentenceOver = false;
        wordSpeed = 0.1f;
        current_NPC_Dialogue_Text.text = "";
        foreach (char letter in currentSentence.ToCharArray())
        {
            current_NPC_Dialogue_Text.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isSentenceOver = true;
    }

    //    private Queue<string> sentences;

    //    public TextMeshProUGUI nameText;
    //    public TextMeshProUGUI dialogueText;
    //    public bool playerIsClose = false;
    //    public GameObject dialoguePanel;

    //    void Start()
    //    {
    //        sentences = new Queue<string>();
    //    }

    //    public void StartDialogue(Dialogue dialogue)
    //    {
    //        nameText.text = dialogue.name;

    //        sentences.Clear();

    //        foreach (string sentence in dialogue.sentences)
    //        {
    //            sentences.Enqueue(sentence);
    //        }
    //        DisplayNextSentence();
    //    }

    //    public void DisplayNextSentence()
    //    {
    //        if (sentences.Count == 0)
    //        {
    //            EndDialogue();
    //            return;
    //        }

    //        string sentence = sentences.Dequeue();
    //        StopAllCoroutines();
    //        StartCoroutine(Typing(sentence));
    //    }

    //    IEnumerator Typing(string sentence)
    //    {
    //        dialogueText.text = "";
    //        foreach (char letter in sentence.ToCharArray())
    //        {
    //            dialogueText.text += letter;
    //            yield return new WaitForSeconds(wordSpeed);
    //        }
    //    }

    //    public void EndDialogue()
    //    {
    //        Debug.Log("End of Conversation");
    //        dialoguePanel.SetActive(false);
    //    }

    //    private void OnTriggerEnter(Collider other)
    //    {
    //        if (other.CompareTag("Player"))
    //        {
    //            playerIsClose = true;
    //            dialoguePanel.SetActive(true);
    //        }
    //    }

    //    private void OnTriggerExit(Collider other)
    //    {
    //        if (other.CompareTag("Player"))
    //        {
    //            playerIsClose = false;
    //            dialoguePanel.SetActive(false);
    //        }
    //    }

}
