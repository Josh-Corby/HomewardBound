using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class IntroSequence : GameBehaviour
{
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private GameObject titleScreenCanvas;

    [SerializeField]
    private Image buttonPanelColour;
    [SerializeField]
    private Image backgroundColour;

    [SerializeField]
    private Color emptyColour;

    [SerializeField]
    private GameObject buttonPanel;

    private bool lerpColour = false;

    private void Start()
    {
        videoPlayer.loopPointReached += ChangeToGameScene;
    }
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (lerpColour)
        {
            LerpColour();
        }
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
        buttonPanel.SetActive(false);
        lerpColour = true;
        titleText.SetActive(false);
    }
    void ChangeToGameScene(VideoPlayer videoPlayer)
    {
        SC.LoadScene("EricScene");
    }

    private void LerpColour()
    {
        buttonPanelColour.color = Color.Lerp(buttonPanelColour.color, emptyColour, 0.01f);
        backgroundColour.color = Color.Lerp(backgroundColour.color, emptyColour, 0.01f);
    }

       
}