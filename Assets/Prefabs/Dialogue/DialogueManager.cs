using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : GameBehaviour<DialogueManager>
{
    private GameObject _dialoguePanel;
    private TMP_Text _currentNPCNameText;
    private TMP_Text _currentNPCDialogueText;
    [SerializeField]
    private Sprite[] _dialoguePanelBackgrounds;
    [SerializeField]
    private NPCDialog _currentNPC;
    [SerializeField]
    private string _currentNPCName;
    [SerializeField]
    private string[] _currentNPCDialogue;
    private int _currentSentenceIndex;
    private string _currentSentence;
    private bool _isSentenceOver;
    private float _wordSpeed;
    private bool _isInDialogue;   
    public bool IsConversationStarted;
    private Coroutine typing;

    private void Awake()
    {
        _dialoguePanel = UI.DialoguePanel;
        _currentNPCNameText = UI.currentNPC_Name_Text;
        _currentNPCDialogueText = UI.current_NPC_Dialogue_Text;
    }
    private void Start()
    {
        _isSentenceOver = false;
    }
    private void Update()
    {
        if (_currentNPC != null)
        {

            if (!_isSentenceOver)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _wordSpeed = 0.005f;
                }

            if (_isSentenceOver)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _currentSentenceIndex += 1;
                    if (_currentSentenceIndex > _currentNPCDialogue.Length - 1)
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
        _currentNPC = NPC;
        _currentNPCName = _currentNPC.NPC_Name;
        _currentNPCNameText.text = _currentNPCName;
        _currentNPCDialogue = _currentNPC.NPC_Dialogue;
        _currentSentenceIndex = 0;

        SetDialogueCharacterImage();
        
    }

    private void SetDialogueCharacterImage()
    {
        if(_currentNPC.Type == NPCType.Ant)
        {
            _dialoguePanel.GetComponent<Image>().sprite = _dialoguePanelBackgrounds[0];
        }

        if(_currentNPC.Type == NPCType.Spider)
        {
            _dialoguePanel.GetComponent<Image>().sprite = _dialoguePanelBackgrounds[1];
        }
    }

    private void StartNextSentence()
    {
        _currentSentence = _currentNPCDialogue[_currentSentenceIndex];
        typing = StartCoroutine(Typing());
    }

    public void ClearNPCInformation()
    {
        _currentNPC = null;
        _currentNPCName = null;
        _currentNPCDialogue = null;
        _currentSentence = null;
    }

    private void EnablePanel()
    {
        _dialoguePanel.SetActive(true);
        IsConversationStarted = true;
    }

    public void StartDialogue()
    {
        _currentNPC.EnableOutline();
        _currentNPC.GUITween.ScaleDown();
        _isInDialogue = true;
        EnablePanel();
        StartNextSentence();  
    }

    public void EndDialogue()
    {
        if (_isInDialogue)
        {
            
            _currentSentence = "";
            _currentNPC.DisableOutline();
            _currentNPC.GUITween.ScaleUp();
            ClearNPCInformation();
            _currentNPCDialogueText.text = "";
           
            _dialoguePanel.SetActive(false);
            IsConversationStarted = false;
            _isSentenceOver = true;
            _isInDialogue = false;
            _currentSentenceIndex = 0;
           
            StopCoroutine(typing);
        }
    }

    IEnumerator Typing()
    {
        Debug.Log("is typing");
        _isSentenceOver = false;
        _wordSpeed = 0.02f;
        _currentNPCDialogueText.text = "";
        foreach (char letter in _currentSentence.ToCharArray())
        {
            
            if (_currentSentence == "")
            {
                yield return null;
            }
            _currentNPCDialogueText.text += letter;
            yield return new WaitForSeconds(_wordSpeed);
        }
        _isSentenceOver = true;
    }

}
