using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : GameBehaviour<DialogueManager>
{
    private GameObject dialoguePanel;
    private TMP_Text currentNPC_Name_Text;
    private TMP_Text current_NPC_Dialogue_Text;

    [SerializeField]
    private Sprite[] dialoguePanelBackgrounds;

    private NPCDialog currentNPC;
    [SerializeField]
    private string currentNPC_Name;
    [SerializeField]
    private string[] currentNPC_Dialogue;
    [SerializeField]
    private int currentSentence_Index;
    [SerializeField]
    private string currentSentence;
    [SerializeField]
    private bool isSentenceOver;
    [SerializeField]
    private float wordSpeed;

    private bool isInDialogue;
    
    public bool isConversationStarted;

    [SerializeField]
    private CameraTransform cam;

    private Coroutine typing;

    private void Awake()
    {
        dialoguePanel = UI.DialoguePanel;
        currentNPC_Name_Text = UI.currentNPC_Name_Text;
        current_NPC_Dialogue_Text = UI.current_NPC_Dialogue_Text;
    }
    private void Start()
    {
        isSentenceOver = false;
    }
    private void Update()
    {
        if (currentNPC != null)
        {

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

        SetDialogueCharacterImage();
        
    }

    private void SetDialogueCharacterImage()
    {
        if(currentNPC.type == NPCType.Ant)
        {
            dialoguePanel.GetComponent<Image>().sprite = dialoguePanelBackgrounds[0];
        }

        if(currentNPC.type == NPCType.Spider)
        {
            dialoguePanel.GetComponent<Image>().sprite = dialoguePanelBackgrounds[1];
        }
    }

    private void StartNextSentence()
    {
        currentSentence = currentNPC_Dialogue[currentSentence_Index];
        typing = StartCoroutine(Typing());
    }

    public void ClearNPCInformation()
    {
        currentNPC = null;
        currentNPC_Name = null;
        currentNPC_Dialogue = null;
        currentSentence = null;
    }

    private void EnablePanel()
    {
        dialoguePanel.SetActive(true);
        isConversationStarted = true;
    }

    public void StartDialogue()
    {
        currentNPC.EnableOutline();
        currentNPC._GUITween.ScaleDown();
        isInDialogue = true;
        EnablePanel();
        StartNextSentence();  
    }

    public void EndDialogue()
    {
        if (isInDialogue)
        {
            
            currentSentence = "";
            currentNPC.DisableOutline();
            currentNPC._GUITween.ScaleUp();
            ClearNPCInformation();
            current_NPC_Dialogue_Text.text = "";
           
            dialoguePanel.SetActive(false);
            isConversationStarted = false;
            isSentenceOver = true;
            isInDialogue = false;
            currentSentence_Index = 0;
           
            StopCoroutine(typing);
        }
    }

    IEnumerator Typing()
    {
        Debug.Log("is typing");
        isSentenceOver = false;
        wordSpeed = 0.02f;
        current_NPC_Dialogue_Text.text = "";
        foreach (char letter in currentSentence.ToCharArray())
        {
            
            if (currentSentence == "")
            {
                yield return null;
            }
            current_NPC_Dialogue_Text.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isSentenceOver = true;
    }

}
