using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : GameBehaviour<UIManager>
{
    public TMP_Text flashLightIntensity;
    public TMP_Text canBuild;
    public TMP_Text fallTimer;

    public TMP_Text smallRocksCollected;
    public TMP_Text sticksCollected;
    public TMP_Text mushroomsCollected;

    public GameObject gameUI;
    public GameObject buildPanel;
    public bool buildPanelStatus;

    public float timeScale;

    private void Start()
    {
        gameUI.SetActive(!buildPanelStatus);
        buildPanelStatus = false;
        UpdateCanBuildText(false);
        buildPanel.SetActive(buildPanelStatus);
    }
    private void Update()
    {
        flashLightIntensity.text = "Light Power: " + 
            FL.myLight.intensity.ToString("F2") + " /10";

        fallTimer.text = "Fall timer: " +  PL.fallTimer.ToString();

        ToggleBuildMenu();
    }

    #region Text Updaters
    public void UpdatePebblesCollected()
    {
        smallRocksCollected.text = "Pebbles Collected: " + GM.pebblesCollected.ToString();
    }

    public void UpdateSticksCollected()
    {
        sticksCollected.text = "Sticks Collected: " + GM.sticksCollected.ToString();
    }

    public void UpdateMushroomsCollected()
    {
        mushroomsCollected.text = "Mushrooms Collected: " + GM.mushroomsCollected.ToString();
    }

    public void UpdateCanBuildText(bool canBuild)
    {
        if (!canBuild)
        {
            this.canBuild.text = "";
        }
        if (canBuild)
        {
            this.canBuild.text = "Build";
        }
    }

    public void UpdateCanBuildText(string text)
    {
        canBuild.text = text;
    }
    #endregion

    public void ToggleBuildMenu()
    {
        if (IM.buildMenu_Input)
        {
            
            buildPanelStatus = !buildPanelStatus;
            gameUI.SetActive(!buildPanelStatus);
            buildPanel.SetActive(buildPanelStatus);
            IM.buildMenu_Input = false;
            if (buildPanelStatus)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
            }

            if (!buildPanelStatus)
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
                
        }
    }
}
