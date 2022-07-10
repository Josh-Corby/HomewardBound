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

    public Button buildLadderButton;
    public Button buildBridgeButton;
    public Button buildGliderButton;


    

    public float timeScale;

    private void Start()
    {
        gameUI.SetActive(true);
        buildPanelStatus = false;
        UpdateCanBuildText(false);
        buildPanel.SetActive(false);
    }
    private void Update()
    {
        flashLightIntensity.text = "Light Power: " + 
            FL.myLight.intensity.ToString("F2") + " /10";

        fallTimer.text = "Fall timer: " +  PL.fallTimer.ToString();

        ToggleBuildMenu();
    }

    #region Text Updaters

    public void UpdateMaterialsCollected()
    {
        UpdatePebblesCollected();
        UpdateSticksCollected();
        UpdateMushroomsCollected();
    }
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

    #region Button Updaters

    public void IsButtonClickable()
    {
        buildLadderButton.interactable = BM.LadderCheck();
        buildBridgeButton.interactable = BM.BridgeCheck();
        buildGliderButton.interactable = BM.GliderCheck();
    }
    #endregion
    public void ToggleBuildMenu()
    {
        
        if (BM.haveGlider)
        {
            buildGliderButton.interactable = false;
        }

        if (IM.buildMenu_Input)
        {
            IsButtonClickable();
            BuildMenuToggle();
            
            
            IM.buildMenu_Input = false;
        
        }
    }

    public void BuildMenuToggle()
    {
        buildPanelStatus = !buildPanelStatus;
        buildPanel.SetActive(buildPanelStatus);
        gameUI.SetActive(!buildPanelStatus);

        if (buildPanelStatus)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;

        }

        if (!buildPanelStatus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;

        }
    }
}
